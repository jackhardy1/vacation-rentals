using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Contact.Api.Models.Error;
using VacationRental.Domain.Contact.Queries;

namespace VacationRental.Contact.Api.Controllers
{
    using VacationRental.Domain.Contact.Models;
    using VacationRental.Domain.Contact.Commands;

    [Route("api/v1/vacationrental/{rentalId:int}/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IMediator mediator;
        private IMapper mapper;

        public ContactController(IMediator _mediator, IMapper _mapper)
        {
            this.mediator = _mediator;
            this.mapper = _mapper;
        }

        [HttpPost]
        public async Task<Contact> Create([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            this.ValidateModel(model);

            var contact = await this.CreateContact(rentalId, model);

            return contact;
        }

        [HttpGet]
        public async Task<ContactViewModel> Get([FromRoute] int rentalId)
        {
            var contact = await GetContactInfo(rentalId);

            return contact;
        }

        [HttpPut]
        public async Task<Contact> Put([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            await this.GetContactInfo(rentalId);

            this.ValidateModel(model);

            var contact = await this.UpdateContact(rentalId, model);

            return contact;
        }

        private async Task<ContactViewModel> GetContactInfo(int rentalId)
        {
            var contact = await this.mediator.Send(new ContactQuery { VacationRentalId = rentalId });

            if (contact == null)
            {
                throw new NotFoundException();
            }

            var contactViewModel = this.mapper.Map<ContactViewModel>(contact);

            return contactViewModel;
        }

        private void ValidateModel(ContactViewModel model)
        {
            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(model, null);

                if (value == null)
                {
                    throw new BadRequestException();
                }
            }
        }

        private async Task<Contact> GetContact(int rentalId)
        {
            var contact = await this.mediator.Send(new ContactQuery { VacationRentalId = rentalId });

            return contact;
        }

        private async Task<Contact> CreateContact(int rentalId, ContactViewModel model)
        {
            var contact = this.mapper.Map<Contact>(model);
            contact.Id = rentalId;

            var createdContact = await this.mediator.Send(new CreateContactCommand { Contact = contact });

            return createdContact;
        }

        private async Task<Contact> UpdateContact(int rentalId, ContactViewModel model)
        {
            var existingContact = await this.GetContact(rentalId);

            if (existingContact != null)
            {
                throw new BadRequestException();
            }

            var contact = this.mapper.Map<Contact>(model);
            contact.Id = rentalId;

            var updatedContact = await this.mediator.Send(new UpdateContactCommand { Contact = contact });

            return updatedContact;
        }
    }
}
