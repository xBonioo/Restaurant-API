using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
