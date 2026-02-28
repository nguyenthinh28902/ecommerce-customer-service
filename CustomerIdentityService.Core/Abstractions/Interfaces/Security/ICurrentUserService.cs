namespace CustomerIdentityService.Core.Abstractions.Interfaces.Security
{
    public interface ICurrentUserService
    {
        int Id { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
        string? EmailOrPhone { get; }
    }
}
