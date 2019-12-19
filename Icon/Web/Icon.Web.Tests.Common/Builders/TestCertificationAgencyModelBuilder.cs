using Icon.Web.DataAccess.Models;
using Icon.Framework;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestCertificationAgencyModelBuilder
    {
        private int hierarchyClassId;
        private string hierarchyClassName;
        private int hierarchyId;
        private int? hierarchyLevel;
        private string hierarchyLineage;
        private int? hierarchyParentClassId;
        private string glutenFree;
        private string kosher;
        private string nonGMO;
        private string organic;
        private string vegan;

        public TestCertificationAgencyModelBuilder()
        {
             this.hierarchyClassId = 1;
            this.hierarchyId = Hierarchies.Merchandise;
            this.hierarchyClassName = "Test HierarchyClass Model";
            this.hierarchyLevel = 1;
            this.hierarchyLineage = "Test1";
            this.hierarchyParentClassId = null;
            glutenFree = "0";
            kosher = "0";
            nonGMO = "0";
            organic = "0";
            vegan = "0";
        }


        public TestCertificationAgencyModelBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierarchyClassId = hierarchyClassId;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithHierarchyId(int hierarchyId)
        {
            this.hierarchyId = hierarchyId;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithHierarchyClassName(string hierarchyClassName)
        {
            this.hierarchyClassName = hierarchyClassName;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithHierarchyLevel(int? hierarchyLevel)
        {
            this.hierarchyLevel = hierarchyLevel;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithHierarchyClassParentId(int? hierarchyClassParentId)
        {
            this.hierarchyParentClassId = hierarchyClassParentId;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithHierarchyClassLineage(string hierarchyClassLineage)
        {
            this.hierarchyLineage = hierarchyClassLineage;
            return this;
        }
        public TestCertificationAgencyModelBuilder WithGlutenFree(string glutenFree)
        {
            this.glutenFree = glutenFree;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithKosher(string kosher)
        {
            this.kosher = kosher;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithNonGMO(string nonGMO)
        {
            this.nonGMO = nonGMO;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithOrganic(string organic)
        {
            this.organic = organic;
            return this;
        }

        public TestCertificationAgencyModelBuilder WithVegan(string vegan)
        {
            this.vegan = vegan;
            return this;
        }


        public CertificationAgencyModel Build()
        {
            var certificationAgencyModel = new CertificationAgencyModel
            {
                HierarchyClassId = this.hierarchyClassId,
                HierarchyClassName = this.hierarchyClassName,
                HierarchyId = this.hierarchyId,
                HierarchyLevel = this.hierarchyLevel,
                HierarchyLineage = this.hierarchyLineage,
                HierarchyParentClassId = this.hierarchyParentClassId,
                GlutenFree = this.glutenFree,
                Kosher = this.kosher,
                NonGMO = this.nonGMO,
                Organic = this.organic,
                Vegan = this.vegan

            };

            return certificationAgencyModel;
        }

        public static implicit operator CertificationAgencyModel(TestCertificationAgencyModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
