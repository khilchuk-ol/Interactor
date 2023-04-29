using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SplitDivider.Infrastructure.Persistence.Configurations;

public class UserSplitConfiguration : IEntityTypeConfiguration<UserSplit>
{
    public void Configure(EntityTypeBuilder<UserSplit> builder)
    {
        builder.HasKey(us => new { us.UserId, us.SplitId });
        
        builder.Property(us => us.Group)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Split>()
            .WithMany()
            .HasForeignKey(r => r.SplitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}