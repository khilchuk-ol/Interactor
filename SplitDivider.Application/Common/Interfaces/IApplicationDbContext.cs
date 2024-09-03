using Microsoft.EntityFrameworkCore;

namespace SplitDivider.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> AppUsers { get; }

    DbSet<Relation> Relations { get; }
    
    DbSet<Split> Splits { get; }
    
    DbSet<UserSplit> UserSplits { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}