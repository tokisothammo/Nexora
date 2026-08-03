namespace Nexora.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private User()
    {
    }

    public User(
        string username,
        string email,
        string phoneNumber,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
