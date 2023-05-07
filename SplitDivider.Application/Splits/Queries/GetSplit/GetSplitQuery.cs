using AutoMapper;
using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Splits.Queries.GetSplit;

public record GetSplitQuery(int Id) : IRequest<SplitDto>;

public class GetSplitQueryHandler : IRequestHandler<GetSplitQuery, SplitDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSplitQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SplitDto> Handle(GetSplitQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }

        return _mapper.Map<SplitDto>(entity);
    }
}