using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Contact.Api.Models;
using VacationRental.Contact.Api.Models.Error;

namespace VacationRental.Contact.Api.Controllers
{
    [Route("api/v1/vacationrental/{rentalId:int}/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IDictionary<int, ContactViewModel> _state;

        public ContactController(IDictionary<int, ContactViewModel> state)
        {
            _state = state;
        }

        [HttpPost]
        public void Create([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            this.ValidateModel(model);

            this.AddOrUpdateContact(rentalId, model);
        }

        [HttpGet]
        public ContactViewModel Get([FromRoute] int rentalId)
        {
            var contact = GetContactInfo(rentalId);

            return contact;
        }

        [HttpPut]
        public void Put([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            this.GetContactInfo(rentalId);

            this.ValidateModel(model);

            this.AddOrUpdateContact(rentalId, model);
        }

        private ContactViewModel GetContactInfo(int rentalId)
        {
            _state.TryGetValue(rentalId, out var value);

            if (value == null)
            {
                throw new NotFoundException();
            }

            return value;
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

        private void AddOrUpdateContact(int rentalId, ContactViewModel model)
        {
            _state[rentalId] = model;
        }
    }
}
