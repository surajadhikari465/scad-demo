using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using RegionalEventController.Common;
using RegionalEventController.Controller.Email;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Queries;
using RegionalEventController.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RegionalEventController.Controller.Processors
{
    public class IconReferenceProcessor : INewItemProcessor
    {
        private ILogger<IconReferenceProcessor> logger;
        private IEmailClient emailClient;
        private IconContext iconContext;
        private IQueryHandler<GetTaxCodeToTaxIdMappingQuery, Dictionary<string, int>> getTaxCodeToTaxIdMappingQueryHandler;
        private IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyQueryHandler;
        private IQueryHandler<GetNationalClassCodeToClassIdMappingQuery, Dictionary<int, int>> getNationalClassCodeToIDMappingQueryHandler;
        private IQueryHandler<GetBrandAbbreviationQueryQuery, Dictionary<string, string>> getBrandAbbreviationQueryQueryHandler;
        private IQueryHandler<GetDefaultCertificationAgenciesQuery, Dictionary<string, int>> getDefaultCertificationAgenciesQueryHandler;
        private const string SendItemSubTeamUpdatesToIRMASettingsKey = "SendItemSubTeamupdatesToIRMA";

        public IconReferenceProcessor(ILogger<IconReferenceProcessor> logger,
            IEmailClient emailClient,
            IconContext iconContext,
            IQueryHandler<GetTaxCodeToTaxIdMappingQuery, Dictionary<string, int>> getTaxCodeToTaxIdMappingQueryHandler,
            IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyQueryHandler,
            IQueryHandler<GetNationalClassCodeToClassIdMappingQuery, Dictionary<int, int>> getNationalClassCodeToIDMappingQueryHandler,
            IQueryHandler<GetBrandAbbreviationQueryQuery, Dictionary<string, string>> getBrandAbbreviationQueryQueryHandler,
            IQueryHandler<GetDefaultCertificationAgenciesQuery, Dictionary<string, int>> getDefaultCertificationAgenciesQueryHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.iconContext = iconContext;
            this.getTaxCodeToTaxIdMappingQueryHandler = getTaxCodeToTaxIdMappingQueryHandler;
            this.getRegionalSettingsBySettingsKeyQueryHandler = getRegionalSettingsBySettingsKeyQueryHandler;
            this.getNationalClassCodeToIDMappingQueryHandler = getNationalClassCodeToIDMappingQueryHandler;
            this.getBrandAbbreviationQueryQueryHandler = getBrandAbbreviationQueryQueryHandler;
            this.getDefaultCertificationAgenciesQueryHandler = getDefaultCertificationAgenciesQueryHandler;
        }
        public void Run()
        {
            try
            {
                PopulateTaxCodeToTaxIdMapping();
                PopulateItemSubTeamEventsEnabledRegions();
                PopulateNationalClassToIdMappings();
                PopulateBrandAbbreviationMappings();
                PopulateDefaultCertificationAgencies();
            }
            catch (Exception ex)
            {
                ExceptionHandler<IconReferenceProcessor>.logger = this.logger;
                ExceptionHandler<IconReferenceProcessor>.HandleException("An unhandled exception occurred in the IconReferenceProcessor.", ex, this.GetType(), MethodBase.GetCurrentMethod());

                emailClient.Send(String.Format(Resource.BuildingIconReferenceUnhandledExceptionMessage, ex), Resource.BuildingIconReferenceUnhandledExceptionEmailSubject);
                throw;
            }
        }

        private void PopulateTaxCodeToTaxIdMapping()
        {
            if (Cache.taxCodeToTaxId.Any())
            {
                return;
            }
            else
            {
                Cache.taxCodeToTaxId = getTaxCodeToTaxIdMappingQueryHandler.Execute(new GetTaxCodeToTaxIdMappingQuery());
            }
        }

        private void PopulateItemSubTeamEventsEnabledRegions()
        {
            if (Cache.itemSbTeamEventEnabledRegions != null)
            {
                return;
            }
            else
            {
                List<RegionalSettingsModel> regionalSettingsList = getRegionalSettingsBySettingsKeyQueryHandler.Execute(new GetRegionalSettingsBySettingsKeyNameParameters { SettingsKeyName = SendItemSubTeamUpdatesToIRMASettingsKey });
                                
                if (regionalSettingsList != null)
                {
                    Cache.itemSbTeamEventEnabledRegions = regionalSettingsList.Where(rs => rs.Value == true).Select(a => a.RegionCode).ToList();
                }
                else
                {
                    Cache.itemSbTeamEventEnabledRegions = new List<string>();
                } 
            }
        }

        private void PopulateNationalClassToIdMappings()
        {
            if(Cache.nationalClassCodeToNationalClassId.Any())
            {
                return;
            }
            else
            {
                Cache.nationalClassCodeToNationalClassId = getNationalClassCodeToIDMappingQueryHandler.Execute(new GetNationalClassCodeToClassIdMappingQuery());
            }
        }

        private void PopulateBrandAbbreviationMappings()
        {
            if (Cache.brandNameToBrandAbbreviationMap.Any())
            {
                return;
            }
            else
            {
                Cache.brandNameToBrandAbbreviationMap = getBrandAbbreviationQueryQueryHandler.Execute(new GetBrandAbbreviationQueryQuery());
            }
        }

        private void PopulateDefaultCertificationAgencies()
        {
            if (Cache.defaultCertificationAgencies.Any())
            {
                return;
            }
            else
            {
                Cache.defaultCertificationAgencies = getDefaultCertificationAgenciesQueryHandler.Execute(new GetDefaultCertificationAgenciesQuery()); ;
            }
        }
    }
}
