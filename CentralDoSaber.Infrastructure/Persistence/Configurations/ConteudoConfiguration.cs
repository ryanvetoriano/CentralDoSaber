using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class ConteudoConfiguration : IEntityTypeConfiguration<Conteudo>
{
    public void Configure(EntityTypeBuilder<Conteudo> builder)
    {
        builder.ToTable("TB_CONTEUDOS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.Tipo)
            .HasConversion<int>();

        builder.Property(c => c.DataLancamento)
            .IsRequired();

        builder.Property(c => c.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.HasOne(c => c.Autor)
            .WithMany(a => a.Conteudos)
            .HasForeignKey(c => c.AutorId);

        builder.HasOne(c => c.Editora)
            .WithMany(e => e.Conteudos)
            .HasForeignKey(c => c.EditoraId);

        builder.HasMany(c => c.Avaliacoes)
            .WithOne(a => a.Conteudo)
            .HasForeignKey(a => a.ConteudoId);

        builder.HasMany(c => c.Comentarios)
            .WithOne(c => c.Conteudo)
            .HasForeignKey(c => c.ConteudoId);
    }
}