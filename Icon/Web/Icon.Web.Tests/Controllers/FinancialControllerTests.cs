using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Queries;
using Icon.Framework;
using System.Collections.Generic;
using Moq;
using Icon.Web.Mvc.Models;
using System.Web.Mvc;
using System.Linq;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class FinancialControllerTests
    {
        private FinancialController controller;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> hierarchyQuery;

        [TestInitialize]
        public void InitializeData()
        {
            this.hierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            this.controller = new FinancialController(this.hierarchyQuery.Object);
        }

        [TestMethod]
        public void Index_NoParameters_ReturnsViewModelResults()
        {
            // Given
            List<Hierarchy> hierarchies = GetFakeFinancialHierarchy();
            hierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchies);

            FinancialGridViewModel expectedViewModel = new FinancialGridViewModel
            {
                SubTeams = hierarchies.Single().HierarchyClass
                    .Select(hc => new FinancialViewModel
                    {
                        HierarchyId = hc.hierarchyID,
                        HierarchyName = hc.Hierarchy.hierarchyName,
                        HierarchyClassId = hc.hierarchyClassID,
                        SubTeamName = hc.hierarchyClassName.Split('(')[0].Trim(),
                        PeopleSoftNumber = hc.hierarchyClassName.Split('(')[1].Trim(')'),
                        PosDeptNumber = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.PosDepartmentNumber).Select(hct => hct.traitValue).SingleOrDefault(),
                        TeamNumber = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TeamNumber).Select(hct => hct.traitValue).SingleOrDefault(),
                        TeamName = hc.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TeamName).Select(hct => hct.traitValue).SingleOrDefault()
                    })
                    .OrderBy(hc => hc.PeopleSoftNumber)
            };

            // When
            var result = controller.Index() as ViewResult;
            var actualViewModel = result.Model as FinancialGridViewModel;

            // Then
            Assert.AreEqual(expectedViewModel.SubTeams.Count(), actualViewModel.SubTeams.Count());
            for (int i = 0; i < actualViewModel.SubTeams.Count(); i++)
            {
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].HierarchyClassId, actualViewModel.SubTeams.ToList()[i].HierarchyClassId);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].HierarchyId, actualViewModel.SubTeams.ToList()[i].HierarchyId);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].HierarchyName, actualViewModel.SubTeams.ToList()[i].HierarchyName);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].SubTeamName, actualViewModel.SubTeams.ToList()[i].SubTeamName);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].PeopleSoftNumber, actualViewModel.SubTeams.ToList()[i].PeopleSoftNumber);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].PosDeptNumber, actualViewModel.SubTeams.ToList()[i].PosDeptNumber);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].TeamName, actualViewModel.SubTeams.ToList()[i].TeamName);
                Assert.AreEqual(expectedViewModel.SubTeams.ToList()[i].TeamNumber, actualViewModel.SubTeams.ToList()[i].TeamNumber);
            }
        }

        private List<Hierarchy> GetFakeFinancialHierarchy()
        {
            List<Hierarchy> hierarchies = new List<Hierarchy>();

            Hierarchy financialHierarchy = new Hierarchy
            {
                hierarchyID = 1,
                hierarchyName = "Financial",
                HierarchyPrototype = GetFakeHierarchyPrototypeList(),
                HierarchyClass = GetFakeHierarchyClassList()
            };

            hierarchies.Add(financialHierarchy);
            return hierarchies;
        }

        private ICollection<HierarchyClass> GetFakeHierarchyClassList()
        {
            List<HierarchyClass> hierarchyClasses = new List<HierarchyClass>();

            HierarchyClass subTeam1 = new HierarchyClass
            {
                hierarchyClassID = 1,
                hierarchyClassName = "SubTeam1 (1000)",
                hierarchyID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = 1,
                HierarchyClassTrait = GetFakeHierarchyClassTraitsList(1),
                Hierarchy = new Hierarchy { hierarchyID = 1, hierarchyName = "Financial" }
            };

            HierarchyClass subTeam2 = new HierarchyClass
            {
                hierarchyClassID = 2,
                hierarchyClassName = "SubTeam2 (1100)",
                hierarchyID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = 1,
                HierarchyClassTrait = GetFakeHierarchyClassTraitsList(2),
                Hierarchy = new Hierarchy { hierarchyID = 1, hierarchyName = "Financial" }
            };

            hierarchyClasses.Add(subTeam1);
            hierarchyClasses.Add(subTeam2);

            return hierarchyClasses;
        }

        private List<HierarchyPrototype> GetFakeHierarchyPrototypeList()
        {
            List<HierarchyPrototype> prototypes = new List<HierarchyPrototype>();
            HierarchyPrototype prototype1 = new HierarchyPrototype
            {
                hierarchyID = 1,
                hierarchyLevel = 1,
                hierarchyLevelName = "Team",
                itemsAttached = true
            };
            HierarchyPrototype prototype2 = new HierarchyPrototype
            {
                hierarchyID = 1,
                hierarchyLevel = 2,
                hierarchyLevelName = "SubTeam",
                itemsAttached = true
            };

            prototypes.Add(prototype1);
            prototypes.Add(prototype2);
            return prototypes;
        }

        private List<HierarchyClassTrait> GetFakeHierarchyClassTraitsList(int hierarchyClassId)
        {
            int posDeptTraitId = 1;
            int teamNameTraitId = 2;
            int teamNumberTraitId = 3;
            List<HierarchyClassTrait> hierarchyClassTraits = new List<HierarchyClassTrait>();

            // Fake PosDeptNumber, TeamName, TeamNumber traits
            HierarchyClassTrait hierarchyClassTrait_POSDept = new HierarchyClassTrait
            {
                hierarchyClassID = hierarchyClassId,
                traitID = posDeptTraitId,
                traitValue = hierarchyClassId.ToString(),
                Trait = new Trait()
                {
                    traitCode = TraitCodes.PosDepartmentNumber,
                    traitID = posDeptTraitId
                }
            };
            HierarchyClassTrait hierarchyClassTrait_TeamName = new HierarchyClassTrait
            {
                hierarchyClassID = hierarchyClassId,
                traitID = teamNameTraitId,
                traitValue = "Test Team Name",
                Trait = new Trait()
                {
                    traitCode = TraitCodes.TeamName,
                    traitID = teamNameTraitId
                }
            };
            HierarchyClassTrait hierarchyClassTrait_TeamNumber = new HierarchyClassTrait
            {
                hierarchyClassID = hierarchyClassId,
                traitID = teamNumberTraitId,
                traitValue = "123",
                Trait = new Trait()
                {
                    traitCode = TraitCodes.TeamNumber,
                    traitID = teamNumberTraitId
                }
            };

            hierarchyClassTraits.Add(hierarchyClassTrait_POSDept);
            hierarchyClassTraits.Add(hierarchyClassTrait_TeamName);
            hierarchyClassTraits.Add(hierarchyClassTrait_TeamNumber);

            return hierarchyClassTraits;
        }
    }
}
