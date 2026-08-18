using Microsoft.EntityFrameworkCore;
using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Infrastructure.Persistence;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<UserVerification> UserVerifications
    => Set<UserVerification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}