namespace Nexora.Identity.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Role()
    {
    }

    public Role(
        string code,
        string name,
        string? description = null)
    {
        Id = Guid.NewGuid();
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = description?.Trim();
        CreatedAt = DateTime.UtcNow;
    }
}