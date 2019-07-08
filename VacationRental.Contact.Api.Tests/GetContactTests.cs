using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace VacationRental.Contact.Api.Tests
{
    [Collection("Integration")]
    public class GetContactTests
    {
        private readonly HttpClient _client;
        private readonly Random _rnd;

        public GetContactTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _rnd = fixture.Random;
        }

        [Fact]
        public async Task GivenContactDoesNotExist_WhenGetContact_ThenNotFoundIsReturned()
        {
            var vacationRentalId = _rnd.Next();

            using (var getResponse = await _client.GetAsync($"/api/v1/vacationrental/{vacationRentalId}/contact"))
            {
                Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            }
        }
    }
}
