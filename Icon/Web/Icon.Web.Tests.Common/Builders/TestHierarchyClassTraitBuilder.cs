using Icon.Framework;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestHierarchyClassTraitBuilder
    {
        private int traitId;
        private int hierarchyClassId;
        private int? uomId;
        private string traitValue;

        public TestHierarchyClassTraitBuilder()
        {
            this.traitId = 1;
            this.hierarchyClassId = 1;
            this.uomId = null;
            this.traitValue = "test";
        }

        public TestHierarchyClassTraitBuilder WithTraitId(int traitId)
        {
            this.traitId = traitId;
            return this;
        }

        public TestHierarchyClassTraitBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierarchyClassId = hierarchyClassId;
            return this;
        }

        public TestHierarchyClassTraitBuilder WithUomId(int? uomId)
        {
            this.uomId = uomId;
            return this;
        }

        public TestHierarchyClassTraitBuilder WithTraitValue(string traitValue)
        {
            this.traitValue = traitValue;
            return this;
        }

        public HierarchyClassTrait Build()
        {
            var hierarchyClassTrait = new HierarchyClassTrait
            {
                traitID = this.traitId,
                traitValue = this.traitValue,
                hierarchyClassID = this.hierarchyClassId,
                uomID = this.uomId
            };
            
            return hierarchyClassTrait;
        }

        public static implicit operator HierarchyClassTrait(TestHierarchyClassTraitBuilder builder)
        {
            return builder.Build();
        }
    }
}
