using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(120)]
    public required string Name { get; set; }

    [MaxLength(255)]
    public required string Email { get; set; }

    [MaxLength(512)]
    public required string PasswordHash { get; set; }

    [MaxLength(128)]
    public string? SessionTokenHash { get; set; }

    public DateTime? SessionExpiresAt { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public DateTime? Deleted { get; set; }
}
