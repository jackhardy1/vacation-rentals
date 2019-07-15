using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Xunit;

namespace VacationRental.Contact.Api.Tests
{
    [CollectionDefinition("Integration")]
    public class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }
        public Random Random { get; } = new Random();

        public IntegrationFixture()
        {
            var webhostBuilder = new WebHostBuilder()
            .UseStartup<Startup>()
            .UseSetting("ConnectionStrings:DefaultConnection", "Host=localhost;Port=5432;Username=postgres;Password=password;Database=Contacts;");

            _server = new TestServer(webhostBuilder);

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
