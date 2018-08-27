using Icon.Framework;
using System;
using System.Collections.Generic;

namespace Icon.Testing.Builders
{
    public class TestHierarchyClassBuilder
    {
        private int hierarchyClassId;
        private string hierarchyClassName;
        private int? hierarchyLevel;
        private int hierarchyId;
        private int? hierarchyParentClassId;
        private string merchFinMapping;
        private string taxAbbreviation;
        private string nonMerchandise;
        private string glAccount;
        private string sentToEsb;
        private string financialHierarchyCode;
        private string subBrickCode;
        private string affinityTrait;
        private string posDeptNumberTrait;
        private string teamNumberTrait;
        private string teamNameTrait;
        private string disableEventGenerationTrait;
        private string brandAbbreviation;
        private string glutenFreeTrait;
        private string kosherTrait;
        private string nonGmoTrait;
        private string organicTrait;
        private string prohibitDiscountTrait;
        private string veganTrait;
        private string nationalClassCode;
        private List<HierarchyClassTrait> hierarchyTraits;
        private ICollection<ItemSignAttribute> itemSignAttributes;

        public TestHierarchyClassBuilder()
        {
            this.hierarchyClassId = 0;
            this.hierarchyClassName = "Unit Test Hierarchy Class Level 1";
            this.hierarchyId = Hierarchies.Merchandise;
            this.hierarchyLevel = 1;
            this.hierarchyParentClassId = null;
            this.merchFinMapping = null;
            this.taxAbbreviation = null;
            this.glAccount = null;
            this.nonMerchandise = null;
            this.sentToEsb = null;
            this.financialHierarchyCode = null;
            this.subBrickCode = null;
            this.affinityTrait = null;
            this.posDeptNumberTrait = null;
            this.teamNumberTrait = null;
            this.teamNameTrait = null;
            this.disableEventGenerationTrait = null;
            this.brandAbbreviation = null;
            this.glutenFreeTrait = null;
            this.kosherTrait = null;
            this.nonGmoTrait = null;
            this.organicTrait = null;
            this.prohibitDiscountTrait = null;
            this.veganTrait = null;
            hierarchyTraits = new List<HierarchyClassTrait>();
            itemSignAttributes = new List<ItemSignAttribute>();
        }

        /// <summary>
        /// Populate the HierarchyClass object with your own Id.
        /// If adding the HierarchyClass to the IconContext, the SaveChanges() will fail.
        /// </summary>
        /// <param name="hierarchyClassId">hierarchyClassID</param>
        /// <returns></returns>
        public TestHierarchyClassBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierarchyClassId = hierarchyClassId;
            return this;
        }

        public TestHierarchyClassBuilder WithHierarchyClassName(string hierarchyClassName)
        {
            this.hierarchyClassName = hierarchyClassName;
            return this;
        }

        public TestHierarchyClassBuilder WithHierarchyLevel(int? hierarchyLevel)
        {
            this.hierarchyLevel = hierarchyLevel;
            return this;
        }

        public TestHierarchyClassBuilder WithHierarchyParentClassId(int? hierarchyParentClassId)
        {
            this.hierarchyParentClassId = hierarchyParentClassId;
            return this;
        }

        public TestHierarchyClassBuilder WithHierarchyId(int hierarchyId)
        {
            this.hierarchyId = hierarchyId;
            return this;
        }

        public TestHierarchyClassBuilder WithMerchFinMapping(string merchFinMapping)
        {
            this.merchFinMapping = merchFinMapping;
            return this;
        }

        public TestHierarchyClassBuilder WithTaxAbbreviationTrait(string taxAbbreviation)
        {
            this.taxAbbreviation = taxAbbreviation;
            return this;
        }
        public TestHierarchyClassBuilder WithNationalClassCodeTrait(string nationalClassCode)
        {
            this.nationalClassCode = nationalClassCode;
            return this;
        }

        public TestHierarchyClassBuilder WithGlAccountTrait(string glAccount)
        {
            this.glAccount = glAccount;
            return this;
        }

        public TestHierarchyClassBuilder WithNonMerchandiseTrait(string nonMerchandiseTrait)
        {
            this.nonMerchandise = nonMerchandiseTrait;
            return this;
        }

        public TestHierarchyClassBuilder WithMerchFinMappingTrait(string merchFinMapping)
        {
            this.merchFinMapping = merchFinMapping;
            return this;
        }

        public TestHierarchyClassBuilder WithSentToEsbTrait(DateTime sentToEsb)
        {
            this.sentToEsb = sentToEsb.ToString();
            return this;
        }

        public TestHierarchyClassBuilder WithFinancialCodeTrait(string finCode)
        {
            this.financialHierarchyCode = finCode;
            return this;
        }

        public TestHierarchyClassBuilder WithSubBrickCode(string subBrickCode)
        {
            this.subBrickCode = subBrickCode;
            return this;
        }

        public TestHierarchyClassBuilder WithAffinityTrait(string affinityTrait)
        {
            this.affinityTrait = affinityTrait;
            return this;
        }

        public TestHierarchyClassBuilder WithPosDeptNumberTrait(string posDeptNumber)
        {
            this.posDeptNumberTrait = posDeptNumber;
            return this;
        }

        public TestHierarchyClassBuilder WithTeamNumberTrait(string teamNumber)
        {
            this.teamNumberTrait = teamNumber;
            return this;
        }

        public TestHierarchyClassBuilder WithTeamNameTrait(string teamName)
        {
            this.teamNameTrait = teamName;
            return this;
        }

        public TestHierarchyClassBuilder WithBrandAbbreviation(string brandAbbreviation)
        {
            this.brandAbbreviation = brandAbbreviation;
            return this;
        }

        public TestHierarchyClassBuilder WithDisableEventGenerationTrait(string disableEventGenerationTraitValue)
        {
            this.disableEventGenerationTrait = disableEventGenerationTraitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithGlutenFreeTrait(string traitValue)
        {
            this.glutenFreeTrait = traitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithKosherTrait(string traitValue)
        {
            this.kosherTrait = traitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithNonGmoTrait(string traitValue)
        {
            this.nonGmoTrait = traitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithOrganicTrait(string traitValue)
        {
            this.organicTrait = traitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithProhibitDiscountTrait(string traitValue)
        {
            this.prohibitDiscountTrait = traitValue;
            return this;
        }

        public TestHierarchyClassBuilder WithVeganTrait(string traitValue)
        {
            this.veganTrait = traitValue;
            return this;
        }

        public HierarchyClass Build()
        {
            HierarchyClass hierarchyClass = new HierarchyClass();
            hierarchyClass.hierarchyClassID = this.hierarchyClassId;
            hierarchyClass.hierarchyClassName = this.hierarchyClassName;
            hierarchyClass.hierarchyID = this.hierarchyId;
            hierarchyClass.hierarchyLevel = this.hierarchyLevel;
            hierarchyClass.hierarchyParentClassID = this.hierarchyParentClassId;

            AddHierarchyClassTrait(hierarchyClass, Traits.TaxAbbreviation, this.taxAbbreviation);
            AddHierarchyClassTrait(hierarchyClass, Traits.GlAccount, this.glAccount);
            AddHierarchyClassTrait(hierarchyClass, Traits.NonMerchandise, this.nonMerchandise);
            AddHierarchyClassTrait(hierarchyClass, Traits.MerchFinMapping, this.merchFinMapping);
            AddHierarchyClassTrait(hierarchyClass, Traits.SentToEsb, this.sentToEsb);
            AddHierarchyClassTrait(hierarchyClass, Traits.FinancialHierarchyCode, this.financialHierarchyCode);
            AddHierarchyClassTrait(hierarchyClass, Traits.SubBrickCode, this.subBrickCode);
            AddHierarchyClassTrait(hierarchyClass, Traits.Affinity, this.affinityTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.PosDepartmentNumber, this.posDeptNumberTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.TeamNumber, this.teamNumberTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.TeamName, this.teamNameTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.TeamName, this.disableEventGenerationTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.BrandAbbreviation, this.brandAbbreviation);
            AddHierarchyClassTrait(hierarchyClass, Traits.GlutenFree, this.glutenFreeTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.Kosher, this.kosherTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.NonGmo, this.nonGmoTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.Organic, this.organicTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.ProhibitDiscount, this.prohibitDiscountTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.Vegan, this.veganTrait);
            AddHierarchyClassTrait(hierarchyClass, Traits.NationalClassCode, this.nationalClassCode);

            hierarchyClass.HierarchyClassTrait = hierarchyTraits;

            return hierarchyClass;
        }

        private void AddHierarchyClassTrait(HierarchyClass hierarchyClass, int traitId, string traitValue)
        {
            if (!String.IsNullOrWhiteSpace(traitValue))
            {
                HierarchyClassTrait trait =
                    new HierarchyClassTrait
                    {
                        hierarchyClassID = hierarchyClass.hierarchyClassID,
                        traitID = traitId,
                        traitValue = traitValue
                    };
                hierarchyTraits.Add(trait);
            }
        }

        public static implicit operator HierarchyClass(TestHierarchyClassBuilder builder)
        {
            return builder.Build();
        }
    }
}
