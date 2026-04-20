using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Infrastructure.Persistence.Configurations;

public class UserConfigurationConfiguration : IEntityTypeConfiguration<UserConfiguration>
{
    public void Configure(EntityTypeBuilder<UserConfiguration> builder)
    {
        builder.ToTable("TB_USER_CONFIGURATIONS");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserId)
            .IsRequired();

        builder.HasIndex(u => u.UserId)
            .IsUnique();
        
        builder.Property(u => u.NotificacoesAtivas)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.Property(u => u.Disponivel)
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.HasOne<User>()
            .WithOne(u => u.Configuration)
            .HasForeignKey<UserConfiguration>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}