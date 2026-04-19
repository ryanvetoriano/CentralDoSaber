using Microsoft.EntityFrameworkCore;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence;

public class CentralDoSaberContext : DbContext
{
    public CentralDoSaberContext(DbContextOptions<CentralDoSaberContext> options) 
        : base(options)
    {
    }

    public DbSet<Autor> Autores { get; set; }

    public DbSet<Conteudo> Conteudos { get; set; }

    public DbSet<Genero> Generos { get; set; }

    public DbSet<Editora> Editoras { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserConfiguration> UserConfigurations { get; set; }

    public DbSet<Avaliacao> Avaliacoes { get; set; }

    public DbSet<Comentario> Comentarios { get; set; }

    public DbSet<ConteudoGenero> ConteudoGeneros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CentralDoSaberContext).Assembly);
    }
}