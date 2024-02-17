using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotNull().NotEmpty();
            RuleFor(u => u.LastName).NotNull().NotEmpty();
            RuleFor(u=>u.Email).NotEmpty().NotNull();
            RuleFor(u=>u.Password).NotEmpty().NotNull().WithMessage("Please specify a phone number."); ;
            RuleFor(u=> u.PhoneNumber).NotEmpty().NotNull();
            RuleFor(u=>u.CompanyName).NotEmpty().NotNull();
        }
    }
}
