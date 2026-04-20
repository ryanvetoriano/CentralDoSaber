using Microsoft.EntityFrameworkCore;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Application.Services;
using CentralDoSaber.Infrastructure.Persistence;
using CentralDoSaber.Infrastructure.Persistence.Repositories;

namespace CentralDoSaber.API;

/// <summary>
/// Ponto de entrada da aplicação.
/// Configura e executa o pipeline da API ASP.NET Core.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // Services (Application)
        builder.Services.AddScoped<IUserService, UserService>();

        // Repositories (Infrastructure)
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IConteudoRepository, ConteudoRepository>();   
        builder.Services.AddScoped<IAutorRepository, AutorRepository>();           
        builder.Services.AddScoped<IGeneroRepository, GeneroRepository>(); 

        // DbContext (EF Core + Oracle)
        builder.Services.AddDbContext<CentralDoSaberContext>(options =>
        {
            var connectionString =
                builder.Configuration.GetConnectionString("CentralDoSaberContextOracle");

            options.UseOracle(connectionString);
        });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}