using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Users.Queries.GetSplitUsers;

public record GetSplitUsersQuery(int Id) : IRequest<IReadOnlyCollection<SplitUserDto>>;

public class GetSplitUsersQueryHandler : IRequestHandler<GetSplitUsersQuery, IReadOnlyCollection<SplitUserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSplitUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<SplitUserDto>> Handle(GetSplitUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserSplits
            .Where(us => us.SplitId == request.Id)
            .OrderByDescending(us => us.SplitId)
            .ProjectTo<SplitUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}