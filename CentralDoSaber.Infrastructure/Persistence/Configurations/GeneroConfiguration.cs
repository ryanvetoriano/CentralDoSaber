using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class GeneroConfiguration : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        builder.ToTable("TB_GENEROS");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Descricao)
            .IsRequired();

        builder.Property(g => g.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();
    }
}