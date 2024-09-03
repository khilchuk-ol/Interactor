using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SplitDivider.Infrastructure.Persistence.Configurations;

public class RelationConfiguration : IEntityTypeConfiguration<Relation>
{
    public void Configure(EntityTypeBuilder<Relation> builder)
    {
        builder.HasKey(t => new { t.Id, t.UserId });
        
        builder.Property(r => r.Dt)
            .HasColumnType("timestamp")
            .IsRequired();
        
        builder.Property(r => r.Interaction)
            .IsRequired();
        
        builder.Property(r => r.UserId)
            .IsRequired();
        
        builder.Property(r => r.ContactId)
            .IsRequired();
        
        // builder.HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(r => r.UserId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(r => r.ContactId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}