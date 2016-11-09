using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    public class TestStagingHierarchyClassModelBuilder
    {
        private int hierachyClassId;
        private int? hierarchyId;
        private string hierarchyClassName;
        private int? hierarchyClassParentId;
        private DateTime? timestamp;

        public TestStagingHierarchyClassModelBuilder()
        {
            this.hierachyClassId = 1;
            this.hierarchyId = 1;
            this.hierarchyClassName = "Test Hierarchy Class";
            this.hierarchyClassParentId = 1;
            this.timestamp = DateTime.Now;
        }

        internal TestStagingHierarchyClassModelBuilder WithHierarchyClassId(int hierarchyClassId)
        {
            this.hierachyClassId = hierarchyClassId;
            return this;
        }

        internal TestStagingHierarchyClassModelBuilder WithHierarchyId(int? hierarchyId)
        {
            this.hierarchyId = hierarchyId;
            return this;
        }

        internal TestStagingHierarchyClassModelBuilder WithHierarchyClassName(string hierarchyClassName)
        {
            this.hierarchyClassName = hierarchyClassName;
            return this;
        }

        internal TestStagingHierarchyClassModelBuilder WithTimestamp(DateTime? Timestamp)
        {
            this.timestamp = Timestamp;
            return this;
        }

        internal TestStagingHierarchyClassModelBuilder WithParentId(int? parentId)
        {
            this.hierarchyClassParentId = parentId;
            return this;
        }

        internal StagingHierarchyClassModel Build()
        {
            var hierarchyClass = new StagingHierarchyClassModel
            {
                HierarchyClassID = this.hierachyClassId,
                HierarchyClassName = this.hierarchyClassName,
                HierarchyID = this.hierarchyId,
                HierarchyClassParentID = this.hierarchyClassParentId,
                Timestamp = this.timestamp
            };

            return hierarchyClass;
        }

        internal List<StagingHierarchyClassModel> BuildList(int numberOfRows, int hierarchyId, int maxHierarchyClassId, DateTime timestamp, int? parentId = null)
        {
            var hierarchyClasses = new List<StagingHierarchyClassModel>();

            for (int i = 1; i <= numberOfRows; i++)
            {
                StagingHierarchyClassModel hierarchyClass = new TestStagingHierarchyClassModelBuilder()
                    .WithHierarchyClassId(maxHierarchyClassId + i)
                    .WithHierarchyClassName(String.Format("Test HierarchyClass {0}", i))
                    .WithHierarchyId(hierarchyId)
                    .WithTimestamp(timestamp)
                    .WithParentId(parentId)
                    .Build();

                hierarchyClasses.Add(hierarchyClass);
            }

            return hierarchyClasses;
        }
    }
}
