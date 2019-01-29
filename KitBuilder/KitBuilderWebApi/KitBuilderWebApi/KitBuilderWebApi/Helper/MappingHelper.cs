using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using System.Collections.Generic;

namespace KitBuilderWebApi.Helper
{
    public static class MappingHelper
    {

        public static void InitializeMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LinkGroup, LinkGroupDto>()
                    .ForMember(dest => dest.LinkGroupItemDto, conf => conf.MapFrom(src => src.LinkGroupItem));

                cfg.CreateMap<LinkGroupDto, LinkGroup>();
                cfg.CreateMap<LinkGroupItem, LinkGroupItemDto>();
                cfg.CreateMap<LinkGroupItemDto, LinkGroupItem>();
                cfg.CreateMap<Items, ItemsDto>();
                cfg.CreateMap<ItemsDto, Items>();
                cfg.CreateMap<InstructionList, InstructionListDto>();
                cfg.CreateMap<InstructionListAddDto, InstructionList>();
                cfg.CreateMap<InstructionListUpdateDto, InstructionList>();
                cfg.CreateMap<InstructionListDto, InstructionList>();
                cfg.CreateMap<InstructionListMemberDto, InstructionListMember>();
                cfg.CreateMap<InstructionListMember, InstructionListMemberDto>();
                cfg.CreateMap<KitLocale, KitLocaleDto>();
				cfg.CreateMap<KitLinkGroupItemLocale, KitLinkGroupItemLocaleDto>();
                cfg.CreateMap<KitLinkGroupItemDto, KitLinkGroupItem>();
                cfg.CreateMap<KitLinkGroupItem, KitLinkGroupItemDto>();
                cfg.CreateMap<KitLinkGroupLocale, KitLinkGroupLocaleDto>()
						.ForMember(dest => dest.KitLinkGroupItemLocales,
								opts => opts.MapFrom(src => src.KitLinkGroupItemLocale));
                cfg.CreateMap<KitLinkGroup, KitLinkGroupDto>();
                cfg.CreateMap<KitLinkGroupDto, KitLinkGroup>();
                cfg.CreateMap<KitInstructionListDto, KitInstructionList>();
                cfg.CreateMap<KitDto, Kit>();
            });
        }

        public static void CleanupMapper()
        {
            AutoMapper.Mapper.Reset();
        }
    }
}