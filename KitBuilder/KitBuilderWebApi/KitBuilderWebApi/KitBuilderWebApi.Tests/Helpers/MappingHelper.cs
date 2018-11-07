using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;

namespace KitBuilderWebApi.Tests.Helper
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
                cfg.CreateMap<KitLinkGroupItemDto, KitLinkGroupItem>();
                cfg.CreateMap<KitLinkGroupDto, KitLinkGroup>();
                cfg.CreateMap<KitInstructionListDto, KitInstructionList>();
                cfg.CreateMap<KitDto, Kit>();
            });
        }
    }
}
