using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.ToTable("TB_COMENTARIOS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Texto)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Conteudo)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(c => c.ConteudoId);
    }
}