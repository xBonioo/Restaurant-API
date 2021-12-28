using FluentValidation;
using RestaurantCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantLogic.Models.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .MaximumLength(50);
            RuleFor(x => x.LastName)
                .MaximumLength(50);

            RuleFor(x => x.Country)
                .MaximumLength(100);
            RuleFor(x => x.City)
                .MaximumLength(100);
            RuleFor(x => x.PostalCode)
                .MaximumLength(10);
            RuleFor(x => x.Street)
                .MaximumLength(100);
            RuleFor(x => x.HouseNumber)
                .MaximumLength(10);
            RuleFor(x => x.LocalNumber)
                .MaximumLength(10);

            RuleFor(x => x.Password)
                .MinimumLength(8);
        }
    }
}
