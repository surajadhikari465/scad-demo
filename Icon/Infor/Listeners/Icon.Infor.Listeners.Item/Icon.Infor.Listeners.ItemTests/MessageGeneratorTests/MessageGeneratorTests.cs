using Esb.Core.Serializer;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.Item.Tests.MessageGeneratorTests
{
    //[Ignore] //Use these tests to create test messages. Removed from normal test plan
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

        [Ignore] //Used to create test messages for Infor
        [TestMethod]
        public void BuildItemMessages()
        {
            //Build the items
            Contracts.items esbItems = new Contracts.items
            {
                item = context.Item
                    .Include(i => i.ItemTrait)
                    .Include(i => i.ItemHierarchyClass)
                    .Where(i => i.ScanCode.FirstOrDefault().scanCode.StartsWith("4"))
                    .Take(10)
                    .ToList()
                    .Select(i => CreateEsbItemMessageFromDbItem(i))
                    .ToArray()
            };

            //Serialize the items
            Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();
            var message = serializer.Serialize(esbItems, new Utf8StringWriter());

            //Save items to a file to use
            if (!Directory.Exists("GeneratedMessages"))
            {
                Directory.CreateDirectory("GeneratedMessages");
            }
            if(File.Exists(@"GeneratedMessages\ProductMessage.xml"))
            {
                File.Delete(@"GeneratedMessages\ProductMessage.xml");
            }
            var xmlMessage = XDocument.Parse(message);
            xmlMessage.Save(@"GeneratedMessages\ProductMessage.xml");
        }

        private Esb.Schemas.Wfm.Contracts.ItemType CreateEsbItemMessageFromDbItem(Framework.Item item)
        {
            return new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = item.itemID,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = ItemTypes.Codes.RetailSale,
                        description = ItemTypes.Descriptions.RetailSale
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = "1",
                        name = "Whole Foods Market",
                        type = new Contracts.LocaleTypeType
                        {
                            code = Contracts.LocaleCodeType.CHN,
                            description = Contracts.LocaleDescType.Chain
                        },
                        Item = new Contracts.EnterpriseItemAttributesType
                        {
                            scanCodes = new Contracts.ScanCodeType[]
                            {
                                new Contracts.ScanCodeType
                                {
                                    code = item.ScanCode.First().scanCode
                                }
                            },
                            hierarchies = new Contracts.HierarchyType[]
                            {
                                CreateHierarchy(Hierarchies.Names.Merchandise, GetHierarchyClass(item, Hierarchies.Merchandise)),
                                CreateHierarchy(Hierarchies.Names.Brands, GetHierarchyClass(item, Hierarchies.Brands)),
                                CreateTaxHierarchy(GetHierarchyClass(item, Hierarchies.Tax)),
                                CreateFinancialHierarchy(GetFinancialHierarchyClass(item)),
                                CreateHierarchy(Hierarchies.Names.National, GetHierarchyClass(item, Hierarchies.National))
                            },
                            traits = new Contracts.TraitType[]
                            {
                                CreateTrait(Traits.ProductDescription, item),
                                CreateTrait(Traits.PosDescription, item),
                                CreateTrait(Traits.FoodStampEligible, item),
                                CreateTrait(Traits.PosScaleTare, item),
                                CreateTrait(Traits.ProhibitDiscount, item),
                                CreateTrait(Traits.PackageUnit, item),
                                CreateTrait(Traits.RetailSize, item),
                                CreateTrait(Traits.RetailUom, item),
                                CreateTrait(Traits.AnimalWelfareRating, item),
                                CreateTrait(Traits.Biodynamic, item),
                                CreateTrait(Traits.CheeseMilkType, item),
                                CreateTrait(Traits.CheeseRaw, item),
                                CreateTrait(Traits.EcoScaleRating, item),
                                CreateTrait(Traits.GlutenFree, item),
                                CreateTrait(Traits.Kosher, item),
                                CreateTrait(Traits.Msc, item),
                                CreateTrait(Traits.NonGmo, item),
                                CreateTrait(Traits.Organic, item),
                                CreateTrait(Traits.PremiumBodyCare, item),
                                CreateTrait(Traits.FreshOrFrozen, item),
                                CreateTrait(Traits.SeafoodCatchType, item),
                                CreateTrait(Traits.Vegan, item),
                                CreateTrait(Traits.Vegetarian, item),
                                CreateTrait(Traits.WholeTrade, item),
                                CreateTrait(Traits.GrassFed, item),
                                CreateTrait(Traits.PastureRaised, item),
                                CreateTrait(Traits.FreeRange, item),
                                CreateTrait(Traits.DryAged, item),
                                CreateTrait(Traits.AirChilled, item),
                                CreateTrait(Traits.MadeInHouse, item),
                                CreateTrait(Traits.AlcoholByVolume, item),
                                CreateTrait(Traits.CaseinFree, item),
                                CreateTrait(Traits.DrainedWeight, item),
                                CreateTrait(Traits.DrainedWeightUom, item),
                                CreateTrait(Traits.FairTradeCertified, item),
                                CreateTrait(Traits.Hemp, item),
                                CreateTrait(Traits.LocalLoanProducer, item),
                                CreateTrait(Traits.MainProductName, item),
                                CreateTrait(Traits.NutritionRequired, item),
                                CreateTrait(Traits.OrganicPersonalCare, item),
                                CreateTrait(Traits.Paleo, item),
                                CreateTrait(Traits.ProductFlavorType, item),
                                CreateTrait(Traits.InsertDate, item),
                                CreateTrait(Traits.ModifiedDate, item),
                                CreateTrait(Traits.ModifiedUser, item),
                                CreateTrait(Traits.HiddenItem, item),
                                CreateTrait(Traits.Notes, item),
                                CreateTrait(Traits.DeliverySystem, item),
                            }
                        }
                    }
                }
            };
        }

        private Contracts.HierarchyType CreateFinancialHierarchy(HierarchyClass hierarchyClass)
        {
            var id = hierarchyClass.hierarchyClassName.Split('(')[1].Substring(0, 4);
            return new Contracts.HierarchyType
            {
                name = Hierarchies.Names.Financial,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = id,
                        name = hierarchyClass.hierarchyClassName,
                        parentId = new Contracts.hierarchyParentClassType { Value = 0 }
                    }
                }
            };
        }

        private Contracts.HierarchyType CreateTaxHierarchy(HierarchyClass hierarchyClass)
        {
            return new Contracts.HierarchyType
            {
                name = Hierarchies.Names.Tax,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = hierarchyClass.hierarchyClassName.Substring(0, 7),
                        name = hierarchyClass.hierarchyClassName,
                        parentId = new Contracts.hierarchyParentClassType { Value = 0 }
                    }
                }
            };
        }

        private Contracts.HierarchyType CreateHierarchy(string hierarchyName, HierarchyClass hierarchyClass)
        {
            var hierarchyParentClassID = hierarchyClass.hierarchyParentClassID.HasValue ? hierarchyClass.hierarchyParentClassID.Value : 0;
            return new Contracts.HierarchyType
            {
                name = hierarchyName,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = hierarchyClass.hierarchyClassID.ToString(),
                        name = hierarchyClass.hierarchyClassName,
                        parentId = new Contracts.hierarchyParentClassType { Value = hierarchyParentClassID }
                    }
                }
            };
        }

        private HierarchyClass GetHierarchyClass(Framework.Item i, int hierarchyId)
        {
            return i.ItemHierarchyClass.First(ihc => ihc.HierarchyClass.hierarchyID == hierarchyId).HierarchyClass;
        }

        private HierarchyClass GetFinancialHierarchyClass(Framework.Item i)
        {
            var merchandise = i.ItemHierarchyClass.First(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).HierarchyClass;
            var merchFinMappingTrait = merchandise.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.MerchFinMapping).traitValue;

            return context.HierarchyClass
                .FirstOrDefault(hc => hc.hierarchyID == Hierarchies.Financial && hc.hierarchyClassName.Contains(merchFinMappingTrait));
        }

        private Contracts.TraitType CreateTrait(int traitId, Framework.Item i)
        {
            var traitValue = i.ItemTrait.FirstOrDefault(it => it.traitID == traitId)?.traitValue;
            traitValue = traitValue == null ? string.Empty : traitValue;

            return new Contracts.TraitType
            {
                code = Traits.Codes.AsDictionary[traitId],
                type = new Contracts.TraitTypeType
                {
                    description = Traits.Descriptions.AsDictionary[traitId],
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType { value = traitValue }
                    }
                }
            };
        }
    }
}
