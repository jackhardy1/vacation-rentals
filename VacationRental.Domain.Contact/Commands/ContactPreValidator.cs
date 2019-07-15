using FluentValidation;

namespace VacationRental.Domain.Contact.Commands
{
    using VacationRental.Domain.Contact.Models;

    public class ContactPreValidator : AbstractValidator<Contact>
    {
        public ContactPreValidator()
        {
            this.RuleFor(command => command.Phone).NotNull();
            this.RuleFor(command => command.Forename).NotNull();
            this.RuleFor(command => command.Surname).NotNull();
            this.RuleFor(command => command.AboutMe).NotNull();
            this.RuleFor(command => command.NativeLanguage).NotNull();
        }
    }
}
