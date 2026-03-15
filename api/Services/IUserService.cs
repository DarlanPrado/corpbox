using api.Models;

namespace api.Services;

public interface IUserService
{
    Task<User?> GetActiveByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailIncludingDeletedAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetBySessionTokenAsync(string sessionToken, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(string name, string email, string passwordHash, CancellationToken cancellationToken = default);

    Task SetSessionAsync(User user, string sessionTokenHash, DateTime sessionExpiresAt, CancellationToken cancellationToken = default);

    Task ClearSessionAsync(User user, CancellationToken cancellationToken = default);
}
