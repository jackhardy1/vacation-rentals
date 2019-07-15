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
    using FluentValidation;

    [Route("api/v1/vacationrental/{rentalId:int}/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IValidator<Contact> validator;
        private readonly IMapper mapper;

        public ContactController(IMediator _mediator, IValidator<Contact> _validator, IMapper _mapper)
        {
            this.mediator = _mediator;
            this.validator = _validator;
            this.mapper = _mapper;
        }

        [HttpPost]  
        public async Task<Contact> Create([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            await CheckEntityExists(rentalId);

            var contact = await this.CreateContact(rentalId, model);

            return contact;
        }

        [HttpGet]
        public async Task<ContactViewModel> Get([FromRoute] int rentalId)
        {
            var contact = await this.mediator.Send(new ContactQuery { VacationRentalId = rentalId });

            if (contact == null)
            {
                throw new NotFoundException();
            }

            var contactViewModel = this.mapper.Map<ContactViewModel>(contact);

            return contactViewModel;
        }

        [HttpPut]
        public async Task<Contact> Put([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            var existingContact = await this.mediator.Send(new ContactQuery { VacationRentalId = rentalId });

            if (existingContact == null)
            {
                throw new BadRequestException();
            }
            
            var contact = await this.UpdateContact(rentalId, model);

            return contact;
        }

        private async Task CheckEntityExists(int rentalId)
        {
            var existing = await this.mediator.Send(new ContactQuery { VacationRentalId = rentalId });

            if (existing != null)
            {
                throw new EntityAlreadyExistsException();
            }
        }

        private async Task<Contact> CreateContact(int rentalId, ContactViewModel model)
        {
            var contact = this.mapper.Map<Contact>(model);

            await this.validator.ValidateAndThrowAsync(contact);

            contact.Id = rentalId;

            var createdContact = await this.mediator.Send(new CreateContactCommand { Contact = contact });

            return createdContact;
        }

        private async Task<Contact> UpdateContact(int rentalId, ContactViewModel model)
        {
            var contact = this.mapper.Map<Contact>(model);

            await this.validator.ValidateAndThrowAsync(contact);

            contact.Id = rentalId;

            var updatedContact = await this.mediator.Send(new UpdateContactCommand { Contact = contact });

            return updatedContact;
        }
    }
}
