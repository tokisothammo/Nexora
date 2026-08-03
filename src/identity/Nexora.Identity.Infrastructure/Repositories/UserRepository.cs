using Nexora.Identity.Domain.Entities;
using Nexora.Identity.Domain.Repositories;

namespace Nexora.Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task AddAsync(User user)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(new List<User>());
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return Task.FromResult<User?>(null);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<User?>(null);
    }

    public Task UpdateAsync(User user)
    {
        return Task.CompletedTask;
    }
}