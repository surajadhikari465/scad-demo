using Icon.Framework;
using System.Linq;

namespace Icon.Web.Tests.Common.Builders
{
    internal class TestItemTraitBuilder
    {
        private int traitId;
        private int itemId;
        private string traitValue;
        private int localeId;
        private string uomId;
        private Trait trait;
        private Item item;
        private Locale locale;
        private bool includeNavigationProperties;
        private IconContext context;

        internal TestItemTraitBuilder()
        {
            traitId = 1;
            itemId = 1;
            traitValue = "Test Trait Value";
            localeId = Locales.WholeFoods;
            trait = null;
            item = null;
            locale = null;
            context = null;
            uomId = null;
            includeNavigationProperties = false;
            context = null;
        }

        internal TestItemTraitBuilder WithTraitId(int traitId)
        {
            this.traitId = traitId;
            return this;
        }

        internal TestItemTraitBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemTraitBuilder WithLocaleId(int localeId)
        {
            this.localeId = localeId;
            return this;
        }

        internal TestItemTraitBuilder WithUomId(string uomId)
        {
            this.uomId = uomId;
            return this;
        }

        internal TestItemTraitBuilder WithTraitValue(string traitValue)
        {
            this.traitValue = traitValue;
            return this;
        }

        internal TestItemTraitBuilder WithTrait(Trait trait)
        {
            this.trait = trait;
            return this;
        }

        internal TestItemTraitBuilder WithItem(Item item)
        {
            this.item = item;
            return this;
        }

        internal TestItemTraitBuilder WithLocale(Locale locale)
        {
            this.locale = locale;
            return this;
        }

        internal TestItemTraitBuilder IncludeNavigationPropertiesFromContext(IconContext context)
        {
            this.includeNavigationProperties = true;
            this.context = context;
            return this;
        }

        private ItemTrait Build()
        {
            ItemTrait itemTrait = new ItemTrait
            {
                itemID = itemId,
                traitID = traitId,
                localeID = localeId,
                uomID = uomId,
                traitValue = traitValue,
                Item = item,
                Locale = locale,
                Trait = trait
            };

            if (includeNavigationProperties)
            {
                if (context == null)
                {
                    context = new IconContext();
                }
                if (item == null) 
                { 
                    itemTrait.Item = context.Item.FirstOrDefault(i => i.ItemId == itemId); 
                }
                if (locale == null) 
                {
                    itemTrait.Locale = context.Locale.FirstOrDefault(l => l.localeID == localeId);
                }
                if (trait == null) 
                {
                    itemTrait.Trait = context.Trait.FirstOrDefault(t => t.traitID == traitId);
                }
            }

            //Assign the foreign keys of the ItemTrait to the primary keys of the navigational properties
            if (item != null)
            {
                itemTrait.itemID = item.ItemId;
            }
            if (locale != null)
            {
                itemTrait.localeID = locale.localeID;
            }
            if (trait != null)
            {
                itemTrait.traitID = trait.traitID;
            }

            return itemTrait;
        }

        public static implicit operator ItemTrait(TestItemTraitBuilder builder)
        {
            return builder.Build();
        }
    }
}
