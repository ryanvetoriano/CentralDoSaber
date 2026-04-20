using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class EditoraConfiguration : IEntityTypeConfiguration<Editora>
{
    public void Configure(EntityTypeBuilder<Editora> builder)
    {
        builder.ToTable("TB_EDITORAS");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Pais)
            .IsRequired();

        builder.Property(e => e.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.HasMany(e => e.Conteudos)
            .WithOne(c => c.Editora)
            .HasForeignKey(c => c.EditoraId);
    }
}