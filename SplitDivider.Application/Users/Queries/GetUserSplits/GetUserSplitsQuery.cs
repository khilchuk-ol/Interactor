using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Users.Queries.GetUserSplits;

public record GetUserSplitsQuery(int Id) : IRequest<IReadOnlyCollection<UserSplitDto>>;

public class GetUserSplitsQueryHandler : IRequestHandler<GetUserSplitsQuery, IReadOnlyCollection<UserSplitDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserSplitsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<UserSplitDto>> Handle(GetUserSplitsQuery request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        return await _context.UserSplits
            .Where(us => us.UserId == request.Id)
            .OrderByDescending(us => us.SplitId)
            .ProjectTo<UserSplitDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(true);
    }
}