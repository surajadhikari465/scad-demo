using Icon.Framework;
using System;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestPluCategoryBuilder
    {
        private int pluCategoryID;
        private string pluCategoryName;
        private string beginRange;
        private string endRange;

        public TestPluCategoryBuilder()
        {
            this.pluCategoryID = 0;
            this.pluCategoryName = "TestCat1";
            this.beginRange = "1";
            this.endRange = "5";
        }

        public TestPluCategoryBuilder WithPluCategoryID(int id)
        {
            this.pluCategoryID = id;
            return this;
        }

        public TestPluCategoryBuilder WithPluCategoryName(string pluCategoryName)
        {
            this.pluCategoryName = pluCategoryName;
            return this;
        }

        public TestPluCategoryBuilder WithBeginRange(string beginRange)
        {
            this.beginRange = beginRange;
            return this;
        }

        public TestPluCategoryBuilder WithEndRange(string endRange)
        {
            this.endRange = endRange;
            return this;
        }

        public PLUCategory Build()
        {
            var pluCategory = new PLUCategory
            {
                PluCategoryID = this.pluCategoryID,
                PluCategoryName = this.pluCategoryName,
                BeginRange = this.beginRange,
                EndRange = this.endRange
            };

            return pluCategory;
        }

        public static implicit operator PLUCategory(TestPluCategoryBuilder builder)
        {
            return builder.Build();
        }


        public PluCategoryViewModel BuildViewModel()
        {
            var pluCategory = new PluCategoryViewModel
            {
                PluCategoryId = this.pluCategoryID,
                PluCategoryName = this.pluCategoryName,
                BeginRange = this.beginRange,
                EndRange = this.endRange
            };

            return pluCategory;
        }

        public static implicit operator PluCategoryViewModel(TestPluCategoryBuilder builder)
        {
            return builder.BuildViewModel();
        }
    }
}
