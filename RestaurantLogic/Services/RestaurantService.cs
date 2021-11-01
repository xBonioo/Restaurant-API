using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantCommon.Entities;
using RestaurantCommon.Helpers.Exceptions;
using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RestaurantLogic.Services
{
    public class RestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public IEnumerable<RestaurantDto> GetAll(string searchPhrase)
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .Where(x => searchPhrase == null || (x.Name.ToLower().Contains(searchPhrase.ToLower()) || x.Description.ToLower().Contains(searchPhrase.ToLower())))
                .ToList();

            _logger.LogInformation("User get all restaurnts");

            var result = _mapper.Map<List<RestaurantDto>>(restaurants);
            return result;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            _logger.LogInformation($"User get restaurnt id: {id}");

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }
        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            _logger.LogInformation($"User create restaurnt id: {restaurant.Id} name: {restaurant.Name}");

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            _logger.LogInformation($"User delete restaurnt id: {id}");
        }

        public void Update(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _logger.LogInformation($"User update restaurnt id: {id}");

            _dbContext.SaveChanges();
        }
    }
}
