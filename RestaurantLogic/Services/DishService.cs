using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantCommon.Entities;
using RestaurantCommon.Helpers.Exceptions;
using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantLogic.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        IEnumerable<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
    }
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDtos;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            _logger.LogInformation($"User get dish id: {dishId} from restaurant id: {restaurantId}");

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dishEntity);
            _dbContext.SaveChanges();

            _logger.LogInformation($"User create new dish id: {dishEntity.Id} name: {dishEntity.Name} in restaurant id: {restaurantId}");

            return dishEntity.Id;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();

            _logger.LogInformation($"User delete dishes from restaurant id: {restaurantId}");
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            return restaurant;
        }
    }
}
