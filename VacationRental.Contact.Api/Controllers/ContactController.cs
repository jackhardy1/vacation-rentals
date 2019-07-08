using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Contact.Api.Models;

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

        [HttpGet]
        public ContactViewModel Get([FromRoute] int rentalId)
        {
            return _state[rentalId];
        }

        [HttpPut]
        public void Put([FromRoute] int rentalId, [FromBody] ContactViewModel model)
        {
            _state[rentalId] = model;
        }
    }
}
