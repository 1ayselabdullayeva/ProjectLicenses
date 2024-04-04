using FluentValidation;
using Models.Entities;

namespace Application.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(p=>p.ProductName).NotEmpty().NotNull().WithMessage("Please enter product name");
        }

    }
}
