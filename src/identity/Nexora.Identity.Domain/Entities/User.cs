using Nexora.Identity.Domain.Enums;

namespace Nexora.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public string PasswordHash { get; private set; } = string.Empty;

    public UserStatus Status { get; private set; }

    public bool IsVerified { get; private set; }

    public string? ProfilePhoto { get; private set; }

    public DateTime? LastLogin { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Guid? CreatedBy { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    private User()
    {
    }

    public User(
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        string passwordHash)
    {
        Id = Guid.NewGuid();

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;

        Status = UserStatus.Pending;
        IsVerified = false;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
        IsVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = UserStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Deactivated;
        UpdatedAt = DateTime.UtcNow;
        DeletedAt = DateTime.UtcNow;
    }
}