using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class AseguradoTests : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly HttpClient _client;

        public AseguradoTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task POST_Asegurados_DeberiaRetornar201()
        {
            var dto = new
            {
                Cedula = "0102030405",
                NombreCompleto = "Juan Perez",
                Telefono = "0987654321",
                Edad = 35
            };

            var response = await _client.PostAsJsonAsync("/api/Asegurados", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

    }

}
