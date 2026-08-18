using Nexora.Identity.Domain.Entities;

namespace Nexora.Identity.Domain.Repositories;

public interface IUserVerificationRepository
{
    Task AddAsync(UserVerification verification);

    Task<UserVerification?> GetPendingAsync(
        Guid userId,
        string verificationType);

    Task SaveChangesAsync();
}