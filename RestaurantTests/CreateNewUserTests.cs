using RestaurantCommon.Entities;
using RestaurantLogic.Models;
using RestaurantLogic.Services;
using System;
using Xunit;

namespace RestaurantTests
{
    public class CreateNewUserTests
    {
        private readonly IAccountService _accountService;

        public CreateNewUserTests(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Fact]
        public void CreateUser_FromDto_ReturnId()
        {
            // arrange 
            CreateUserDto dto = new CreateUserDto()
            {
                Email = "test@mail.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                DateOfBirth = DateTime.Now,
                Gender = true,
                Password = "12345678",
                Country = "Polska",
                City = "Bialystok",
                PostalCode = "15-000",
                Street = "Sienkiewicza",
                HouseNumber = "13",
                RoleId = 3,
            };

            // act
            var id = _accountService.Create(dto);

            // assert
            //Assert.IsType(typeof(int), id);
            Assert.IsType<int>(id);
        }
    }
}
