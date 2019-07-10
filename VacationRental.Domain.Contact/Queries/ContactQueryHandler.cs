namespace VacationRental.Domain.Contact.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VacationRental.Domain.Contact.EntityFramework;
    using VacationRental.Domain.Contact.Models;

    public class ContactQueryHandler : IRequestHandler<ContactQuery, Contact>
    {
        public ContactContext contactContext;

        public ContactQueryHandler(ContactContext contactContext)
        {
            this.contactContext = contactContext;
        }

        public async Task<Contact> Handle(ContactQuery request, CancellationToken cancellationToken)
        {
            var contact = await this.contactContext.FindAsync<Contact>(request.VacationRentalId);

            return contact;
        }
    }
}
