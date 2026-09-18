using Microsoft.EntityFrameworkCore;
using CentralDoSaber.API.Extensions;
using CentralDoSaber.API.Handlers;
using CentralDoSaber.API.HealthChecks;
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

        // Logs no console em uma linha, com timestamp e escopos (TraceId)
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.IncludeScopes = true;
            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
        });

        // Controllers
        builder.Services.AddControllers();

        // Tratamento global de exceções (ProblemDetails + log com traceId)
        builder.Services.AddProblemDetails(options =>
        {
            // O padrão usa o Activity.Id (W3C); usamos o mesmo TraceIdentifier
            // gravado nos logs para que resposta e log se correlacionem.
            options.CustomizeProblemDetails = context =>
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        });
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        // Services (Application)
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IGeneroService, GeneroService>();
        builder.Services.AddScoped<IAutorService, AutorService>();

        // Repositories (Infrastructure)
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

        // Health checks (self + banco Oracle + site FIAP)
        builder.Services.AddCentralDoSaberHealthChecks(builder.Configuration);

        // Swagger (metadados em appsettings "Swagger" + XML comments)
        builder.Services.AddCentralDoSaberSwagger(builder.Configuration);

        var app = builder.Build();

        // Pipeline HTTP
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseCentralDoSaberSwagger(builder.Configuration);
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // GET /health — relatório completo de todos os checks (fora do Swagger)
        app.MapCentralDoSaberHealthChecks();

        app.Run();
    }
}
