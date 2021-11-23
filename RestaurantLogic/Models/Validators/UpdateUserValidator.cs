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
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.DateOfBirth)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.HouseNumber)
                .NotEmpty()
                .MaximumLength(10);
            RuleFor(x => x.LocalNumber)
                .MaximumLength(10);

            RuleFor(x => x.Password)
                .MinimumLength(8);
        }
    }
}
