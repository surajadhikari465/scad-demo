using System;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Linq;
using Icon.Common;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateNationalHierarchyLineageCommandHandler : ICommandHandler<AddOrUpdateNationalHierarchyLineageCommand>
    {
        private IDbProvider dbProvider;

        public AddOrUpdateNationalHierarchyLineageCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            string levelName = data.HierarchyClasses[0].HierarchyLevelName;

            switch (levelName)
            {
                case Constants.National.HierarchyLevels.Family:
                    UpdateFamilies(data);
                    break;
                case Constants.National.HierarchyLevels.Category:
                    UpdateCategories(data);
                    break;
                case Constants.National.HierarchyLevels.SubCategory:
                    UpdateSubCategories(data);
                    break;
                case Constants.National.HierarchyLevels.NationalClass:
                    UpdateClasses(data);
                    break;
                default: throw new Exception($"Invalid Hierarchy Level: {levelName}");
            }
        }

        private void UpdateFamilies(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyNationalFamily",
                param: new
                {
                    hierarchyNational = data.HierarchyClasses.Select(x => new
                    {
                        FamilyHCID = x.HierarchyClassId,
                        CategoryHCID = (int?)null,
                        SubcategoryHCID = (int?)null,
                        ClassHCID = (int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter("dbo.HierarchyNationalClassType")
                },
            transaction: dbProvider.Transaction,
            commandType: System.Data.CommandType.StoredProcedure);
        }

        private void UpdateCategories(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyNationalCategory",
                param: new
                {
                    hierarchyNational = data.HierarchyClasses.Select(x => new
                    {
                        FamilyHCID = x.HierarchyClassParentId,
                        CategoryHCID = x.HierarchyClassId,
                        SubcategoryHCID = (int?)null,
                        ClassHCID = (int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter("dbo.HierarchyNationalClassType")
                },
            transaction: dbProvider.Transaction,
            commandType: System.Data.CommandType.StoredProcedure);
        }

        private void UpdateSubCategories(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: "dbo.AddOrUpdateHierarchyNationalSubCategory",
                param: new
                {
                    hierarchyNational = data.HierarchyClasses.Select(x => new
                    {
                        FamilyHCID = (int?)null,
                        CategoryHCID = x.HierarchyClassParentId,
                        SubcategoryHCID = x.HierarchyClassId,
                        ClassHCID = (int?)null
                    })
                    .ToDataTable()
                    .AsTableValuedParameter("dbo.HierarchyNationalClassType")
                },
            transaction: dbProvider.Transaction,
            commandType: System.Data.CommandType.StoredProcedure);
		}

        private void UpdateClasses(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
               sql: "dbo.AddOrUpdateHierarchyNationalClass",
               param: new
               {
                   hierarchyNational = data.HierarchyClasses.Select(x => new
                   {
                        FamilyHCID = (int?)null,
                        CategoryHCID = (int?)null,
                        SubcategoryHCID = x.HierarchyClassParentId,
                        ClassHCID = x.HierarchyClassId
                   })
                    .ToDataTable()
                    .AsTableValuedParameter("dbo.HierarchyNationalClassType")
            	},
            transaction: dbProvider.Transaction,
            commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}