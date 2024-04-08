using FluentValidation;
using Models.Entities;

namespace Application.FluentValidation
{
    public class LicensesValidator:AbstractValidator<Licenses>
    {
        public LicensesValidator() 
        {
            RuleFor(x => x.UserCount).NotEmpty().NotNull();
            RuleFor(x=>x.ExpireDate).NotEmpty().NotNull();
            RuleFor(x=>x.ProductId).NotEmpty().NotNull();
        }
    }
}
