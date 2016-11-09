using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Data.Entity;
using Esb.Core.Serializer;
using System.IO;
using System.Xml.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.MessageGeneratorTests
{
    [TestClass]
    public class MessageGeneratorTests
    {
        private IconContext context;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void BuildHierarchyMessages()
        {
            CreateHierarchyMessage(Hierarchies.Brands, HierarchyLevelNames.Brand, HierarchyLevels.Brand, "BrandMessage", new List<int> { Traits.BrandAbbreviation });
            CreateHierarchyMessage(Hierarchies.Tax, HierarchyLevelNames.Tax, HierarchyLevels.Tax, "TaxMessage", new List<int> { Traits.TaxAbbreviation, Traits.TaxRomance });
            CreateHierarchyMessage(Hierarchies.Merchandise, HierarchyLevelNames.SubBrick, HierarchyLevels.SubBrick, "MerchandiseMessage", new List<int> { Traits.SubBrickCode });
            CreateHierarchyMessage(Hierarchies.National, HierarchyLevelNames.NationalClass, HierarchyLevels.NationalClass, "NationalMessage", new List<int> { Traits.NationalClassCode });
        }

        private void CreateHierarchyMessage(int hierarchyId, string levelName, int level, string fileName, List<int> traitIds)
        {
            //Build the hierarchies
            Contracts.HierarchyType hierarchy = new Contracts.HierarchyType
                {
                    name = Hierarchies.Names.AsDictionary[hierarchyId],
                    prototype = new Contracts.HierarchyPrototypeType { hierarchyLevelName = levelName, itemsAttached = "1" },
                    @class = context.HierarchyClass
                        .Include(hc => hc.HierarchyClassTrait)
                        .Where(hc => hc.hierarchyID == hierarchyId && hc.hierarchyLevel == level)
                        .Take(10)
                        .ToList()
                        .Select(hc => CreateHierarchyClassContract(hc, traitIds))
                        .ToArray()
                };               

            //Serialize the items
            Serializer<Contracts.HierarchyType> serializer = new Serializer<Contracts.HierarchyType>();
            var message = serializer.Serialize(hierarchy, new Utf8StringWriter());

            //Save items to a file to use
            if (!Directory.Exists("GeneratedMessages"))
            {
                Directory.CreateDirectory("GeneratedMessages");
            }
            var xmlMessage = XDocument.Parse(message);
            xmlMessage.Save(@"GeneratedMessages\" + fileName + ".xml");
        }

        private static Contracts.HierarchyClassType CreateHierarchyClassContract(Framework.HierarchyClass hc, List<int> traitIds)
        {
            var parentId = hc.hierarchyParentClassID.HasValue ? hc.hierarchyParentClassID.Value : 0;

            return new Contracts.HierarchyClassType
            {
                id = hc.hierarchyClassID.ToString(),
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                name = hc.hierarchyClassName, 
                parentId = new Contracts.hierarchyParentClassType { Value = parentId },   
                traits = traitIds.Select(id => new Contracts.TraitType
                    {
                        code = Traits.Codes.AsDictionary[id],
                        type = new Contracts.TraitTypeType
                        {
                            description = Traits.Descriptions.AsDictionary[id],
                            value = new Contracts.TraitValueType[]
                                {
                                    new Contracts.TraitValueType
                                    {
                                        value = hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == id) == null 
                                            ? string.Empty
                                            : hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == id).traitValue
                                    }
                                }
                        }
                    })
                    .ToArray()
            };
        }
    }
}
