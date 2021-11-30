using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantCommon.Entities;
using RestaurantLogic.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

//[assembly: TestFramework("RestaurantTests.Startup", "RestaurantTests")]

namespace RestaurantTests
{
    public class Startup //: DependencyInjectionTestFramework
    {
        public Startup()
        {
        }

        //public Startup(IConfiguration configuration, IMessageSink messageSink) : base(messageSink)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<RestaurantDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("Task")));

            services.AddScoped<AccountService>();
        }
    }
}
