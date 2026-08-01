namespace Nexora.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; }

    public string Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public string PasswordHash { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private User()
    {
        Username = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        PasswordHash = string.Empty;
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

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ChangeEmail(string email)
    {
        Email = email;
    }

    public void ChangePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}