using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using Icon.Common.Email;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using Irma.Framework;
using Moq;
using RegionalEventController.Common;
using RegionalEventController.Controller.Processors;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Infrastructure;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.Controller.ProcessorModules;


namespace RegionalEventController.Tests.Controller
{
    [TestClass]
    public class NewItemProcessorPerformanceTest
    {
        private const string OrganicCertificationAgency = "Organic";
        private IconContext iconContext = new IconContext();
        private int testNewItemToCreateEachRegion = 200;
        private string currentRegion;
        private string testDefaultOrganicAgencyName = "NewItemProcessorPerformanceTest DefaultOrganicAgencyName";

        [TestInitialize]
        public void AddIRMANewItemsPerformanceInitializeTest()
        {
            Cache.brandNameToBrandAbbreviationMap?.Clear();
            Cache.defaultCertificationAgencies?.Clear();
            Cache.itemSbTeamEventEnabledRegions?.Clear();
            Cache.nationalClassCodeToNationalClassId?.Clear();
            Cache.taxCodeToTaxId?.Clear();

            StartupOptions.Instance = 123;
            StartupOptions.RegionsToProcess = new string[] { "FL" };

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                if (!String.IsNullOrEmpty(connectionString))
                {

                    string sql = @"insert into dbo.IconItemChangeQueue (Item_Key,Identifier,ItemChgTypeID, InsertDate) 
                                    select top " + testNewItemToCreateEachRegion +
                                 @" ii.Item_Key, ii.Identifier, 1,  CONVERT(VARCHAR, GETDATE(), 23)
                                    from ItemIdentifier ii 
                                    inner join item i on i.Item_Key = ii.Item_Key
                                    where i.Deleted_Item = 0 and i.Remove_Item = 0
                                      and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
                                      and i.Retail_Sale = 1
                                     and left(ii.Identifier, 1) != '2'
                                      and len(ii.Identifier) > 9";
                    int returnCode = db.Database.ExecuteSqlCommand(sql);
                }
            }

            //ReCon relies on the existence of a Default Organic Certification Agency in order to 
            //assign it to items. Adding it here if it doesn't exist so that this test doesn't break.
            using (var context = new IconContext())
            {
                if (!context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.DefaultCertificationAgency))
                {
                    context.HierarchyClass.Add(new HierarchyClass
                    {
                        hierarchyID = Hierarchies.CertificationAgencyManagement,
                        hierarchyClassName = testDefaultOrganicAgencyName,
                        hierarchyLevel = 1,
                        HierarchyClassTrait = new List<HierarchyClassTrait>
                        {
                            new HierarchyClassTrait { traitID = Traits.DefaultCertificationAgency, traitValue = OrganicCertificationAgency },
                            new HierarchyClassTrait { traitID = Traits.Organic, traitValue = OrganicCertificationAgency }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        [TestCleanup]
        public void AddIRMANewItemsPerformanceCleanup()
        {
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                if (!String.IsNullOrEmpty(connectionString))
                {

                    string sql = @"delete dbo.IconItemChangeQueue 
                                   where InsertDate = CONVERT(VARCHAR, GETDATE(), 23)";
                    int returnCode = db.Database.ExecuteSqlCommand(sql);
                }
            }

            //Removing test default certification agency in case it was added
            using (var context = new IconContext())
            {
                var testDefaultCertificationAgency = context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyClassName == testDefaultOrganicAgencyName);
                if (testDefaultCertificationAgency != null)
                {
                    context.HierarchyClassTrait.RemoveRange(testDefaultCertificationAgency.HierarchyClassTrait);
                    context.SaveChanges();
                    context.HierarchyClass.Remove(testDefaultCertificationAgency);
                    context.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void AddIRMANewItemsPerformanceTest()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Given
            var iconReferenceProcessor = BuildIconReferenceProcessor();
            iconReferenceProcessor.Run();

            Console.WriteLine("Finished preparing the RegionalEventController.");

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                currentRegion = irmaRegion;
                var newItemProcessor = BuildNewItemProcessor(irmaContext);

                //When
                newItemProcessor.Run();

                Console.WriteLine("Regional Event Controller finished running for " + irmaRegion);
            }

            Console.WriteLine("Regional Event Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));
            stopWatch.Stop();

            //Then
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                var getIrmaNewItemsQueryHandler = new GetIrmaNewItemsQueryHandler(irmaContext);

                GetIrmaNewItemsQuery irmaNewItemsQuery = new GetIrmaNewItemsQuery();
                List<IrmaNewItem> newItems = getIrmaNewItemsQueryHandler.Execute(irmaNewItemsQuery);

                Assert.IsTrue(newItems.Count == 0);
            }

        }

        private INewItemProcessor BuildIconReferenceProcessor()
        {
            var logger = new NLogLoggerInstance<IconReferenceProcessor>(StartupOptions.Instance.ToString());

            var getTaxCodeToTaxIdMappingQueryHandler = new GetTaxCodeToTaxIdMappingQueryHandler(iconContext);
            var getNationcalClassCodeToIdMappingQueryHandler = new GetNationalClassCodeToClassIdMappingQueryHandler(iconContext);
            var getBrandAbbreviationQueryQueryHandler = new GetBrandAbbreviationQueryHandler(iconContext);
            var getDefaultCertificationAgenciesQueryHandler = new GetDefaultCertificationAgenciesQueryHandler(iconContext);

            return new IconReferenceProcessor(
                logger,
                new Mock<IEmailClient>().Object,
                iconContext,
                getTaxCodeToTaxIdMappingQueryHandler,
                new GetRegionalSettingsBySettingsKeyNameQuery(iconContext),
                getNationcalClassCodeToIdMappingQueryHandler,
                getBrandAbbreviationQueryQueryHandler,
                getDefaultCertificationAgenciesQueryHandler);
        }

        private INewItemProcessor BuildNewItemProcessor(IrmaContext irmaContext)
        {
            var logger = new NLogLoggerInstance<NewItemProcessor>(StartupOptions.Instance.ToString());

            var getIrmaNewItemsQueryHandler = new GetIrmaNewItemsQueryHandler(irmaContext);

            var getAppConfigValueQueryHandler = new GetAppConfigValueQueryHandler(irmaContext);

            var getInvalidInProcessedQueueEntriesQueryHandler = new GetInvalidInProcessedQueueEntriesQueryHandler(irmaContext);

            var deleteNewItemsFromIrmaQueueCommandHandler = new DeleteNewItemsFromIrmaQueueCommandHandler(new NLogLoggerInstance<DeleteNewItemsFromIrmaQueueCommandHandler>(StartupOptions.Instance.ToString()),
               irmaContext);

            var markIconItemChangeQueueEntriesInProcessByCommandHandler = new MarkIconItemChangeQueueEntriesInProcessByCommandHandler(new NLogLoggerInstance<MarkIconItemChangeQueueEntriesInProcessByCommandHandler>(StartupOptions.Instance.ToString()),
                irmaContext);

            var getValidatedItemsQueryHandler = new GetValidatedItemsQueryHandler(iconContext);

            var getBrandNewScanCodesQueryHandler = new GetBrandNewScanCodesQueryHandler(iconContext);

            var getScanCodesNeedSubscriptionQueryHandler = new GetScanCodesNeedSubscriptionQueryHandler(iconContext);

            return new NewItemProcessor(
                logger,
                iconContext,
                currentRegion,
                getIrmaNewItemsQueryHandler,
                getAppConfigValueQueryHandler,
                getInvalidInProcessedQueueEntriesQueryHandler,
                deleteNewItemsFromIrmaQueueCommandHandler,
                markIconItemChangeQueueEntriesInProcessByCommandHandler,
                getValidatedItemsQueryHandler,
                getBrandNewScanCodesQueryHandler,
                getScanCodesNeedSubscriptionQueryHandler);
        }
    }
}
