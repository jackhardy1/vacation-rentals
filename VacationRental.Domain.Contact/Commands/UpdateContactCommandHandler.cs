namespace VacationRental.Domain.Contact.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VacationRental.Domain.Contact.EntityFramework;
    using VacationRental.Domain.Contact.Models;

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Contact>
    {
        public ContactContext contactContext;

        public UpdateContactCommandHandler(ContactContext contactContext)
        {
            this.contactContext = contactContext;
        }

        public async Task<Contact> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await this.contactContext.FindAsync<Contact>(request.Contact.Id);

            contact = request.Contact;

            await this.contactContext.SaveChangesAsync();

            return request.Contact;
        }
    }
}
