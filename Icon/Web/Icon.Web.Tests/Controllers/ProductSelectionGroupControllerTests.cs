using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class ProductSelectionGroupControllerTests
    {
        ProductSelectionGroupController controller;

        Mock<ILogger> mockLogger;
        Mock<IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>> mockGetProductSelectionGroupsQueryHandler;
        Mock<IQueryHandler<GetProductSelectionGroupTypesParameters, List<ProductSelectionGroupType>>> mockGetProductSelectionGroupTypesQueryHandler;
        Mock<IQueryHandler<GetTraitsParameters, List<Trait>>> mockGetTraitsQueryHandler;
        Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQueryHandler;
        Mock<IManagerHandler<AddProductSelectionGroupManager>> mockAddProductSelectionGroupManagerHandler;
        Mock<IManagerHandler<UpdateProductSelectionGroupManager>> mockUpdateProductSelectionGroupManagerHandler;
        Mock<ControllerContext> mockContext;
        Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQueryHandler;

        List<Trait> traits;
        List<ProductSelectionGroupType> types;
        List<ProductSelectionGroup> mappings;
        List<Hierarchy> hierarchies;
        List<AttributeModel> attributes;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetProductSelectionGroupsQueryHandler = new Mock<IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>>();
            mockGetProductSelectionGroupTypesQueryHandler = new Mock<IQueryHandler<GetProductSelectionGroupTypesParameters, List<ProductSelectionGroupType>>>();
            mockGetTraitsQueryHandler = new Mock<IQueryHandler<GetTraitsParameters, List<Trait>>>();
            mockGetHierarchyQueryHandler = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            mockAddProductSelectionGroupManagerHandler = new Mock<IManagerHandler<AddProductSelectionGroupManager>>();
            mockUpdateProductSelectionGroupManagerHandler = new Mock<IManagerHandler<UpdateProductSelectionGroupManager>>();
            mockContext = new Mock<ControllerContext>();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();

            controller = new ProductSelectionGroupController(
                mockLogger.Object,
                mockGetProductSelectionGroupsQueryHandler.Object,
                mockGetProductSelectionGroupTypesQueryHandler.Object,
                mockGetTraitsQueryHandler.Object,
                mockGetHierarchyQueryHandler.Object,
                mockAddProductSelectionGroupManagerHandler.Object,
                mockUpdateProductSelectionGroupManagerHandler.Object,
                mockGetAttributesQueryHandler.Object);
            controller.ControllerContext = mockContext.Object;

            mappings = new List<ProductSelectionGroup>
            {
                BuildProductSelectionGroup(1, "psg 1", 1, "trait 1", "val 1", 1, "type 1", null, 1, "attribute value1"),
                BuildProductSelectionGroup(2, "psg 2", 1, "trait 2", "val 2", 2, "type 2", null, 2, "attribute value2"),
                BuildProductSelectionGroup(3, "psg 3", null, null, null, 3, "type 3", 1, 3, "attribute value3"),
                BuildProductSelectionGroup(4, "psg 4", null, null, null, 4, "type 4", 2, 4, "attribute value4")
            };
            types = new List<ProductSelectionGroupType>
            {
                new ProductSelectionGroupType { ProductSelectionGroupTypeId = 1, ProductSelectionGroupTypeName = "Consumable"},
                new ProductSelectionGroupType { ProductSelectionGroupTypeId = 2, ProductSelectionGroupTypeName = "Online"}
            };
            traits = new List<Trait>
            {
                new Trait { traitID = 1, traitDesc = TraitDescriptions.AgeRestrict,TraitGroup = new TraitGroup {traitGroupID = 2, traitGroupCode = "ILA", traitGroupDesc = "Item-Locale Attributes"}},
                new Trait { traitID = 2, traitDesc = "Food Stamp", TraitGroup = new TraitGroup {traitGroupID = 2, traitGroupCode = "ILA", traitGroupDesc = "Item-Locale Attributes"}}
            };
            hierarchies = new List<Hierarchy>
            {
                new Hierarchy
                {
                    hierarchyID = Hierarchies.Merchandise,
                    hierarchyName = HierarchyNames.Merchandise,
                    HierarchyClass = new List<HierarchyClass>
                    {
                        new TestHierarchyClassBuilder().WithHierarchyClassId(1)
                            .WithHierarchyLevel(5).WithHierarchyParentClassId(4).WithHierarchyClassName("Tester1").Build(),
                        new TestHierarchyClassBuilder().WithHierarchyClassId(2)
                            .WithHierarchyLevel(5).WithHierarchyParentClassId(4).WithHierarchyClassName("Tester2").Build(),
                        new TestHierarchyClassBuilder().WithHierarchyClassId(3)
                            .WithHierarchyLevel(4).WithHierarchyParentClassId(5).WithHierarchyClassName("Tester3").Build()
                    }
                }
            };
            attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 1, AttributeName = "Test", AttributeGroupId = 1
                },
                new AttributeModel
                {
                    AttributeId = 2, AttributeName = "Test2", AttributeGroupId = 1
                }
            };

        }

        [TestMethod]
        public void Index_ProductSelectionGroupsExist_ShouldReturnProductSelectionGroupGridViewModel()
        {
            //Given
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(mappings);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            //When
            var result = controller.Index() as ViewResult;
            var viewModel = result.Model as ProductSelectionGroupGridViewModel;

            //Then
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(mappings.Count, viewModel.ProductSelectionGroups.Count);
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count);
            Assert.AreEqual(traits.Count, viewModel.Traits.Count);
            Assert.AreEqual(hierarchies.First().HierarchyClass.Where(hc => hc.hierarchyLevel == 5).Count(), viewModel.MerchandiseHierarchyClasses.Count);
            Assert.AreEqual(attributes.Count, viewModel.Attributes.Count());

            for (int i = 0; i < mappings.Count; i++)
            {
                Assert.AreEqual(mappings[i].ProductSelectionGroupId, viewModel.ProductSelectionGroups[i].ProductSelectionGroupId);
                Assert.AreEqual(mappings[i].ProductSelectionGroupName, viewModel.ProductSelectionGroups[i].ProductSelectionGroupName);
                Assert.AreEqual(mappings[i].ProductSelectionGroupTypeId, viewModel.ProductSelectionGroups[i].ProductSelectionGroupTypeId);
                Assert.AreEqual(mappings[i].TraitId, viewModel.ProductSelectionGroups[i].TraitId);
                Assert.AreEqual(mappings[i].TraitValue, viewModel.ProductSelectionGroups[i].TraitValue);
                Assert.AreEqual(mappings[i].MerchandiseHierarchyClassId, viewModel.ProductSelectionGroups[i].MerchandiseHierarchyClassId);
                Assert.AreEqual(mappings[i].AttributeId, viewModel.ProductSelectionGroups[i].AttributeId);
                Assert.AreEqual(mappings[i].AttributeValue, viewModel.ProductSelectionGroups[i].AttributeValue);
            }
            for (int i = 0; i < types.Count; i++)
            {
                Assert.AreEqual(types[i].ProductSelectionGroupTypeId, viewModel.ProductSelectionGroupTypes[i].ProductSelectionGroupTypeId);
                Assert.AreEqual(types[i].ProductSelectionGroupTypeName, viewModel.ProductSelectionGroupTypes[i].ProductSelectionGroupTypeName);
            }
            for (int i = 0; i < traits.Count; i++)
            {
                Assert.AreEqual(traits[i].traitDesc, viewModel.Traits[i].TraitDesc);
                Assert.AreEqual(traits[i].traitID, viewModel.Traits[i].TraitId);
            }
            for (int i = 0; i < attributes.Count; i++)
            {
                Assert.AreEqual(attributes[i].AttributeName, viewModel.Attributes.ToList()[i].AttributeName);
                Assert.AreEqual(attributes[i].AttributeId, viewModel.Attributes.ToList()[i].AttributeId);
            }
        }

        [TestMethod]
        public void Index_ProductSelectionGroupExistsOrNot_IndexViewReturned()
        {
            // Given
            string expectedViewName = "Index";

            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(mappings);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            // When
            //When
            var result = controller.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result, "The ViewResult is null");
            Assert.AreEqual(expectedViewName, result.ViewName, "The expected ViewName does not match the actual ViewName");
        }

        private ProductSelectionGroup BuildProductSelectionGroup(int psgId, string psgName, int? traitId, string traitDesc, string traitValue, int psgTypeId, string psgTypeName, int? merchandiseHierarchyClassId, int? attributeId, string attributeValue)
        {
            return new ProductSelectionGroup
            {
                ProductSelectionGroupId = psgId,
                ProductSelectionGroupName = psgName,
                ProductSelectionGroupTypeId = psgTypeId,
                TraitId = traitId,
                TraitValue = traitValue,
                MerchandiseHierarchyClassId = merchandiseHierarchyClassId,
                AttributeId = attributeId,
                AttributeValue = attributeValue
            };
        }

        [TestMethod]
        public void Create_ShouldReturnProductSelectionGroupCreateViewModel()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            //When
            var result = controller.Create() as ViewResult;
            var model = result.Model as ProductSelectionGroupCreateViewModel;

            //Then
            Assert.AreEqual(traits.Count, model.Traits.Count());
            Assert.AreEqual(types.Count, model.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), model.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in model.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in model.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in model.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_ShouldSaveProductSelectionGroupCreateViewModel()
        {
            //Given
            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            mockAddProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<AddProductSelectionGroupManager>())).Verifiable();

            //When
            var result = controller.Create(viewModel) as RedirectToRouteResult;

            //Then
            Assert.AreEqual(1, result.RouteValues.Values.Count);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Successfully created the Product Selection Group.", controller.ViewData["Message"]);
            mockAddProductSelectionGroupManagerHandler.Verify();
        }

        [TestMethod]
        public void Create_ExceptionOccursWhenSaving_ShouldReturnViewWithErrorMessage()
        {
            //Given
            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockAddProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<AddProductSelectionGroupManager>()))
                .Throws(new Exception("Test Exception"));

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual("Test Exception", controller.ViewData["Error"]);
            Assert.AreEqual(traits.Count, viewModel.Traits.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in viewModel.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_InvalidModel_ShouldReturnCreateView()
        {
            // Given
            var viewModel = new ProductSelectionGroupCreateViewModel();
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            mockAddProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<AddProductSelectionGroupManager>()))
                .Throws(new Exception("Test Exception"));

            string expectedViewName = "Create";

            // When
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Create(viewModel) as ViewResult;

            // Then
            Assert.AreEqual(expectedViewName, result.ViewName, "Expected ViewName does not match the actual ViewName.");
        }

        [TestMethod]
        public void Create_InvalidModel_ShouldReturnViewWithBuiltViewModel()
        {
            // Given
            var viewModel = new ProductSelectionGroupCreateViewModel();
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            mockAddProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<AddProductSelectionGroupManager>()))
                .Throws(new Exception("Test Exception"));

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            // When
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Create(viewModel) as ViewResult;

            // Then
            Assert.AreEqual(traits.Count, viewModel.Traits.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in viewModel.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_MerchSubBrickHasValueWithNotNullTraitIdAndNotNullTraitValue_ShouldReturnViewWithErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = 1;
            viewModel.SelectedTraitId = 2;
            viewModel.TraitValue = "test";
            viewModel.SelectedAttributeId = 3;
            viewModel.AttributeValue = "test";

            string expectedErrorMessage = "There cannot be a Merchandise Sub Brick assigned to the PSG if there is a Trait/Attribute and Trait Value/Attribute Value assigned.";

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual(expectedErrorMessage, controller.ViewData["Error"]);
            Assert.AreEqual(traits.Count, viewModel.Traits.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in viewModel.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_NullSubBrickWithNotNullTraitIdAndNullTraitValue_ShouldReturnViewWithErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = null;
            viewModel.SelectedTraitId = 2;
            viewModel.TraitValue = null;

            string expectedErrorMessage = "Trait/Attribute must have a value for the selected Trait/Attribute.";

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual(expectedErrorMessage, controller.ViewData["Error"]);
            Assert.AreEqual(traits.Count, viewModel.Traits.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in viewModel.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_NullSubBrickWithNullTraitIdAndNotNullTraitValue_ShouldReturnViewWithErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = null;
            viewModel.SelectedTraitId = null;
            viewModel.TraitValue = "test";

            string expectedErrorMessage = "Trait/Attribute must be selected for the specific Trait/Attribute Value.";

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual(expectedErrorMessage, controller.ViewData["Error"]);
            Assert.AreEqual(traits.Count, viewModel.Traits.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var traitSelectListItem in viewModel.Traits)
            {
                Assert.AreEqual(traits[x].traitDesc, traitSelectListItem.Text);
                Assert.AreEqual(traits[x].traitID.ToString(), traitSelectListItem.Value);
                x++;
            }

            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }

            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_PsgExistsWithSameMerchandiseHierarchyClass_ShouldReturnViewWithErrorMessage()
        {
            //Given
            var viewModel = new ProductSelectionGroupCreateViewModel
            {
                ProductSelectionGroupName = "Test PSG",
                SelectedProductSelectionGroupTypeId = 1,
                SelectedMerchandiseHierarchyClassId = 1,
                SelectedTraitId = null,
                TraitValue = null,
                SelectedAttributeId = null,
                AttributeValue = null
            };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(mappings);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            //When
            var result = controller.Create(viewModel);

            //Then
            mockAddProductSelectionGroupManagerHandler.Verify(m => m.Execute(It.IsAny<AddProductSelectionGroupManager>()), Times.Never);
            Assert.AreEqual("A PSG is already associated to the selected Merchandise Hierarchy Class.", controller.ViewData["Error"]);
        }

        [TestMethod]
        public void SaveChangesInGrid_NoErrors_ShouldReturnSuccess()
        {
            //Given
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = new ProductSelectionGroupViewModel()
                    }
                };
            mockUpdateProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<UpdateProductSelectionGroupManager>()))
                .Verifiable();
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.AreEqual(true, result.GetDataProperty("Success"));
            mockUpdateProductSelectionGroupManagerHandler.Verify();
        }

        [TestMethod]
        public void SaveChangesInGrid_NoTransactions_ShouldReturnFailure()
        {
            //Given
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", "[]"}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("No new values were specified.", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void SaveChangesInGrid_ErrorSavingProductSelectionGroup_ShouldReturnFailure()
        {
            //Given
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = new ProductSelectionGroupViewModel()
                    }
                };
            mockUpdateProductSelectionGroupManagerHandler.Setup(m => m.Execute(It.IsAny<UpdateProductSelectionGroupManager>()))
                .Throws(new Exception("Test Exception"));
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("Test Exception", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void SaveChangesInGrid_PsgExistsWithSameMerchandiseHierarchyClass_ShouldReturnViewWithErrorMessage()
        {
            //Given
            var viewModel = new ProductSelectionGroupViewModel
            {
                ProductSelectionGroupName = "Test PSG",
                ProductSelectionGroupTypeId = 1,
                MerchandiseHierarchyClassId = 1,
                TraitId = null,
                TraitValue = null
            };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(mappings);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = viewModel
                    }
                };
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            mockUpdateProductSelectionGroupManagerHandler.Verify(m => m.Execute(It.IsAny<UpdateProductSelectionGroupManager>()), Times.Never);
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("A PSG is already associated to the selected Merchandise Hierarchy Class.", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void Create_MerchSubBrickHasValueWithNotNullAttributeIdAndNotNullAttributeValue_ShouldReturnViewWithErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler
                .Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetProductSelectionGroupsQueryHandler
                .Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = 1;
            viewModel.SelectedAttributeId = 3;
            viewModel.AttributeValue = "test";

            string expectedErrorMessage = "There cannot be a Merchandise Sub Brick assigned to the PSG if there is a Trait/Attribute and Trait Value/Attribute Value assigned.";

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual(expectedErrorMessage, controller.ViewData["Error"]);
            Assert.AreEqual(attributes.Count, viewModel.Attributes.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(),
                viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var attributeSelectListItem in viewModel.Attributes)
            {
                Assert.AreEqual(attributes[x].AttributeName, attributeSelectListItem.Text);
                Assert.AreEqual(attributes[x].AttributeId.ToString(), attributeSelectListItem.Value);
                x++;
            }
            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }
            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_PSGWithNullAttributeIdAndNotNullAttributeValue_ShouldReturnViewWithErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler
                .Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetProductSelectionGroupsQueryHandler
                .Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = null;
            viewModel.SelectedAttributeId = null;
            viewModel.AttributeValue = "test";


            string expectedErrorMessage = "Trait/Attribute must be selected for the specific Trait/Attribute Value.";

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.AreEqual(expectedErrorMessage, controller.ViewData["Error"]);
            Assert.AreEqual(attributes.Count, viewModel.Attributes.Count());
            Assert.AreEqual(types.Count, viewModel.ProductSelectionGroupTypes.Count());
            Assert.AreEqual(hierarchyClasses.Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick).Count(), viewModel.MerchandiseHierarchyClasses.Count());

            int x = 0;
            foreach (var attributeSelectListItem in viewModel.Attributes)
            {
                Assert.AreEqual(attributes[x].AttributeName, attributeSelectListItem.Text);
                Assert.AreEqual(attributes[x].AttributeId.ToString(), attributeSelectListItem.Value);
                x++;
            }
            x = 0;
            foreach (var typeSelectListItem in viewModel.ProductSelectionGroupTypes)
            {
                Assert.AreEqual(types[x].ProductSelectionGroupTypeId.ToString(), typeSelectListItem.Value);
                Assert.AreEqual(types[x].ProductSelectionGroupTypeName, typeSelectListItem.Text);
                x++;
            }
            x = 0;
            foreach (var merchClass in viewModel.MerchandiseHierarchyClasses)
            {
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassName, merchClass.Text);
                Assert.AreEqual(hierarchyClassViewModels[x].HierarchyClassId.ToString(), merchClass.Value);
                x++;
            }
        }

        [TestMethod]
        public void Create_PSGWithTraitAndAttributeValues_ShouldReturnViewWithOutErrorMessage()
        {
            //Given
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);

            List<HierarchyClass> hierarchyClasses = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Select(hc => new HierarchyClassViewModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyClassName = hc.hierarchyClassName + "|" + hc.hierarchyClassID,
                    HierarchyId = hc.hierarchyID,
                    HierarchyLevel = hc.hierarchyLevel,
                    HierarchyParentClassId = hc.hierarchyParentClassID
                })
                .ToList();

            ProductSelectionGroupCreateViewModel viewModel = new ProductSelectionGroupCreateViewModel();
            viewModel.SelectedMerchandiseHierarchyClassId = null;
            viewModel.SelectedAttributeId = 1;
            viewModel.AttributeValue = "test1";
            viewModel.SelectedTraitId = 2;
            viewModel.TraitValue = "test2";

            //When
            var result = controller.Create(viewModel) as RedirectToRouteResult;

            //Then
            Assert.AreEqual(1, result.RouteValues.Values.Count);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Successfully created the Product Selection Group.", controller.ViewData["Message"]);
            mockAddProductSelectionGroupManagerHandler.Verify();
        }

        [TestMethod]
        public void Edit_MerchSubBrickHasValueWithNotNullAttributeIdAndNotNullAttributeValue_ShouldReturnViewWithErrorMessage()
        {
            var viewModel = new ProductSelectionGroupViewModel
            {
                ProductSelectionGroupName = "Test PSG1",
                ProductSelectionGroupTypeId = 5,
                MerchandiseHierarchyClassId = 2,
                AttributeId = 3,
                AttributeValue = "Test"
            };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = viewModel
                    }
                };
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            mockUpdateProductSelectionGroupManagerHandler.Verify(m => m.Execute(It.IsAny<UpdateProductSelectionGroupManager>()), Times.Never);
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("There cannot be a Merchandise Sub Brick assigned to the PSG if there is a Trait/Attribute and Trait Value/Attribute Value assigned.", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void Edit_PsgWithNotNullAttributeIdAndNullAttributeValue_ShouldReturnViewWithErrorMessage()
        {
            var viewModel = new ProductSelectionGroupViewModel
            {
                ProductSelectionGroupName = "Test PSG1",
                ProductSelectionGroupTypeId = 1,
                MerchandiseHierarchyClassId = null,
                AttributeId = 2,
                AttributeValue = null
            };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = viewModel
                    }
                };
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            mockUpdateProductSelectionGroupManagerHandler.Verify(m => m.Execute(It.IsAny<UpdateProductSelectionGroupManager>()), Times.Never);
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("Trait/Attribute must have a value for the selected Trait/Attribute.", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void Edit_PsgWithTraitAndAttributeValues_ShouldReturnViewWithOutErrorMessage()
        {
            var viewModel = new ProductSelectionGroupViewModel
            {
                ProductSelectionGroupName = "Test PSG1",
                ProductSelectionGroupTypeId = 1,
                MerchandiseHierarchyClassId = null,
                TraitId = 1,
                TraitValue = "test1",
                AttributeId = 2,
                AttributeValue = "Test2"
            };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());
            mockGetHierarchyQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyParameters>()))
                .Returns(hierarchies);
            mockGetTraitsQueryHandler.Setup(m => m.Search(It.IsAny<GetTraitsParameters>()))
                .Returns(traits);
            mockGetProductSelectionGroupTypesQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupTypesParameters>()))
                .Returns(types);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(attributes);
            List<Transaction<ProductSelectionGroupViewModel>> transactions = new List<Transaction<ProductSelectionGroupViewModel>>
                {
                    new Transaction<ProductSelectionGroupViewModel>
                    {
                        row = viewModel
                    }
                };
            mockContext.SetupGet(c => c.HttpContext.Request.Form)
                .Returns(new NameValueCollection
                {
                    {"ig_transactions", new JavaScriptSerializer().Serialize(transactions)}
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.AreEqual(true, result.GetDataProperty("Success"));
            mockUpdateProductSelectionGroupManagerHandler.Verify();
        }
    }
}