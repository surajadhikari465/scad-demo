using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DatabaseModels;

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
                cfg.CreateMap<InstructionListDto, InstructionList>();
                cfg.CreateMap<InstructionListMemberDto, InstructionListMember>();
                cfg.CreateMap<InstructionListMember, InstructionListMemberDto>();
            });
        }
    }
}
