using System.Globalization;
using AutoMapper;
using SplitDivider.Application.Common.Mappings;

namespace SplitDivider.Application.Splits.Queries.GetSplit;

public class SplitDto : IMapFrom<Split>
{
    public int Id { get; init; }

    public string Name { get; set; }
    
    public int State { get; set; }
    
    public string CreatedAt { get; set; }
    
    public IReadOnlyDictionary<string, int> ActionsWeights { get; set; }

    public string? Gender { get; set; }
    
    public IReadOnlyList<int>? CountryIds { get; set; }
    
    public string? MinRegDt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Split, SplitDto>()
            .ForMember(nameof(CreatedAt),
                opts => opts.MapFrom(src => src.Created.ToString(CultureInfo.InvariantCulture)))
            .ForMember(nameof(ActionsWeights), opts => opts.MapFrom(src => src.ActionsWeights
                .ToDictionary(p => p.Key.Name, p => p.Value)
                .AsReadOnly()))
            .ForMember(nameof(Gender), opts => opts.MapFrom(src => src.Gender != null ? src.Gender.ToString() : null))
            .ForMember(nameof(MinRegDt), opts => opts.MapFrom(src =>
                src.MinRegistrationDt.HasValue
                    ? src.MinRegistrationDt.Value.ToString(CultureInfo.InvariantCulture)
                    : null))
            .ForMember(nameof(CountryIds), opts => opts.MapFrom(src => src.CountryIds != null ? src.CountryIds.AsReadOnly() : null));
    }
}