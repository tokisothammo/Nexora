namespace Nexora.Identity.Domain.Entities;

public class UserVerification
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string VerificationType { get; private set; } = string.Empty;

    public string Channel { get; private set; } = string.Empty;

    public string Destination { get; private set; } = string.Empty;

    public string CodeHash { get; private set; } = string.Empty;

    public string Status { get; private set; } = "PENDING";

    public int AttemptCount { get; private set; }

    public int MaximumAttempts { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public DateTime? VerifiedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private UserVerification()
    {
    }

    public UserVerification(
        Guid userId,
        string verificationType,
        string channel,
        string destination,
        string codeHash,
        DateTime expiresAt,
        int maximumAttempts = 5)
    {
        Id = Guid.NewGuid();
        UserId = userId;

        VerificationType = verificationType;
        Channel = channel;
        Destination = destination;
        CodeHash = codeHash;

        Status = "PENDING";
        AttemptCount = 0;
        MaximumAttempts = maximumAttempts;

        ExpiresAt = expiresAt;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

    public bool CanAttempt()
    {
        return Status == "PENDING"
            && !IsExpired()
            && AttemptCount < MaximumAttempts;
    }

    public void RecordFailedAttempt()
    {
        AttemptCount++;
        UpdatedAt = DateTime.UtcNow;

        if (AttemptCount >= MaximumAttempts)
        {
            Status = "FAILED";
        }
    }

    public void MarkVerified()
    {
        Status = "VERIFIED";
        VerifiedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkExpired()
    {
        Status = "EXPIRED";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = "CANCELLED";
        UpdatedAt = DateTime.UtcNow;
    }
}