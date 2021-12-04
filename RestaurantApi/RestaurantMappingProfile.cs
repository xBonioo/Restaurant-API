using AutoMapper;
using RestaurantCommon.Entities;
using RestaurantLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(x => x.Address,
                c => c.MapFrom(dto => new Address()
                { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));

            CreateMap<Dish, DishDto>();

            CreateMap<User, UserDto>()
                .ForMember(m => m.Country, c => c.MapFrom(s => s.Address.Country))
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode))
                .ForMember(m => m.HouseNumber, c => c.MapFrom(s => s.Address.HouseNumber))
                .ForMember(m => m.LocalNumber, c => c.MapFrom(s => s.Address.LocalNumber));
            CreateMap<CreateUserDto, User>()
                .ForMember(x => x.Address,
                c => c.MapFrom(dto => new UserAddress()
                { Country = dto.Country, City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode, HouseNumber = dto.HouseNumber }));
            CreateMap<User, UserLittleDataDto>()
                .ForMember(m => m.Country, c => c.MapFrom(s => s.Address.Country));

            CreateMap<CreateDishDto, Dish>();
        }
    }
}
