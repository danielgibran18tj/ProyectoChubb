using API;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.Single(
                    d => d.ServiceType == typeof(IAseguradoRepository));

                services.Remove(descriptor);

                // Registrar versión FAKE para repositorio de asegurados
                services.AddScoped<IAseguradoRepository, FakeAseguradoRepository>();
            });
        }
    }
}
