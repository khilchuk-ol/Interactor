using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Common.Mappings;
using SplitDivider.Application.Common.Models;

namespace SplitDivider.Application.Splits.Queries.GetSplitsWithPagination;

public record GetSplitsWithPaginationQuery : IRequest<PaginatedList<SplitBriefDto>>
{
    public int? State { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

public class GetSplitsWithPaginationQueryHandler : IRequestHandler<GetSplitsWithPaginationQuery, PaginatedList<SplitBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSplitsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SplitBriefDto>> Handle(GetSplitsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var res = _context.Splits.AsQueryable();

        if (request.State.HasValue)
        {
            res = res.Where(s => (int) s.State == request.State.Value);
        }  
        
        return await res.OrderByDescending(s => s.Id)
            .ProjectTo<SplitBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize)
            .ConfigureAwait(true);
    }
}
