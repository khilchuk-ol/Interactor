using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SplitDivider.Infrastructure.Persistence.Configurations;

public class RelationConfiguration : IEntityTypeConfiguration<Relation>
{
    public void Configure(EntityTypeBuilder<Relation> builder)
    {
        builder.Property(r => r.Dt)
            .HasColumnType("timestamp")
            .IsRequired();
        
        builder.Property(r => r.Interaction)
            .IsRequired();
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.ContactId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}