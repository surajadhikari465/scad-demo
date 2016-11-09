using AutoMapper;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.AutoMapperConverters;
using Icon.Web.Mvc.Models;
using Irma.Framework;

namespace Icon.Web.Mvc.App_Start
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config => 
            { 
                config.AddProfile(new LocaleProfile());
                config.AddProfile(new HierarchyClassProfile());
                config.AddProfile(new RegionalItemProfile());
            });
        }
    }

    public class HierarchyClassProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<AddHierarchyClassManager, AddHierarchyClassCommand>();
            Mapper.CreateMap<AddBrandManager, AddBrandCommand>();
            Mapper.CreateMap<UpdateBrandManager, UpdateBrandCommand>();
            Mapper.CreateMap<UpdateBrandManager, UpdateBrandHierarchyClassTraitsCommand>();
            Mapper.CreateMap<UpdateHierarchyClassManager, UpdateHierarchyClassCommand>();
            Mapper.CreateMap<UpdateHierarchyClassManager, UpdateHierarchyClassTraitCommand>();
            Mapper.CreateMap<AddHierarchyClassCommand, MessageHierarchyData>()
                .ForMember(destination => destination.HierarchyClass, option => option.MapFrom(source => source.NewHierarchyClass));
            Mapper.CreateMap<UpdateHierarchyClassCommand, MessageHierarchyData>()
                .ForMember(destination => destination.HierarchyClass, option => option.MapFrom(source => source.UpdatedHierarchyClass))
                .ForMember(destination => destination.ClassNameChange, option => option.MapFrom(source => source.ClassNameChanged));
            Mapper.CreateMap<AddHierarchyClassCommand, AddHierarchyClassMessageCommand>();
            Mapper.CreateMap<DeleteHierarchyClassManager, DeleteHierarchyClassCommand>();
            Mapper.CreateMap<UpdateSubTeamManager, UpdateSubTeamCommand>();
            Mapper.CreateMap<AddCertificationAgencyManager, AddCertificationAgencyCommand>();
            Mapper.CreateMap<UpdateCertificationAgencyManager, UpdateCertificationAgencyCommand>();
            Mapper.CreateMap<AddMerchTaxAssociationManager, AddMerchTaxMappingCommand>();
            Mapper.CreateMap<AddMerchTaxAssociationManager, ApplyMerchTaxAssociationToItemsCommand>();
            Mapper.CreateMap<UpdateMerchTaxAssociationManager, UpdateMerchTaxMappingCommand>();
            Mapper.CreateMap<UpdateMerchTaxAssociationManager, ApplyMerchTaxAssociationToItemsCommand>();
            Mapper.CreateMap<UpdateHierarchyClassCommand, AddHierarchyClassMessageCommand>()
                .ForMember(destination => destination.HierarchyClass, option => option.MapFrom(source => source.UpdatedHierarchyClass))
                .ForMember(destination => destination.ClassNameChange, option => option.MapFrom(source => source.ClassNameChanged));
        }
    }

    public class LocaleProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UpdateLocaleManager, UpdateLocaleCommand>();
            Mapper.CreateMap<AddLocaleManager, AddLocaleCommand>();
            Mapper.CreateMap<Locale, MessageQueueLocale>()
                .ConvertUsing<LocaleToMessageQueueLocaleConverter>();
            Mapper.CreateMap<AddLocaleManager, AddAddressCommand>();
        }
    }

    public class RegionalItemProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IconItemChangeQueue, FailedRegionalEventViewModel>()
                .ConvertUsing<IconItemChangeQueueToFailedRegionalItemUpdateViewModelConverter>();
        }
    }
}