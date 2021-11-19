using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCommon.Entities
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Role> Roles { get; set; }

        public RestaurantDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(r => r.Email)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(r => r.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(r => r.LastName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(r => r.DateOfBirth)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(r => r.Gender)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(r => r.AddressId)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(r => r.Weight)
                .HasColumnType("decimal(5,2)");


            modelBuilder.Entity<UserAddress>()
                .Property(r => r.Country)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<UserAddress>()
                .Property(r => r.City)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<UserAddress>()
                .Property(r => r.PostalCode)
                .IsRequired()
                .HasMaxLength(10);
            modelBuilder.Entity<UserAddress>()
                .Property(r => r.Street)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<UserAddress>()
                .Property(r => r.HouseNumber)
                .IsRequired()
                .HasMaxLength(10);
            modelBuilder.Entity<UserAddress>()
                .Property(r => r.LocalNumber)
                .HasMaxLength(10);



            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(32);

            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(r => r.City)
                .IsRequired()
                .HasMaxLength(48);

            modelBuilder.Entity<Address>()
                .Property(r => r.Street)
                .IsRequired()
                .HasMaxLength(48);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }
    }
}
