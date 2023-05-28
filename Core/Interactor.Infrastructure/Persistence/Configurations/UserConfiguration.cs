using Interactor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interactor.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        
        builder.Property(u => u.RegistrationDt)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(u => u.Gender)
            .IsRequired();
        
        builder.Property(u => u.CountryId)
            .IsRequired();
        
        builder.Property(u => u.State)
            .IsRequired();
    }
}