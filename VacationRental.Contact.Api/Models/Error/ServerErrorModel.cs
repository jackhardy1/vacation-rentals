using System.Net;

namespace VacationRental.Contact.Api.Models.Error
{
    public class ServerErrorModel
    {
        public string code { get; set; } = "UNKNOWN";

        public int status { get; set; } = (int)HttpStatusCode.BadRequest;
    }
}
