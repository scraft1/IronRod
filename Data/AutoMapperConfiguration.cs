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
                config.CreateMap<PassageTopic, TopicViewModel>()
                    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Topic.Title)); 

                // config.CreateMap<Topic,ReviewTopicViewModel>()
                //     .ForMember(dest => dest.Count,opts => opts.MapFrom(src => src.PassageTopics
                //     .Where(pt => pt.Passage.DatePassed.AddDays(pt.Passage.Level) <= DateTime.Today).ToList().Count));

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