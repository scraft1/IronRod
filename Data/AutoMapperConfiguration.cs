using System.Linq;
using AutoMapper;
using IronRod.Models;

public static class AutoMapperConfig
{
    public static void CreateMaps() 
    {
        Mapper.Initialize(
            config => {
                config.CreateMap<PassageViewModel, Passage>().ReverseMap();

                config.CreateMap<Passage, PassageDetailViewModel>().ReverseMap();
                config.CreateMap<PassageVerse, PassageVerseViewModel>().ReverseMap();

                config.CreateMap<Passage, PassageBackup>().ForMember(dest => dest.VerseIds,
                                    opts => opts.MapFrom(src => src.Verses.Select(v => v.VerseID)));

                config.CreateMap<PassageBackup, Passage>().ForMember(dest => dest.Verses,
                    opts => opts.MapFrom(src => src.VerseIds.Select(v => new PassageVerse(){VerseID = v}).ToList()));
            });
    }
}