using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Contact.Api.Controllers;
using VacationRental.Contact.Api.Models;
using Xunit;

namespace VacationRental.Contact.Api.Tests
{
    [Collection("Integration")]
    public class UpdateContactTests
    {
        private readonly HttpClient _client;
        private readonly Random _rnd;

        public UpdateContactTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _rnd = fixture.Random;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdateContact_ThenAGetReturnsTheUpdatedContact()
        {
            var vacationRentalId = _rnd.Next();
            var request = new
            {
                Forename = "Flash",
                Surname = "Gordan",
                Phone = "1800-SPEED-ING",
                NativeLanguage = "English",
                OtherSpokenLanguages = new[] {"Spanish"},
                AboutMe = "Fastest man alive"
            };

            using (var updateResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/{vacationRentalId}/contact", request))
            {
                Assert.True(updateResponse.IsSuccessStatusCode);
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/vacationrental/{vacationRentalId}/contact"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var returnedData = await getResponse.Content.ReadAsAsync<ContactViewModel>();
                Assert.Equal(request.Forename, returnedData.Forename);
                Assert.Equal(request.Surname, returnedData.Surname);
                Assert.Equal(request.Phone, returnedData.Phone);
                Assert.Equal(request.NativeLanguage, returnedData.NativeLanguage);
                Assert.Equal(request.OtherSpokenLanguages.Single(), returnedData.OtherSpokenLanguages.Single());
                Assert.Equal(request.AboutMe, returnedData.AboutMe);
            }
        }

        [Fact]
        public async Task GivenAContactWithoutPhoneNumber_WhenUpdateContact_ThenBadRequestIsReturned()
        {
            var vacationRentalId = _rnd.Next();

			var createRequest = new
			{
				Forename = "Flash",
				Surname = "Gordan",
				Phone = "1800-SPEED-ING",
				NativeLanguage = "English",
				OtherSpokenLanguages = new[] { "Spanish" },
				AboutMe = "Fastest man alive"
			};

			using (var createResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/{vacationRentalId}/contact", createRequest))
			{
				Assert.True(createResponse.IsSuccessStatusCode);
			}

			var updateRequest = new
            {
                Forename = "Flash",
                Surname = "Gordan",
                NativeLanguage = "English",
                OtherSpokenLanguages = new[] { "Spanish" },
                AboutMe = "Fastest man alive"
            };

            using (var updateResponse = await _client.PutAsJsonAsync($"/api/v1/vacationrental/{vacationRentalId}/contact", updateRequest))
            {
                Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
            }
        }
    }
}
