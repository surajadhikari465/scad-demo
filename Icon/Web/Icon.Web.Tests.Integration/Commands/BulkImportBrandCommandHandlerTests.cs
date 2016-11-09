using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class BulkImportBrandCommandHandlerTests
    {
        private BulkImportBrandCommandHandler commandHandler;
        private BulkImportCommand<BulkImportBrandModel> command;
        private Mock<ILogger> mockLogger;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<BulkImportBrandModel> brandModels;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger>();
            commandHandler = new BulkImportBrandCommandHandler(context, mockLogger.Object);
            brandModels = new List<BulkImportBrandModel>
            {                
                new BulkImportBrandModel 
                {
                    BrandId = "0",
                    BrandName = "TestBrand1",
                    BrandAbbreviation = "BrandAbbr1"
                }, 
                new BulkImportBrandModel
                {
                    BrandId = "0",
                    BrandName = "TestBrand2",
                    BrandAbbreviation = "BrandAbbr2"
                },
                new BulkImportBrandModel
                {
                    BrandId = "0",
                    BrandName = "TestBrand3",
                    BrandAbbreviation = "BrandAbbr3"
                }
            };
            command = new BulkImportCommand<BulkImportBrandModel>()
            {
                BulkImportData = brandModels,
                UserName = "TestUser"
            };
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void BulkImportBrand_BrandsAndBrandAbbreviationsDontExist_ShouldAddBrandsAndBrandAbbreviations()
        {
            //When
            commandHandler.Execute(command);

            //Then
            var brands = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyClassName == "TestBrand1"
                    || hc.hierarchyClassName == "TestBrand2"
                    || hc.hierarchyClassName == "TestBrand3")
                .ToList();

            Assert.AreEqual(3, brands.Count());
            Assert.IsNotNull(brands[0].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr1"));
            Assert.IsNotNull(brands[1].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr2"));
            Assert.IsNotNull(brands[2].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr3"));

            var brandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation &&
                (hct.traitValue == "BrandAbbr1"
                    || hct.traitValue == "BrandAbbr2"
                    || hct.traitValue == "BrandAbbr3"))
                .ToList();
            Assert.AreEqual(3, brandAbbreviations.Count);
        }

        [TestMethod]
        public void BulkImportBrand_BrandsExistButBrandAbbreviationsDontExist_ShouldAddBrandAbbreviations()
        {
            //Given
            var existingBrands = new List<HierarchyClass>
                {
                    new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = brandModels[0].BrandName },
                    new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = brandModels[1].BrandName },
                    new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = brandModels[2].BrandName }
                };
            context.HierarchyClass.AddRange(existingBrands);
            context.SaveChanges();
            brandModels[0].BrandId = existingBrands[0].hierarchyClassID.ToString();
            brandModels[1].BrandId = existingBrands[1].hierarchyClassID.ToString();
            brandModels[2].BrandId = existingBrands[2].hierarchyClassID.ToString();

            //When
            commandHandler.Execute(command);

            //Then
            var brands = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyClassName == "TestBrand1"
                    || hc.hierarchyClassName == "TestBrand2"
                    || hc.hierarchyClassName == "TestBrand3")
                .ToList();

            Assert.AreEqual(3, brands.Count());
            Assert.IsNotNull(brands[0].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr1"));
            Assert.IsNotNull(brands[1].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr2"));
            Assert.IsNotNull(brands[2].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr3"));

            var brandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation &&
                (hct.traitValue == "BrandAbbr1"
                    || hct.traitValue == "BrandAbbr2"
                    || hct.traitValue == "BrandAbbr3"))
                .ToList();
            Assert.AreEqual(3, brandAbbreviations.Count);
        }

        [TestMethod]
        public void BulkImportBrand_BrandsAndAbbreviationsExist_ShouldUpdateBrandAbbreviations()
        {
            //Given
            var existingBrands = new List<HierarchyClass>
                {
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[0].BrandName,
                            HierarchyClassTrait = new List<HierarchyClassTrait>
                                { 
                                    new HierarchyClassTrait 
                                        { 
                                            traitID = Traits.BrandAbbreviation, 
                                            traitValue = brandModels[0].BrandAbbreviation 
                                        } 
                                } 
                        },
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[1].BrandName,
                            HierarchyClassTrait = new List<HierarchyClassTrait>
                                { 
                                    new HierarchyClassTrait 
                                        { 
                                            traitID = Traits.BrandAbbreviation, 
                                            traitValue = brandModels[1].BrandAbbreviation
                                        } 
                                } 
                        },
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[2].BrandName,
                            HierarchyClassTrait = new List<HierarchyClassTrait>
                                { 
                                    new HierarchyClassTrait 
                                        { 
                                            traitID = Traits.BrandAbbreviation, 
                                            traitValue = brandModels[2].BrandAbbreviation
                                        } 
                                } 
                        },
                };
            context.HierarchyClass.AddRange(existingBrands);
            context.SaveChanges();

            brandModels[0].BrandId = existingBrands[0].hierarchyClassID.ToString();
            brandModels[0].BrandAbbreviation = "BrandAbbr4";

            brandModels[1].BrandId = existingBrands[1].hierarchyClassID.ToString();
            brandModels[1].BrandAbbreviation = "BrandAbbr5";

            brandModels[2].BrandId = existingBrands[2].hierarchyClassID.ToString();
            brandModels[2].BrandAbbreviation = "BrandAbbr6";

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var brand in existingBrands)
            {
                context.Entry<HierarchyClass>(brand).Reload();
                foreach (var trait in brand.HierarchyClassTrait)
                {
                    context.Entry<HierarchyClassTrait>(trait).Reload();
                }
            }
            var brands = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyClassName == "TestBrand1"
                    || hc.hierarchyClassName == "TestBrand2"
                    || hc.hierarchyClassName == "TestBrand3")
                .ToList();
            Assert.AreEqual(3, brands.Count());
            Assert.IsNotNull(brands[0].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr4"));
            Assert.IsNotNull(brands[1].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr5"));
            Assert.IsNotNull(brands[2].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr6"));

            var oldBrandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation &&
                (hct.traitValue == "BrandAbbr1"
                    || hct.traitValue == "BrandAbbr2"
                    || hct.traitValue == "BrandAbbr3"))
                .ToList();
            Assert.AreEqual(0, oldBrandAbbreviations.Count);

            var newBrandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation &&
                (hct.traitValue == "BrandAbbr4"
                    || hct.traitValue == "BrandAbbr5"
                    || hct.traitValue == "BrandAbbr6"))
                .ToList();
            Assert.AreEqual(3, newBrandAbbreviations.Count);
        }

        [TestMethod]
        public void BulkImportBrand_MultipleScenarios_ShouldAddAndUpdateBrandAbbreviations()
        {
            //Given
            var existingBrands = new List<HierarchyClass>
                {
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[0].BrandName,
                            HierarchyClassTrait = new List<HierarchyClassTrait>
                                { 
                                    new HierarchyClassTrait 
                                        { 
                                            traitID = Traits.BrandAbbreviation, 
                                            traitValue = brandModels[0].BrandAbbreviation 
                                        } 
                                } 
                        },
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[1].BrandName
                        }
                };

            context.HierarchyClass.AddRange(existingBrands);
            context.SaveChanges();

            brandModels[0].BrandId = existingBrands[0].hierarchyClassID.ToString();
            brandModels[0].BrandAbbreviation = "BrandAbbr4";

            brandModels[1].BrandId = existingBrands[1].hierarchyClassID.ToString();

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var brand in existingBrands)
            {
                context.Entry<HierarchyClass>(brand).Reload();
                foreach (var trait in brand.HierarchyClassTrait)
                {
                    context.Entry<HierarchyClassTrait>(trait).Reload();
                }
            }
            var brands = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyClassName == "TestBrand1"
                    || hc.hierarchyClassName == "TestBrand2"
                    || hc.hierarchyClassName == "TestBrand3")
                .ToList();
            Assert.AreEqual(3, brands.Count());
            Assert.IsNotNull(brands[0].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr4"));
            Assert.IsNotNull(brands[1].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr2"));
            Assert.IsNotNull(brands[2].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr3"));

            var oldBrandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr1")
                .ToList();
            Assert.AreEqual(0, oldBrandAbbreviations.Count);

            var newBrandAbbreviations = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation &&
                (hct.traitValue == "BrandAbbr4"
                    || hct.traitValue == "BrandAbbr2"
                    || hct.traitValue == "BrandAbbr3"))
                .ToList();
            Assert.AreEqual(3, newBrandAbbreviations.Count);
        }

        [TestMethod]
        public void BulkImportBrand_EmptyBrandAbbreviation_ShouldNotAddOrUpdateBrandAbbreviation()
        {
            //Given
            var existingBrands = new List<HierarchyClass>
                {
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[0].BrandName,
                            HierarchyClassTrait = new List<HierarchyClassTrait>
                                { 
                                    new HierarchyClassTrait 
                                        { 
                                            traitID = Traits.BrandAbbreviation, 
                                            traitValue = brandModels[0].BrandAbbreviation 
                                        } 
                                } 
                        },
                    new HierarchyClass 
                        { 
                            hierarchyID = Hierarchies.Brands, 
                            hierarchyClassName = brandModels[1].BrandName
                        }
                };

            context.HierarchyClass.AddRange(existingBrands);
            context.SaveChanges();

            brandModels[0].BrandId = existingBrands[0].hierarchyClassID.ToString();
            brandModels[0].BrandAbbreviation = String.Empty;

            brandModels[1].BrandId = existingBrands[1].hierarchyClassID.ToString();
            brandModels[1].BrandAbbreviation = String.Empty;

            brandModels[2].BrandAbbreviation = String.Empty;

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var brand in existingBrands)
            {
                context.Entry<HierarchyClass>(brand).Reload();
                foreach (var trait in brand.HierarchyClassTrait)
                {
                    context.Entry<HierarchyClassTrait>(trait).Reload();
                }
            }
            var brands = context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyClassName == "TestBrand1"
                    || hc.hierarchyClassName == "TestBrand2"
                    || hc.hierarchyClassName == "TestBrand3")
                .ToList();
            Assert.AreEqual(3, brands.Count());
            Assert.IsNotNull(brands[0].HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.BrandAbbreviation && hct.traitValue == "BrandAbbr1"));
            Assert.IsFalse(brands[1].HierarchyClassTrait.Any(hct => hct.traitID == Traits.BrandAbbreviation));
            Assert.IsFalse(brands[2].HierarchyClassTrait.Any(hct => hct.traitID == Traits.BrandAbbreviation));
        }
    }
}
