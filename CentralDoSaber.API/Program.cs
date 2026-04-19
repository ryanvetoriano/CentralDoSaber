using Microsoft.EntityFrameworkCore;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Application.Services;
using CentralDoSaber.Infrastructure.Persistence;

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

        builder.Services.AddControllers();

        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddDbContext<CentralDoSaberContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Oracle");

            options.UseOracle(connectionString);
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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