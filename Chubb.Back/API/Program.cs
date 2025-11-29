using Business.Interfaces;
using Business.Services;
using Data.Connection;
using Data.Interfaces;
using Data.Repositories;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Registrar SqlConnectionFactory
            builder.Services.AddSingleton<SqlConnectionFactory>();

            // Registrar Repositorios (Data Layer)
            builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
            builder.Services.AddScoped<IAseguradoRepository, AseguradoRepository>();
            builder.Services.AddScoped<IAseguradoSeguroRepository, AseguradoSeguroRepository>();

            // Registrar Servicios (Business Layer)
            builder.Services.AddScoped<ISeguroService, SeguroService>();
            builder.Services.AddScoped<IAseguradoService, AseguradoService>();
            builder.Services.AddScoped<IAsignacionService, AsignacionService>();
            builder.Services.AddScoped<ICargaMasivaService, CargaMasivaService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
