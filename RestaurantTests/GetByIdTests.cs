using Microsoft.EntityFrameworkCore;
using RestaurantCommon.Entities;
using RestaurantLogic.Models;
using RestaurantLogic.Services;
using System;
using System.Linq;
using Xunit;

namespace RestaurantTests
{
    public class GetByIdTests
    {
        private readonly RestaurantDbContext _dbContext;

        public GetByIdTests(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Fact]
        public void GetById_ForId_ReturnUser()
        {
            // arrange 
            int userId = 15;

            // act
            var user = _dbContext
                .Users
                .Include(x => x.Address)
                .FirstOrDefault(d => d.Id == userId);

            // assert
            Assert.IsType<User>(user);
        }
    }
}
