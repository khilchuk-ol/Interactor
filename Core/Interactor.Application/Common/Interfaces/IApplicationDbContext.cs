using Interactor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interactor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}