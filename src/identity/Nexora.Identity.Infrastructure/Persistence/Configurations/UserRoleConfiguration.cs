using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration
    : IEntityTypeConfiguration<UserRole>
{
    public void Configure(
        EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles", "identity");

        builder.HasKey(userRole => new
        {
            userRole.UserId,
            userRole.RoleId
        });

        builder.Property(userRole => userRole.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(userRole => userRole.RoleId)
            .HasColumnName("role_id")
            .IsRequired();

        builder.Property(userRole => userRole.AssignedAt)
            .HasColumnName("assigned_at")
            .IsRequired();

        builder.Property(userRole => userRole.AssignedBy)
            .HasColumnName("assigned_by");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(userRole => userRole.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(userRole => userRole.RoleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
