using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
