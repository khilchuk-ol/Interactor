using AutoMapper;
using SplitDivider.Application.Common.Mappings;

namespace SplitDivider.Application.Splits.Queries.GetSplitsWithPagination;

public class SplitBriefDto : IMapFrom<Split>
{
    public int Id { get; init; }

    public string Name { get; set; }
    
    public int State { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Split, SplitBriefDto>()
            .ForMember(nameof(Id), opts => opts.MapFrom(src => src.Id))
            .ForMember(nameof(Name), opts => opts.MapFrom(src => src.Name))
            .ForMember(nameof(State), opts => opts.MapFrom(src => src.State));
    }
}