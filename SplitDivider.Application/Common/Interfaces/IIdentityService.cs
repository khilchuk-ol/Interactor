using SplitDivider.Application.Common.Models;

namespace SplitDivider.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<bool> AddRoleForUser(string userId, string role);
    
    Task<bool> DeleteRoleForUser(string userId, string role);
}