using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Infrastructure.Persistence.Configurations;

public sealed class UserVerificationConfiguration
    : IEntityTypeConfiguration<UserVerification>
{
    public void Configure(EntityTypeBuilder<UserVerification> builder)
    {
        builder.ToTable("user_verifications", "identity");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.VerificationType)
            .HasColumnName("verification_type")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Channel)
            .HasColumnName("channel")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Destination)
            .HasColumnName("destination")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.CodeHash)
            .HasColumnName("code_hash")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.AttemptCount)
            .HasColumnName("attempt_count")
            .IsRequired();

        builder.Property(x => x.MaximumAttempts)
            .HasColumnName("maximum_attempts")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(x => x.VerifiedAt)
            .HasColumnName("verified_at");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}