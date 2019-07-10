using System;
using MediatR;

namespace VacationRental.Domain.Contact.Queries
{
    using VacationRental.Domain.Contact.Models;

    public class ContactQuery : IRequest<Contact>
    {
        public int VacationRentalId { get; set; }
    }
}
