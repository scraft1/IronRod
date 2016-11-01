using System.Linq;
using AutoMapper;
using IronRod.Models;

public static class AutoMapperConfig
{
    public static void CreateMaps() 
    {
        Mapper.Initialize(
            config => {
                config.CreateMap<Passage, PassageListViewModel>()
                    .ForMember(dest => dest.Topics,opts => opts.MapFrom(src => src.PassageTopics.Count));

                config.CreateMap<Passage, PassageDetailViewModel>(); 
                config.CreateMap<PassageVerse, PassageVerseViewModel>(); 

                // get backup
                config.CreateMap<Passage, PassageBackup>()
                    .ForMember(dest => dest.VerseIds,opts => opts.MapFrom(src => src.Verses.Select(v => v.VerseID)));
                // post backup 
                config.CreateMap<PassageBackup, Passage>()
                    .ForMember(dest => dest.Verses,opts => 
                        opts.MapFrom(src => src.VerseIds.Select(v => new PassageVerse(){VerseID = v}).ToList()));
            });
            // .ReverseMap();
    }
}