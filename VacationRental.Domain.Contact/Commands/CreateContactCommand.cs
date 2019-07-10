using MediatR;

namespace VacationRental.Domain.Contact.Commands
{
    using VacationRental.Domain.Contact.Models;

    public class CreateContactCommand : IRequest<Contact>
    {
        public Contact Contact { get; set; }
    }
}
