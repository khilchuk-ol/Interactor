using Microsoft.AspNetCore.Identity;
using SplitDivider.Infrastructure.Identity;

namespace SplitDivider.WebApp.Authorization.Models;

public class Response
{
    public ApplicationUser? Result { get; set; }

    public IEnumerable<IdentityError> Errors { get; set; } = new List<IdentityError>();
}