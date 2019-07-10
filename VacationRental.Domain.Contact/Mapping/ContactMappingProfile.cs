using System;
namespace VacationRental.Domain.Contact.Mapping
{
    using AutoMapper;

    public class ContactMappingProfile
    {
        public ContactMappingProfile()
        {
            this.CreateMap<Contact, ContactViewModel>()
        }
    }
}
