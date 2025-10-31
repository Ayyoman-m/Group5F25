using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Group5F25.API.DTOs;
using Group5F25.API.Models;
using Xunit;

namespace Group5F25.Tests
{
    public class AuthTests : IClassFixture<TestAppFactory>
    {
        private readonly HttpClient _client;

        public AuthTests(TestAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Should_Return_Token()
        {
            var payload = new RegisterRequest
            {
                FirstName = "Rasik",
                LastName = "Pandit",
                Email = "rasik@test.com",
                Password = "Secret123!"
            };

            var resp = await _client.PostAsJsonAsync("/auth/register", payload);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await resp.Content.ReadFromJsonAsync<AuthResponse>();
            body.Should().NotBeNull();
            body!.Success.Should().BeTrue();
            body.Token.Should().NotBeNullOrEmpty();
        }
    }
}
