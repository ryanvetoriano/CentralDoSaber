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

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId);

        builder.HasOne(c => c.Conteudo)
            .WithMany(c => c.Comentarios)
            .HasForeignKey(c => c.ConteudoId);
    }
}