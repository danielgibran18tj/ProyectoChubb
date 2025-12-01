using Business.Interfaces;
using Business.Services;
using Data.Connection;
using Data.Interfaces;
using Data.Repositories;
using Serilog;


namespace API
{
    public class Program
    {
  
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("../logs/app-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Iniciando aplicacion Sistema de Seguros");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

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

            // Registrar Repositorios 
            builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
            builder.Services.AddScoped<IAseguradoRepository, AseguradoRepository>();
            builder.Services.AddScoped<IAseguradoSeguroRepository, AseguradoSeguroRepository>();

            // Registrar Servicios
            builder.Services.AddScoped<ISeguroService, SeguroService>();
            builder.Services.AddScoped<IAseguradoService, AseguradoService>();
            builder.Services.AddScoped<IAsignacionService, AsignacionService>();
            builder.Services.AddScoped<ICargaMasivaService, CargaMasivaService>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
