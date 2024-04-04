using FluentValidation;
using Models.Entities;

namespace Application.FluentValidation
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(t => t.Description).NotEmpty().NotNull().MinimumLength(5).MaximumLength(100);
            RuleFor(t => t.Subject).NotEmpty().NotNull().MinimumLength(5);
            RuleFor(t => t.TicketType).NotNull().Must(x => x >= 0);
        }
    }
}
