using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Extensions
{
    [TestClass]
    public class WebExtensionsTests
    {
        [TestMethod]
        public void ToFlattenedHierarchyClassString_HierarchyClassIsFiveLevels_ReturnsFlattenedHierarchyClassStringWithFirstAndLastSegments()
        {
            //Given
            var expected = "Test1|Test5: TestSubteam 8004|5"; //Only displays the first level, the last level, and subteam
            var subject = CreateTestHierarchyClassTree(5);

            //When
            var actual = subject.ToFlattenedHierarchyClassString();

            //Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFlattenedHierarchyClassString_HierarchyClassIsThreeLevels_ReturnsFlattenedHierarchyClassStringWithFirstAndLastSegments()
        {
            //Given
            var expected = "Test1|Test3|3";
            var subject = CreateTestHierarchyClassTree(3);

            //When
            var actual = subject.ToFlattenedHierarchyClassString();

            //Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFlattenedMerchandiseHierarchyClassString_IncludeSubTeamAndSubBrickCode_ReturnsFlattenedMerchandiseHierachyClassStringWithSubTeamAndSubBrickCode()
        {
            // Given
            var subject = CreateTestHierarchyClassTree(5);
            var expected = "Test1|Test5: TestSubteam 8004|99999999|5";

            // When
            var actual = subject.ToFlattenedMerchandiseHierarchyClassString(includeSubTeam: true, includeSubBrickCode: true);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFlattenedMerchandiseHierarchyClassString_DoNotIncludeSubTeamAndSubBrickCode_ReturnsFlattenedMerchandiseHierachyClassStringWithoutSubTeamAndSubBrickCode()
        {
            // Given
            var subject = CreateTestHierarchyClassTree(5);
            var expected = "Test1|Test5|5";

            // When
            var actual = subject.ToFlattenedMerchandiseHierarchyClassString(includeSubTeam: false, includeSubBrickCode: false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFlattenedMerchandiseHierarchyClassString_IncludeOnlySubTeam_ReturnsFlattenedMerchandiseHierachyClassStringWithOnlySubTeam()
        {
            // Given
            var subject = CreateTestHierarchyClassTree(5);
            var expected = "Test1|Test5: TestSubteam 8004|5";

            // When
            var actual = subject.ToFlattenedMerchandiseHierarchyClassString(includeSubTeam: true, includeSubBrickCode: false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFlattenedMerchandiseHierarchyClassString_IncludeOnlySubBrickCode_ReturnsFlattenedMerchandiseHierachyClassStringWithOnlySubBrickCode()
        {
            // Given
            var subject = CreateTestHierarchyClassTree(5);
            var expected = "Test1|Test5|99999999|5";

            // When
            var actual = subject.ToFlattenedMerchandiseHierarchyClassString(includeSubTeam: false, includeSubBrickCode: true);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HierarchyClassForCombo_OneHierarchy_ReturnsHierarchyClassModelWithHierarchyClassLineage()
        {
            // Given
            var subject = CreateTestHierarchy(4);
            var expected = "Test1|Test4|4"; //Only displays the first level, the last level, and HierarchyClassId

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassLineage);
        }

        [TestMethod]
        public void HierarchyClassForCombo_OneHierarchy_ReturnsHierarchyClassNameWithoutId()
        {
            // Given
            var subject = CreateTestHierarchy(1);
            var expected = "Test1";

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassName);
        }

        [TestMethod]
        public void HierarchyClassForCombo_OneHierarchy_ReturnsHierarchyClassId()
        {
            // Given
            var subject = CreateTestHierarchy();
            var expected = 1;

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassId);
        }

        [TestMethod]
        public void HierarchyClassForCombo_HierarchyClassList_ReturnsHierarchyClassModelWithHierarchyClassLineage()
        {
            // Given
            var subject = CreateTestHierarchy(3).HierarchyClass;
            var expected = "Test1|Test3|3"; //Only displays the first level, the last level, and HierarchyClassId

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassLineage);
        }

        [TestMethod]
        public void HierarchyClassForCombo_HierarchyClassList_ReturnsHierarchyClassId()
        {
            // Given
            var subject = CreateTestHierarchy(3).HierarchyClass;
            var expected = 3;

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassId);
        }

        [TestMethod]
        public void HierarchyClassForCombo_HierarchyClassList_ReturnsHierarchyClassNameAsLineageWithoutId()
        {
            // Given
            var subject = CreateTestHierarchy(3).HierarchyClass;
            var expected = "Test1|Test3"; //Only displays the first level and the last level

            // When
            var actual = subject.HierarchyClassForCombo();

            // Then
            Assert.AreEqual(expected, actual[0].HierarchyClassName);
        }

        [TestMethod]
        public void ExcludeAffinitySubBricks_AllMerchandiseHierarchy_AffinitySubBricksNotInList()
        {
            // Given
            Hierarchy hierarchy = new Hierarchy { hierarchyID = 1, hierarchyName = "Test Hierarchy" };
            hierarchy.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(1).Build());
            hierarchy.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(2).Build());
            hierarchy.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(3).Build());
            hierarchy.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(4).Build());

            List<HierarchyClassTrait> hierarchyTraits = new List<HierarchyClassTrait>();
            hierarchyTraits.Add(new TestHierarchyClassTraitBuilder().WithHierarchyClassId(4).WithTraitId(Traits.Affinity).WithTraitValue("0").Build());
            hierarchy.HierarchyClass.First(hc => hc.hierarchyClassID == 4).HierarchyClassTrait = hierarchyTraits;

            // When
            List<HierarchyClass> hierarchyClasses = hierarchy.ExcludeAffinitySubBricks().ToList();

            // Then
            IEnumerable<HierarchyClass> onlyAffinitySubBricks = hierarchy.HierarchyClass
                .Where(hc => hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity && hct.traitValue == "0"));

            foreach (var subBrick in onlyAffinitySubBricks)
            {
                Assert.IsTrue(!hierarchyClasses.Contains(subBrick));
            }
        }

        [TestMethod]
        public void ExcludeAffinitySubBricks_HierarchyClassList_AffinitySubBricksNotInList()
        {
            // Given
            Hierarchy subject = new Hierarchy { hierarchyID = 1, hierarchyName = "Test Hierarchy" };
            subject.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(1).Build());
            subject.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(2).Build());
            subject.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(3).Build());
            subject.HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(4).Build());

            List<HierarchyClassTrait> hierarchyTraits = new List<HierarchyClassTrait>();
            hierarchyTraits.Add(new TestHierarchyClassTraitBuilder().WithHierarchyClassId(4).WithTraitId(Traits.Affinity).WithTraitValue("0").Build());
            subject.HierarchyClass.First(hc => hc.hierarchyClassID == 4).HierarchyClassTrait = hierarchyTraits;

            // When
            List<HierarchyClass> hierarchyClasses = subject.HierarchyClass.ExcludeAffinitySubBricks().ToList();

            // Then
            IEnumerable<HierarchyClass> onlyAffinitySubBricks = subject.HierarchyClass
                .Where(hc => hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity && hct.traitValue == "0"));

            foreach (var subBrick in onlyAffinitySubBricks)
            {
                Assert.IsTrue(!hierarchyClasses.Contains(subBrick));
            }
        }

        private HierarchyClass CreateTestHierarchyClassTree(int levels = 1)
        {

            Hierarchy hierarchy = new Hierarchy { hierarchyID = Hierarchies.Merchandise, hierarchyName = HierarchyNames.Merchandise };
            HierarchyClass leaf = null;
            HierarchyClass parentHierarchyClass = null;
            int? parentId = null;
            for (int i = 1; i <= levels; i++)
            {
                if (i < 5)
                {
                    leaf = new TestHierarchyClassBuilder().WithHierarchyClassName("Test" + i.ToString())
                        .WithHierarchyId(hierarchy.hierarchyID).WithHierarchyParentClassId(parentId).WithHierarchyLevel(i)
                        .WithHierarchyClassId(i)
                        .Build();
                    leaf.HierarchyClass2 = parentHierarchyClass;
                    leaf.Hierarchy = hierarchy;
                }
                else
                {
                    leaf = new TestHierarchyClassBuilder().WithHierarchyClassName("Test" + i.ToString())
                        .WithHierarchyId(hierarchy.hierarchyID).WithHierarchyParentClassId(parentId).WithHierarchyLevel(i)
                        .WithMerchFinMapping("TestSubteam (8004)").WithSubBrickCode("99999999").WithHierarchyClassId(1234)
                        .WithHierarchyClassId(i)
                        .Build();
                    leaf.HierarchyClass2 = parentHierarchyClass;
                    leaf.Hierarchy = hierarchy;
                    Trait merchFinMapping = new Trait { traitCode = TraitCodes.MerchFinMapping, traitDesc = TraitDescriptions.MerchFinMapping, traitID = Traits.MerchFinMapping };
                    Trait subBrickCode = new Trait { traitCode = TraitCodes.SubBrickCode, traitDesc = TraitDescriptions.SubBrickCode, traitID = Traits.SubBrickCode };
                    leaf.HierarchyClassTrait.First(hct => hct.traitID == Traits.MerchFinMapping).Trait = merchFinMapping;
                    leaf.HierarchyClassTrait.First(hct => hct.traitID == Traits.SubBrickCode).Trait = subBrickCode;
                    
                }

                parentId = leaf.hierarchyClassID;
                parentHierarchyClass = leaf;
            }

            return leaf;
        }

        private Hierarchy CreateTestHierarchy(int hierarchyClassLevels = 1)
        {
            var hierarchy = new Hierarchy
            {
                hierarchyID = 11,
                hierarchyName = "HierarchyForCombo"
            };

            var hierarchyClass = CreateTestHierarchyClassTree(hierarchyClassLevels);
            hierarchy.HierarchyClass.Add(hierarchyClass);

            return hierarchy;
        }
    }
}
