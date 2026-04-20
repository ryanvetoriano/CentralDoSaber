using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.ToTable("TB_AVALIACOES");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nota)
            .IsRequired();

        builder.Property(a => a.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany(u => u.Avaliacoes)
            .HasForeignKey(a => a.UserId);

        builder.HasOne(a => a.Conteudo)
            .WithMany(c => c.Avaliacoes)
            .HasForeignKey(a => a.ConteudoId);

        builder.HasIndex(a => new { a.UserId, a.ConteudoId })
            .IsUnique();
    }
}