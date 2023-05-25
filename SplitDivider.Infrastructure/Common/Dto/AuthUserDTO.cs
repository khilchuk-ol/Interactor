using SplitDivider.Infrastructure.Identity;

namespace MediatR.Dto;

public class AuthUserDTO
{
    public ApplicationUser User { get; set; }
    public string Token { get; set; }
}