using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantLogic.Models
{
    public class Filter
    {
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
