using System.Collections.Generic;
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
        }

        [HttpGet]
        public ContactViewModel Get([FromRoute] int rentalId)
        {
            var contact = GetContactInfo(rentalId);

            if (contact == null)
            {   
                throw new NotFoundException();
            }

            return contact;
        }

        [HttpPut]
        public void Put([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            _state[rentalId] = model;
        private ContactViewModel GetContactInfo(int rentalId)
        {
            _state.TryGetValue(rentalId, out var value);
            return value;
        }
        }
    }
}
