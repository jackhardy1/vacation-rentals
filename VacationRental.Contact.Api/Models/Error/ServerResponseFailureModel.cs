using System;
using System.Collections.Generic;

namespace VacationRental.Contact.Api.Models.Error
{
    public class ServerResponseFailureModel
    {
        public List<ServerErrorModel> errors { get; set; } = new List<ServerErrorModel>();
    }
}
