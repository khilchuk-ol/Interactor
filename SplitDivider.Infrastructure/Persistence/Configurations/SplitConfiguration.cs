using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Shared.Values.ValueObjects;

namespace SplitDivider.Infrastructure.Persistence.Configurations;

public class SplitConfiguration : IEntityTypeConfiguration<Split>
{
    public void Configure(EntityTypeBuilder<Split> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(s => s.State)
            .IsRequired();
        
        builder.Property(s => s.LastModified)
            .HasColumnType("timestamp");
        
        builder.Property(s => s.Created)
            .HasColumnType("timestamp")
            .IsRequired();
        
        builder.Property(s => s.MinRegistrationDt)
            .HasColumnType("timestamp");

        builder.Property(s => s.CountryIds)
            .HasConversion(new ValueConverter<List<int>?, string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<int>>(v) ?? new()))
            .HasColumnType("json")
            .IsRequired();
        
        builder.Property(s => s.ActionsWeights)
            .HasConversion(new ValueConverter<Dictionary<InteractionType, int>, string>(
                v => JsonConvert.SerializeObject(
                    v.ToDictionary(p => p.Key.Name, p => p.Value)),
                v => (JsonConvert.DeserializeObject<Dictionary<string, int>>(v) ?? new())
                    .ToDictionary(p => InteractionType.From(p.Key), p => p.Value)))
            .HasColumnType("json")
            .IsRequired();
    }
}