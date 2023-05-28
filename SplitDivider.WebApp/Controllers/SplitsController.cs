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

//[Authorize]
public class SplitsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<SplitBriefDto>>> GetSplitsWithPagination([FromQuery] GetSplitsWithPaginationQuery query)
    {
        return await Mediator.Send(query).ConfigureAwait(true);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<SplitDto>> Get(int id)
    {
        return await Mediator.Send(new GetSplitQuery(id)).ConfigureAwait(true);
    }
    
    [HttpGet("{id}/graph")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetGraph(int id)
    {
        string filename = $"split{id.ToString()}.graphml";

        var stream = System.IO.File.OpenRead(@"Visualization/" + filename);

        if (stream == null)
        {
            return NotFound();
        }

        return File(stream, "application/octet-stream", filename);
    }
    
    [HttpGet("{id}/users")]
    public async Task<IReadOnlyCollection<SplitUserDto>> GetSplitUsers(int id)
    {
        return await Mediator.Send(new GetSplitUsersQuery(id)).ConfigureAwait(true);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateSplitCommand command)
    {
        return await Mediator.Send(command).ConfigureAwait(true);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteSplitCommand(id)).ConfigureAwait(true);

        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdateSplitCommand command)
    {
        command.Id = id;

        await Mediator.Send(command).ConfigureAwait(true);

        return NoContent();
    }
    
    [HttpPatch("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Activate(int id)
    {
        await Mediator.Send(new ActivateSplitCommand(id)).ConfigureAwait(true);

        return NoContent();
    }
    
    [HttpPatch("{id}/suspend")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Suspend(int id)
    {
        await Mediator.Send(new SuspendSplitCommand(id)).ConfigureAwait(true);

        return NoContent();
    }
    
    [HttpPatch("{id}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Close(int id)
    {
        await Mediator.Send(new CloseSplitCommand(id)).ConfigureAwait(true);

        return NoContent();
    }
}