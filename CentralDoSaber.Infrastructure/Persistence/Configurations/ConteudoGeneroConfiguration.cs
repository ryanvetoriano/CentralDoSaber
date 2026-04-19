using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class ConteudoGeneroConfiguration : IEntityTypeConfiguration<ConteudoGenero>
{
    public void Configure(EntityTypeBuilder<ConteudoGenero> builder)
    {
        builder.ToTable("TB_CONTEUDO_GENEROS");

        builder.HasKey(cg => new { cg.ConteudoId, cg.GeneroId });

        builder.HasOne(cg => cg.Conteudo)
            .WithMany(c => c.ConteudoGeneros)
            .HasForeignKey(cg => cg.ConteudoId);

        builder.HasOne(cg => cg.Genero)
            .WithMany(g => g.ConteudoGeneros)
            .HasForeignKey(cg => cg.GeneroId);
    }
}