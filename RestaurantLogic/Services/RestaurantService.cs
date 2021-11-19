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
using System.Linq.Expressions;
using RestaurantCommon.Helpers;

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
        public PageResult<RestaurantDto> GetAll(Query query)
        {
            var baseQuery = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .Where(x => query.SearchPhrase == null || (x.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || x.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            _logger.LogInformation("User get all restaurnts");

            var totalItemCount = baseQuery.Count();
            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(restaurantsDtos, totalItemCount, query.PageSize, query.PageNumber);

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
