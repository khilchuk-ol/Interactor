using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitDivider.Application.Users.Queries.GetUserSplits;

namespace SplitDivider.WebApp.Controllers;

//[Authorize]
public class UsersController : ApiControllerBase
{
    [HttpGet("{id}/splits")]
    public async Task<IReadOnlyCollection<UserSplitDto>> GetUserSplits(int id)
    {
        return await Mediator.Send(new GetUserSplitsQuery(id));
    }
}