using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class UserService(AppDbContext dbContext) : IUserService
{
    public Task<User?> GetActiveByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(email);
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
    }

    public Task<User?> GetByEmailIncludingDeletedAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(email);
        return dbContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
    }

    public Task<User?> GetBySessionTokenAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        var tokenHash = HashSessionToken(sessionToken);
        var now = DateTime.UtcNow;

        return dbContext.Users.FirstOrDefaultAsync(
            u => u.SessionTokenHash == tokenHash && u.SessionExpiresAt != null && u.SessionExpiresAt > now,
            cancellationToken);
    }

    public async Task<User> CreateAsync(string name, string email, string passwordHash, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var user = new User
        {
            Name = name.Trim(),
            Email = NormalizeEmail(email),
            PasswordHash = passwordHash,
            Created = now,
            Updated = now,
            Deleted = null
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task SetSessionAsync(User user, string sessionTokenHash, DateTime sessionExpiresAt, CancellationToken cancellationToken = default)
    {
        user.SessionTokenHash = sessionTokenHash;
        user.SessionExpiresAt = sessionExpiresAt;
        user.Updated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearSessionAsync(User user, CancellationToken cancellationToken = default)
    {
        user.SessionTokenHash = null;
        user.SessionExpiresAt = null;
        user.Updated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static string HashSessionToken(string sessionToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sessionToken));
        return Convert.ToHexString(bytes);
    }
}
