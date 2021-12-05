using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantCommon.Entities;
using RestaurantCommon.Helpers;
using RestaurantCommon.Helpers.Exceptions;
using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantLogic.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
        PageResult<UserLittleDataDto> FilterUsers(Filter filter);
        UserDto GetById(int userId);
        int Create(CreateUserDto dto);
        void Delete(int userId);
        void Update(int userId, UpdateUserDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                claims.Add(
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                    );
            }

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public PageResult<UserLittleDataDto> FilterUsers(Filter filter)
        {
            var baseQuery = from u in _dbContext.Users
                            join a in _dbContext.UserAddresses on u.AddressId equals a.Id
                            where
                                (filter.LastName != null && u.LastName.ToLower().Contains(filter.LastName.ToLower())) ||
                                (filter.DateOfBirth > DateTime.MinValue && (u.DateOfBirth.Day == filter.DateOfBirth.Day && u.DateOfBirth.Month == filter.DateOfBirth.Month && u.DateOfBirth.Year == filter.DateOfBirth.Year))  ||
                                (filter.Country != null && a.Country.ToLower().Contains(filter.Country.ToLower()))
                            select new UserLittleDataDto
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                DateOfBirth = u.DateOfBirth,
                                Country = a.Country,
                            };
            
            var users = baseQuery
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToList();

            var totalItemCount = baseQuery.Count();

            var result = new PageResult<UserLittleDataDto>(users, totalItemCount, filter.PageSize, filter.PageNumber);
            return result;
        }

        public UserDto GetById(int userId)
        {
            var user = _dbContext
                .Users
                .Include(x => x.Address)
                .FirstOrDefault(d => d.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var userDtos = _mapper.Map<UserDto>(user);
            return userDtos;
        }

        public int Create(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            if (dto.RoleId.GetType() is null || dto.RoleId == 0)
            {
                user.RoleId = 3;
            }

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public void Delete(int userId)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(x => x.Id == userId);

            var address = _dbContext
                .UserAddresses
                .FirstOrDefault(x => x.Id == user.AddressId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            _dbContext.Users.Remove(user);
            _dbContext.UserAddresses.Remove(address);
            _dbContext.SaveChanges();
        }

        public void Update(int userId, UpdateUserDto dto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(r => r.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (dto.Email is not null && dto.Email != user.Email)
            {
                user.Email = dto.Email;
            }
            if (dto.FirstName is not null && dto.FirstName != user.FirstName)
            {
                user.FirstName = dto.FirstName;
            }
            if (dto.LastName is not null && dto.LastName != user.LastName)
            {
                user.LastName = dto.LastName;
            }
            if (dto.DateOfBirth.GetType() is not null && dto.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = dto.DateOfBirth;
            }
            if (dto.Gender.GetType() is not null && dto.Gender != user.Gender)
            {
                user.Gender = dto.Gender;
            }
            if (dto.Weight is not null && dto.Weight != user.Weight)
            {
                user.Weight = dto.Weight;
            }
            if (dto.RoleId.GetType() is not null && dto.RoleId != user.RoleId)
            {
                user.RoleId = dto.RoleId;
            }

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            _dbContext.SaveChanges();
        }
    }
}
