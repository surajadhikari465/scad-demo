using Icon.Framework;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestItemSignAttributeBuilder
    {
        private int itemID;
        private string animalWelfareRating;
        private bool biodynamic;
        private string cheeseMilkType;
        private bool cheeseRaw;
        private string ecoScaleRating;
        private string glutenFreeAgencyName;
        private int? healthyEatingRatingId;
        private string kosherAgencyName;
        private string nonGmoAgencyName;
        private string organicAgencyName;
        private bool premiumBodyCare;
        private string seafoodFreshOrFrozen;
        private string seafoodWildOrFarmRaised;
        private string veganAgencyName;
        private bool vegetarian;
        private bool wholeTrade;
        private bool grassFed;
        private bool pastureRaised;
        private bool freeRange;
        private bool dryAged;
        private bool airChilled;
        private bool madeInHouse;
        private bool msc;

        public TestItemSignAttributeBuilder()
        {
            itemID = 123456;
            animalWelfareRating = null;
            biodynamic = false;
            cheeseMilkType = null;
            cheeseRaw = false;
            ecoScaleRating = null;
            glutenFreeAgencyName = null;
            healthyEatingRatingId = null;
            kosherAgencyName = null;
            nonGmoAgencyName = null;
            organicAgencyName = null;
            premiumBodyCare = false;
            seafoodFreshOrFrozen = null;
            seafoodWildOrFarmRaised = null;
            veganAgencyName = null;
            vegetarian = false;
            wholeTrade = false;
            msc = false;
            grassFed = false;
            pastureRaised = false;
            freeRange = false;
            dryAged = false;
            airChilled = false;
            madeInHouse = false;
        }

        public TestItemSignAttributeBuilder WithItemId(int itemId)
        {
            this.itemID = itemId;
            return this;
        }

        public TestItemSignAttributeBuilder WithAnimalWelfareRating(string animalWelfareRating)
        {
            this.animalWelfareRating = animalWelfareRating;
            return this;
        }

        public TestItemSignAttributeBuilder WithCheeseRaw(bool cheeseRaw)
        {
            this.cheeseRaw = cheeseRaw;
            return this;
        }
        public TestItemSignAttributeBuilder WithMilkType(string milkType)
        {
            this.cheeseMilkType = milkType;
            return this;
        }
        public TestItemSignAttributeBuilder WithBiodynamic(bool biodynamic)
        {
            this.biodynamic = biodynamic;
            return this;
        }

        public TestItemSignAttributeBuilder WithPremiumBodyCare(bool premiumBodyCare)
        {
            this.premiumBodyCare = premiumBodyCare;
            return this;
        }

        public TestItemSignAttributeBuilder WithGrassFed(bool grassFed)
        {
            this.grassFed = grassFed;
            return this;
        }
        public TestItemSignAttributeBuilder WithEcoScaleRating(string ecoScaleRating)
        {
            this.ecoScaleRating = ecoScaleRating;
            return this;
        }

        public TestItemSignAttributeBuilder WithOrganicAgencyName(string organicAgencyName)
        {
            this.organicAgencyName = organicAgencyName;
            return this;
        }
        public TestItemSignAttributeBuilder WithGlutenFreeAgencyName(string glutenFreeAgencyName)
        {
            this.glutenFreeAgencyName = glutenFreeAgencyName;
            return this;
        }
        public TestItemSignAttributeBuilder WithKosherAgencyName(string kosherAgencyName)
        {
            this.kosherAgencyName = kosherAgencyName;
            return this;
        }

        public TestItemSignAttributeBuilder WithNonGmoAgencyName(string nonGmoAgencyName)
        {
            this.nonGmoAgencyName = nonGmoAgencyName;
            return this;
        }
        public TestItemSignAttributeBuilder WithHealthyEatingRatingId(int? healthyEatingRatingId)
        {
            this.healthyEatingRatingId = healthyEatingRatingId;
            return this;
        }
        public TestItemSignAttributeBuilder WithSeafoodFreshOrFrozen(string seafoodFreshOrFrozen)
        {
            this.seafoodFreshOrFrozen = seafoodFreshOrFrozen;
            return this;
        }
        public TestItemSignAttributeBuilder WithCheeseMilkType(string cheeseMilkType)
        {
            this.cheeseMilkType = cheeseMilkType;
            return this;
        }
        public TestItemSignAttributeBuilder WithSeafoodCatchType(string seafoodCatchType)
        {
            this.seafoodWildOrFarmRaised = seafoodCatchType;
            return this;
        }

        public TestItemSignAttributeBuilder WithVeganAgencyName(string veganAgencyName)
        {
            this.veganAgencyName = veganAgencyName;
            return this;
        }

        public TestItemSignAttributeBuilder WithVegetarian(bool vegetarian)
        {
            this.vegetarian = vegetarian;
            return this;
        }
        public TestItemSignAttributeBuilder WithWholeTrade(bool wholeTrade)
        {
            this.wholeTrade = wholeTrade;
            return this;
        }
       
        public TestItemSignAttributeBuilder WithPastureRaised(bool pastureRaised)
        {
            this.pastureRaised = pastureRaised;
            return this;
        }

        public TestItemSignAttributeBuilder WithFreeRange(bool freeRange)
        {
            this.freeRange = freeRange;
            return this;
        }

        public TestItemSignAttributeBuilder WithDryAged(bool dryAged)
        {
            this.dryAged = dryAged;
            return this;
        }

        public TestItemSignAttributeBuilder WithAirChilled(bool airChilled)
        {
            this.airChilled = airChilled;
            return this;
        }

        public TestItemSignAttributeBuilder WithMadeInHouse(bool madeInHouse)
        {
            this.madeInHouse = madeInHouse;
            return this;
        }

        public TestItemSignAttributeBuilder WithMsc(bool msc)
        {
            this.msc = msc;
            return this;
        }

        public ItemSignAttribute Build()
        {
            return new ItemSignAttribute
            {
                ItemID = itemID,
                AnimalWelfareRating = animalWelfareRating,
                Biodynamic = biodynamic,
                MilkType = cheeseMilkType,
                CheeseRaw = cheeseRaw,
                EcoScaleRating = ecoScaleRating,
                GlutenFreeAgencyName = glutenFreeAgencyName,
                KosherAgencyName = kosherAgencyName,
                NonGmoAgencyName = nonGmoAgencyName,
                OrganicAgencyName = organicAgencyName,
                PremiumBodyCare = premiumBodyCare,
                FreshOrFrozen = seafoodFreshOrFrozen,
                SeafoodCatchType = seafoodWildOrFarmRaised,
                VeganAgencyName = veganAgencyName,
                Vegetarian = vegetarian,
                WholeTrade = wholeTrade,
                GrassFed = grassFed,
                PastureRaised = pastureRaised,
                FreeRange = freeRange,
                DryAged = dryAged,
                AirChilled = airChilled,
                MadeInHouse = madeInHouse,
                Msc = msc
            };
        }

        public static implicit operator ItemSignAttribute(TestItemSignAttributeBuilder builder)
        {
            return builder.Build();
        }
    }
}

