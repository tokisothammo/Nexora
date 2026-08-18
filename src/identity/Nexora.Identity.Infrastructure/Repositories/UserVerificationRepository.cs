using Microsoft.EntityFrameworkCore;
using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;
using Nexora.Identity.Infrastructure.Persistence;

namespace Nexora.Identity.Infrastructure.Repositories;

public sealed class UserVerificationRepository
    : IUserVerificationRepository
{
    private readonly IdentityDbContext _dbContext;

    public UserVerificationRepository(
        IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(UserVerification verification)
    {
        await _dbContext.UserVerifications.AddAsync(verification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserVerification?> GetPendingAsync(
        Guid userId,
        string verificationType)
    {
        return await _dbContext.UserVerifications
            .Where(x =>
                x.UserId == userId &&
                x.VerificationType == verificationType &&
                x.Status == "PENDING")
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}