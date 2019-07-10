namespace VacationRental.Domain.Contact.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VacationRental.Domain.Contact.EntityFramework;
    using VacationRental.Domain.Contact.Models;

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Contact>
    {
        public ContactContext contactContext;

        public CreateContactCommandHandler(ContactContext contactContext)
        {
            this.contactContext = contactContext;
        }

        public async Task<Contact> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            await this.contactContext.AddAsync(request.Contact);

            return request.Contact;
        }
    }
}
