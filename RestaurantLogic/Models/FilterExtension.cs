using Microsoft.EntityFrameworkCore;
using RestaurantCommon.Entities;
using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCommon.Helpers
{
    public static class FilterExtension
    {
        public static IQueryable<User> FilterBy(this IQueryable<User> value, Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                value = value.Where(x => x.LastName.Contains(filter.LastName));
            }
            if (filter.DateOfBirth > DateTime.MinValue)
            {
                value = value.Where(x => x.DateOfBirth.Day == filter.DateOfBirth.Day && x.DateOfBirth.Month == filter.DateOfBirth.Month && x.DateOfBirth.Year == filter.DateOfBirth.Year);
            }
            if (!string.IsNullOrEmpty(filter.Country))
            {
                value = value.Where(x => x.Address.Country.Contains(filter.Country));
            }

            return value;
        }
    }
}
