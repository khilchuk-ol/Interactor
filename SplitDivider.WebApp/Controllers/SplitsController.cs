using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitDivider.Application.Common.Models;
using SplitDivider.Application.Splits.Commands.CreateSplit;
using SplitDivider.Application.Splits.Commands.DeleteSplit;
using SplitDivider.Application.Splits.Commands.LifecycleCommands.ActivateSplit;
using SplitDivider.Application.Splits.Commands.LifecycleCommands.CloseSplit;
using SplitDivider.Application.Splits.Commands.LifecycleCommands.SuspendSplit;
using SplitDivider.Application.Splits.Commands.UpdateSplit;
using SplitDivider.Application.Splits.Queries.GetSplit;
using SplitDivider.Application.Splits.Queries.GetSplitsWithPagination;
using SplitDivider.Application.Users.Queries.GetSplitUsers;

namespace SplitDivider.WebApp.Controllers;

[Authorize(Roles = "Experimenter")]
public class SplitsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SplitBriefDto>>> GetSplitsWithPagination([FromQuery] GetSplitsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<SplitDto>> Get(int id)
    {
        return await Mediator.Send(new GetSplitQuery(id));
    }
    
    [HttpGet("{id}/users")]
    public async Task<IReadOnlyCollection<SplitUserDto>> GetSplitUsers(int id)
    {
        return await Mediator.Send(new GetSplitUsersQuery(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateSplitCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteSplitCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdateSplitCommand command)
    {
        command.Id = id;

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpPatch("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Activate(int id)
    {
        await Mediator.Send(new ActivateSplitCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id}/suspend")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Suspend(int id)
    {
        await Mediator.Send(new SuspendSplitCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Close(int id)
    {
        await Mediator.Send(new CloseSplitCommand(id));

        return NoContent();
    }
}