namespace Nexora.Identity.Application.Features.Users.GetUsers;

public sealed record UserListItem(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Email,
    string Status,
    bool IsVerified,
    DateTime CreatedAt);