using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionalEventController.Common;
using RegionalEventController.Controller.Processors;
using RegionalEventController.DataAccess.Queries;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RegionalEventController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class IconReferenceProcessorTest
    {
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            Cache.brandNameToBrandAbbreviationMap?.Clear();
            Cache.defaultCertificationAgencies?.Clear();
            Cache.itemSbTeamEventEnabledRegions?.Clear();
            Cache.nationalClassCodeToNationalClassId?.Clear();
            Cache.taxCodeToTaxId?.Clear();

            this.context = new IconContext();
            this.transaction = this.context.Database.BeginTransaction();

            // Cleanup the database from duplicate brand names
            var duplicateBrandNames = this.context.HierarchyClass
                .Where(hc => hc.hierarchyID == Hierarchies.Brands)
                .GroupBy(hc => hc.hierarchyClassName.Length > 25 
                    ? hc.hierarchyClassName.Substring(0, 24) 
                    : hc.hierarchyClassName)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            // Remove hierarchy class traits from the brands
            foreach(var brands in duplicateBrandNames)
            {
                var brandsHierarchyClassTraits = this.context.HierarchyClassTrait
                    .Where(hct => hct.HierarchyClass.hierarchyClassID == brands.hierarchyClassID)
                    .ToList();

                this.context.HierarchyClassTrait.RemoveRange(brandsHierarchyClassTraits);
            }

            this.context.HierarchyClass.RemoveRange(duplicateBrandNames);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction?.Rollback();
            this.transaction?.Dispose();
        }

        [TestMethod]
        public void RunIconReferenceProcessor_ProcessorSuccessfullyExecuted_taxCodeToTaxIdDictionaryIsPopulated()
        {
            //Given
            var iconReferenceProcessor = BuildIconReferenceProcessor();

            //When
            iconReferenceProcessor.Run();

            //Then
            var taxes = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation)
                         .ToList();

            Assert.AreEqual(taxes.Count(), Cache.taxCodeToTaxId.Count);

            foreach (KeyValuePair<string, int> taxCodeToTaxId in Cache.taxCodeToTaxId)
            {
                //In IRMA, ExternalTaxGroupCode on dbo.TaxClass table is defined as varchar(7)
                Assert.IsTrue(taxCodeToTaxId.Key.Length <= 7);
            }
        }

        [TestMethod]
        public void RunIconReferenceProcessor_ProcessorSuccessfullyExecuted_NationalClassCodeToIdMappingDictionaryIsPopulated()
        {
            //Given
            var iconReferenceProcessor = BuildIconReferenceProcessor();

            //When
            iconReferenceProcessor.Run();

            //Then
            var nationalClasses = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.NationalClassCode)
                         .ToList();

            Assert.AreEqual(nationalClasses.Count(), Cache.nationalClassCodeToNationalClassId.Count);
        }

        [TestMethod]
        public void RunIconReferenceProcessor_ProcessorSuccessfullyExecuted_BrandAbbreviationMappingDictionaryIsPopulated()
        {
            //Given
            var iconReferenceProcessor = BuildIconReferenceProcessor();

            //When
            iconReferenceProcessor.Run();

            //Then
            var brandClasses = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation)
                         .ToList();

            Assert.AreEqual(brandClasses.Count(), Cache.brandNameToBrandAbbreviationMap.Count);
        }

        private INewItemProcessor BuildIconReferenceProcessor()
        {
            var mockLogger = new Mock<ILogger<IconReferenceProcessor>>().Object;

            var getTaxCodeToTaxIdMappingQueryHandler = new GetTaxCodeToTaxIdMappingQueryHandler(context);
            var getNationcalCodeToIdMappingQueryHandler = new GetNationalClassCodeToClassIdMappingQueryHandler(context);
            var getBrandAbbreviationQueryQueryHandler = new GetBrandAbbreviationQueryHandler(context);
            var getDefaultCertificationAgenciesQueryHandler = new GetDefaultCertificationAgenciesQueryHandler(context);

            return new IconReferenceProcessor(
                mockLogger,
                new Mock<IEmailClient>().Object,
                context,
                getTaxCodeToTaxIdMappingQueryHandler,
                new GetRegionalSettingsBySettingsKeyNameQuery(context),
                getNationcalCodeToIdMappingQueryHandler,
                getBrandAbbreviationQueryQueryHandler,
                getDefaultCertificationAgenciesQueryHandler);
        }
    }
}
