using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SplitDivider.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        
        builder.Metadata.SetTableName("Users");
        
        builder.Property(u => u.Id)
            .ValueGeneratedNever();
        
        builder.Property(u => u.RegistrationDt)
            .HasColumnType("timestamp")
            .IsRequired();
        
        builder.Property(u => u.Gender)
            .IsRequired();
        
        builder.Property(u => u.CountryId)
            .IsRequired();
        
        builder.Property(u => u.State)
            .IsRequired();
    }
}