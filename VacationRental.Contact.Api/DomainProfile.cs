using System;
using AutoMapper;

namespace VacationRental.Contact.Api
{
    using VacationRental.Domain.Contact.Models;

    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Contact, ContactViewModel>();
            CreateMap<ContactViewModel, Contact>();
        }
    }
}
