﻿using FluentValidation;
using Models.Entities;

namespace Application.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotNull().NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(u => u.LastName).NotNull().NotEmpty().MinimumLength(3).MaximumLength(30);
            RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress(); 
            RuleFor(u=>u.Password).NotEmpty().NotNull().WithMessage("Please specify a phone number.").MinimumLength(5).MaximumLength(20); ;
            RuleFor(u=> u.PhoneNumber).NotEmpty().NotNull();
            RuleFor(u=>u.CompanyName).NotEmpty().NotNull().MinimumLength(5).MaximumLength(30);
        }
    }
}
