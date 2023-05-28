using AutoMapper;

namespace SplitDivider.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        profile.CreateMap(typeof(T), GetType());
    }
}
