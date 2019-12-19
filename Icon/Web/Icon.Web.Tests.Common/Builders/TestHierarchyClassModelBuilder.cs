using Icon.Web.DataAccess.Models;
using Icon.Framework;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestHierarchyClassModelBuilder
    {
        private int hierarchyClassId;
        private string hierarchyClassName;
        private int hierarchyId;
        private int? hierarchyLevel;
        private string hierarchyLineage;
        private int? hierarchyParentClassId;

        public TestHierarchyClassModelBuilder()
        {
            this.hierarchyClassId = 1;
            this.hierarchyId = Hierarchies.Merchandise;
            this.hierarchyClassName = "Test HierarchyClass Model";
            this.hierarchyLevel = 1;
            this.hierarchyLineage = "Test1";
            this.hierarchyParentClassId = null;
        }

        public TestHierarchyClassModelBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierarchyClassId = hierarchyClassId;
            return this;
        }

        public TestHierarchyClassModelBuilder WithHierarchyId(int hierarchyId)
        {
            this.hierarchyId = hierarchyId;
            return this;
        }

        public TestHierarchyClassModelBuilder WithHierarchyClassName(string hierarchyClassName)
        {
            this.hierarchyClassName = hierarchyClassName;
            return this;
        }

        public TestHierarchyClassModelBuilder WithHierarchyLevel(int? hierarchyLevel)
        {
            this.hierarchyLevel = hierarchyLevel;
            return this;
        }

        public TestHierarchyClassModelBuilder WithHierarchyClassParentId(int? hierarchyClassParentId)
        {
            this.hierarchyParentClassId = hierarchyClassParentId;
            return this;
        }

        public TestHierarchyClassModelBuilder WithHierarchyClassLineage(string hierarchyClassLineage)
        {
            this.hierarchyLineage = hierarchyClassLineage;
            return this;
        }

        public HierarchyClassModel Build()
        {
            var hierarchyClassModel = new HierarchyClassModel
            {
                HierarchyClassId = this.hierarchyClassId,
                HierarchyClassName = this.hierarchyClassName,
                HierarchyId = this.hierarchyId,
                HierarchyLevel = this.hierarchyLevel,
                HierarchyLineage = this.hierarchyLineage,
                HierarchyParentClassId = this.hierarchyParentClassId
            };

            return hierarchyClassModel;
        }

        public static implicit operator HierarchyClassModel(TestHierarchyClassModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
