using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FluentValidation
{
    public class TicketValidator:AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(t => t.Description).NotEmpty().NotNull();
            RuleFor(t=>t.Subject).NotEmpty().NotNull();
            RuleFor(t=>t.TicketType).NotEmpty().NotNull();
        }
    }
}
