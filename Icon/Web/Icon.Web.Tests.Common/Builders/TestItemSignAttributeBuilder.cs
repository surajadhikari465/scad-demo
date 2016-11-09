using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestItemSignAttributeBuilder
    {
        private int itemID;
        private int? animalWelfareRatingId;
        private bool biodynamic;
        private int? cheeseMilkTypeId;
        private bool cheeseRaw;
        private int? ecoScaleRatingId;
        private int? glutenFreeAgencyId;
        private int? healthyEatingRatingId;
        private int? kosherAgencyId;
        private int? nonGmoAgencyId;
        private int? organicAgencyId;
        private bool premiumBodyCare;
        private int? seafoodFreshOrFrozenId;
        private int? seafoodWildOrFarmRaisedId;
        private int? veganAgencyId;
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
            animalWelfareRatingId = null;
            biodynamic = false;
            cheeseMilkTypeId = null;
            cheeseRaw = false;
            ecoScaleRatingId = null;
            glutenFreeAgencyId = null;
            healthyEatingRatingId = null;
            kosherAgencyId = null;
            nonGmoAgencyId = null;
            organicAgencyId = null;
            premiumBodyCare = false;
            seafoodFreshOrFrozenId = null;
            seafoodWildOrFarmRaisedId = null;
            veganAgencyId = null;
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

        public TestItemSignAttributeBuilder WithAnimalWelfareRatingId(int? animalWelfareRatingId)
        {
            this.animalWelfareRatingId = animalWelfareRatingId;
            return this;
        }

        public TestItemSignAttributeBuilder WithCheeseRaw(bool cheeseRaw)
        {
            this.cheeseRaw = cheeseRaw;
            return this;
        }
        public TestItemSignAttributeBuilder WithMilkTypeId(int? milkTypeId)
        {
            this.cheeseMilkTypeId = milkTypeId;
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
        public TestItemSignAttributeBuilder WithEcoScaleRatingId(int? ecoScaleRatingId)
        {
            this.ecoScaleRatingId = ecoScaleRatingId;
            return this;
        }

        public TestItemSignAttributeBuilder WithOrganicAgencyId(int? organicAgencyId)
        {
            this.organicAgencyId = organicAgencyId;
            return this;
        }
        public TestItemSignAttributeBuilder WithGlutenFreeAgencyId(int? glutenFreeAgencyId)
        {
            this.glutenFreeAgencyId = glutenFreeAgencyId;
            return this;
        }
        public TestItemSignAttributeBuilder WithKosherAgencyId(int? kosherAgencyId)
        {
            this.kosherAgencyId = kosherAgencyId;
            return this;
        }

        public TestItemSignAttributeBuilder WithNonGmoAgencyId(int? nonGmoAgencyId)
        {
            this.nonGmoAgencyId = nonGmoAgencyId;
            return this;
        }
        public TestItemSignAttributeBuilder WithHealthyEatingRatingId(int? healthyEatingRatingId)
        {
            this.healthyEatingRatingId = healthyEatingRatingId;
            return this;
        }
        public TestItemSignAttributeBuilder WithSeafoodFreshOrFrozenId(int? seafoodFreshOrFrozenId)
        {
            this.seafoodFreshOrFrozenId = seafoodFreshOrFrozenId;
            return this;
        }
        public TestItemSignAttributeBuilder WithCheeseMilkTypeId(int? cheeseMilkTypeId)
        {
            this.cheeseMilkTypeId = cheeseMilkTypeId;
            return this;
        }
        public TestItemSignAttributeBuilder WithSeafoodCatchTypeId(int? seafoodCatchTypeId)
        {
            this.seafoodWildOrFarmRaisedId = seafoodCatchTypeId;
            return this;
        }

        public TestItemSignAttributeBuilder WithVeganAgencyId(int? veganAgencyId)
        {
            this.veganAgencyId = veganAgencyId;
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
                AnimalWelfareRatingId = animalWelfareRatingId,
                Biodynamic = biodynamic,
                CheeseMilkTypeId = cheeseMilkTypeId,
                CheeseRaw = cheeseRaw,
                EcoScaleRatingId = ecoScaleRatingId,
                GlutenFreeAgencyId = glutenFreeAgencyId,
                KosherAgencyId = kosherAgencyId,
                NonGmoAgencyId = nonGmoAgencyId,
                OrganicAgencyId = organicAgencyId,
                PremiumBodyCare = premiumBodyCare,
                SeafoodFreshOrFrozenId = seafoodFreshOrFrozenId,
                SeafoodCatchTypeId = seafoodWildOrFarmRaisedId,
                VeganAgencyId = veganAgencyId,
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

