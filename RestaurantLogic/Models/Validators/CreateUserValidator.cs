using FluentValidation;
using RestaurantCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantLogic.Models.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.DateOfBirth)
                .NotEmpty();
            RuleFor(x => x.Gender)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
            RuleFor(x => x.City)
                .NotEmpty();
            RuleFor(x => x.PostalCode)
                .NotEmpty();
            RuleFor(x => x.Street)
                .NotEmpty();
            RuleFor(x => x.HouseNumber)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(8);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(a => a.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
