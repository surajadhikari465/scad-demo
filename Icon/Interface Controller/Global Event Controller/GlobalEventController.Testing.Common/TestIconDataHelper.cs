using Icon.Framework;
using Icon.Testing.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Testing.Common
{
    public class TestIconDataHelper
    {
        public HierarchyClass SaveHierarchyClassForTest(IconContext context, string brandAbbreviationTrait)
        {
            HierarchyClass hierarchyClass;
            if (brandAbbreviationTrait == null)
            {
                hierarchyClass = new TestHierarchyClassBuilder()
                    .Build();
            }
            else
            {
                hierarchyClass = new TestHierarchyClassBuilder()
                    .WithBrandAbbreviation(brandAbbreviationTrait)
                    .Build();
            }

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            return hierarchyClass;
        }
    }
}
