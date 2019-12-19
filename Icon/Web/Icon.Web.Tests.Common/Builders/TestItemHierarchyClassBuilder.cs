using Icon.Framework;
using System.Linq;

namespace Icon.Web.Tests.Common.Builders
{
    internal class TestItemHierarchyClassBuilder
    {
        private int hierarchyClassId;
        private int itemId;
        private int localeId;
        private Locale locale;
        private HierarchyClass hierarchyClass;
        private Item item;
        private bool includeNavigationProperties;
        private IconContext context;

        internal TestItemHierarchyClassBuilder()
        {
            hierarchyClassId = 1;
            itemId = 1;
            localeId = Locales.WholeFoods;
            includeNavigationProperties = false;
            context = null;
            locale = null;
            hierarchyClass = null;
            item = null;
        }

        internal TestItemHierarchyClassBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierarchyClassId = hierarchyClassId;
            return this;
        }

        internal TestItemHierarchyClassBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemHierarchyClassBuilder WithLocaleId(int localeId)
        {
            this.localeId = localeId;
            return this;
        }

        internal TestItemHierarchyClassBuilder WithLocale(Locale locale)
        {
            this.locale = locale;
            return this;
        }

        internal TestItemHierarchyClassBuilder WithHierarchyClass(HierarchyClass hierarchyClass)
        {
            this.hierarchyClass = hierarchyClass;
            return this;
        }

        internal TestItemHierarchyClassBuilder WithItem(Item item)
        {
            this.item = item;
            return this;
        }

        internal TestItemHierarchyClassBuilder IncludeNavigationPropertiesFromContext(IconContext context)
        {
            this.includeNavigationProperties = true;
            this.context = context;
            return this;
        }

        internal ItemHierarchyClass Build()
        {
            ItemHierarchyClass itemHierarchyClass = new ItemHierarchyClass
            {
                hierarchyClassID = this.hierarchyClassId,
                itemID = this.itemId,
                localeID = this.localeId,
                Item = item,
                HierarchyClass = hierarchyClass,
                Locale = locale
            };

            if(includeNavigationProperties)
            {
                if(context == null)
                {
                    context = new IconContext();
                }
                if(hierarchyClass == null)
                {
                    itemHierarchyClass.HierarchyClass = context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyClassID == hierarchyClassId);
                }
                if(item == null)
                {
                    itemHierarchyClass.Item = context.Item.FirstOrDefault(i => i.ItemId == itemId);
                }
                if(locale == null)
                {
                    itemHierarchyClass.Locale = context.Locale.FirstOrDefault(l => l.localeID == localeId);
                }
            }

            if (hierarchyClass != null)
            {
                itemHierarchyClass.hierarchyClassID = hierarchyClass.hierarchyClassID;
            }
            if (item != null)
            {
                itemHierarchyClass.itemID = item.ItemId;
            }
            if (locale != null)
            {
                itemHierarchyClass.localeID = locale.localeID;
            }

            return itemHierarchyClass;
        }

        public static implicit operator ItemHierarchyClass(TestItemHierarchyClassBuilder builder)
        {
            return builder.Build();
        }
    }
}
