using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration
    : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles", "identity");

        builder.HasKey(role => role.Id);

        builder.Property(role => role.Id)
            .HasColumnName("id");

        builder.Property(role => role.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(role => role.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(role => role.Description)
            .HasColumnName("description");

        builder.Property(role => role.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(role => role.Code)
            .IsUnique();
    }
}