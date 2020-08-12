using AutoMapper;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.AutoMapperConverters;
using Icon.Web.Mvc.Models;
using Irma.Framework;

namespace Icon.Web.Mvc.App_Start
{
    public static class AutoMapperWebConfiguration
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LocaleProfile>();
                cfg.AddProfile<HierarchyClassProfile>();
                cfg.AddProfile<RegionalItemProfile>();
                cfg.AddProfile<FeatureFlagProfile>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }

    public class ItemProfile : Profile
    {

    }

    public class FeatureFlagProfile : Profile
    {
        public FeatureFlagProfile()
        {
            this.CreateMap<FeatureFlagModel, FeatureFlagViewModel>();
            this.CreateMap<FeatureFlagViewModel, FeatureFlagModel>();
        }
    }

    public class HierarchyClassProfile : Profile
    {
        public HierarchyClassProfile()
        {
            this.CreateMap<AddHierarchyClassManager, AddHierarchyClassCommand>();
            this.CreateMap<ManufacturerManager, AddManufacturerCommand>();
            this.CreateMap<ManufacturerManager, UpdateManufacturerCommand>();
            this.CreateMap<ManufacturerManager, UpdateManufacturerHierarchyClassTraitsCommand>();
            this.CreateMap<BrandManager, BrandCommand>();
            this.CreateMap<BrandManager, UpdateBrandHierarchyClassTraitsCommand>();
            this.CreateMap<UpdateHierarchyClassManager, UpdateHierarchyClassCommand>()
                .ForMember(d => d.ClassNameChanged, o => o.Ignore());
            this.CreateMap<UpdateHierarchyClassManager, UpdateHierarchyClassTraitCommand>()
                .ForMember(d => d.PosDeptNumber, o => o.Ignore())
                .ForMember(d => d.TeamNumber, o => o.Ignore())
                .ForMember(d => d.TeamName, o => o.Ignore())
                .ForMember(d => d.NonAlignedSubteam, o => o.Ignore())
                .ForMember(d => d.SubteamChanged, o => o.Ignore())
                .ForMember(d => d.NonMerchandiseTraitChanged, o => o.Ignore())
                .ForMember(d => d.ProhibitDiscountChanged, o => o.Ignore());
            this.CreateMap<AddHierarchyClassCommand, MessageHierarchyData>()
                .ForMember(d => d.HierarchyClass, o => o.MapFrom(s => s.NewHierarchyClass))
                .ForMember(d => d.ClassNameChange, o => o.Ignore())
                .ForMember(d => d.DeleteMessage, o => o.Ignore());
            this.CreateMap<UpdateHierarchyClassCommand, MessageHierarchyData>()
                .ForMember(d => d.HierarchyClass, o => o.MapFrom(s => s.UpdatedHierarchyClass))
                .ForMember(d => d.ClassNameChange, o => o.MapFrom(s => s.ClassNameChanged))
                .ForMember(d => d.DeleteMessage, o => o.Ignore());
            this.CreateMap<AddHierarchyClassCommand, AddHierarchyClassMessageCommand>()
                .ForMember(d => d.HierarchyClass, o => o.MapFrom(s => s.NewHierarchyClass))
                .ForMember(d => d.ClassNameChange, o => o.Ignore())
                .ForMember(d => d.DeleteMessage, o => o.Ignore());
            this.CreateMap<DeleteHierarchyClassManager, DeleteHierarchyClassCommand>();
            this.CreateMap<UpdateSubTeamManager, UpdateSubTeamCommand>()
                .ForMember(d => d.UpdatedHierarchyClass, o => o.Ignore())
                .ForMember(d => d.PeopleSoftChanged, o => o.Ignore());
            this.CreateMap<AddCertificationAgencyManager, AddCertificationAgencyCommand>();
            this.CreateMap<UpdateHierarchyClassCommand, AddHierarchyClassMessageCommand>()
                .ForMember(d => d.HierarchyClass, o => o.MapFrom(s => s.UpdatedHierarchyClass))
                .ForMember(d => d.ClassNameChange, o => o.MapFrom(s => s.ClassNameChanged))
                .ForMember(d => d.DeleteMessage, o => o.Ignore())
                .ForMember(d => d.NationalClassCode, o => o.Ignore());
            this.CreateMap<DeleteNationalHierarchyManager, DeleteHierarchyClassCommand>();
        }
    }

    public class LocaleProfile : Profile
    {
        public LocaleProfile()
        {
            this.CreateMap<UpdateLocaleManager, UpdateLocaleCommand>();
            this.CreateMap<AddLocaleManager, AddLocaleCommand>();
            this.CreateMap<Locale, MessageQueueLocale>()
                .ConvertUsing<LocaleToMessageQueueLocaleConverter>();
            this.CreateMap<AddLocaleManager, AddAddressCommand>()
                .ForMember(d => d.AddressId, o => o.Ignore());
            this.CreateMap<UpdateVenueManager, UpdateVenueCommand>();
            this.CreateMap<AddVenueManager, AddVenueCommand>();
        }
    }

    public class RegionalItemProfile : Profile
    {
        public RegionalItemProfile()
        {
            this.CreateMap<IconItemChangeQueue, FailedRegionalEventViewModel>()
                .ConvertUsing<IconItemChangeQueueToFailedRegionalItemUpdateViewModelConverter>();
        }
    }
}