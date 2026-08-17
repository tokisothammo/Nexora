using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Enums;

namespace Nexora.Identity.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "identity");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id");

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(255);

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(user => user.Status)
            .HasColumnName("status")
            .HasMaxLength(30)
            .HasConversion(
                status => status.ToString().ToUpperInvariant(),
                value => Enum.Parse<UserStatus>(value, true))
            .IsRequired();

        builder.Property(user => user.IsVerified)
            .HasColumnName("is_verified")
            .IsRequired();

        builder.Property(user => user.ProfilePhoto)
            .HasColumnName("profile_photo");

        builder.Property(user => user.LastLogin)
            .HasColumnName("last_login");

        builder.Property(user => user.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(user => user.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(user => user.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(user => user.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(user => user.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(user => user.PhoneNumber)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}