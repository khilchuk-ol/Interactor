using MediatR;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Relations.Commands.CreateRelation;

public record CreateRelationCommand : IRequest
{
    public int UserId { get; init; }
    
    public int ContactId { get; init; }
    
    public string InteractionType { get; init; }
}

public class CreateRelationCommandHanler : IRequestHandler<CreateRelationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRelationCommandHanler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task Handle(CreateRelationCommand request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        var entity = new Relation
        {
            UserId = request.UserId,
            ContactId = request.ContactId,
            Interaction = request.InteractionType,
            Dt = DateTime.Now
        };

        _context.Relations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}