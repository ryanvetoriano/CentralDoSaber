using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class AutorConfiguration : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("TB_AUTORES");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(a => a.Biografia)
            .IsRequired();

        builder.HasMany(a => a.Conteudos)
            .WithOne(c => c.Autor)
            .HasForeignKey(c => c.AutorId);
    }
}