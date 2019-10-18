using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.HierarchyClassListener.Models;
using System.Linq;

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
            }
        }

        private void UpdateFamilies(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            var nationalModels = data.HierarchyClasses
                .Select(hc => new NationalHierarchyModel { FamilyHcid = hc.HierarchyClassId });

            dbProvider.Connection.Execute(
                sql: @"CREATE TABLE #FamilyIds (FamilyHCID int)",
                param: null,
                transaction: dbProvider.Transaction);
            var tempInsertResult = dbProvider.Connection.Execute(
                sql: @"INSERT INTO #FamilyIds VALUES (@FamilyHcid)",
                param: nationalModels,
                transaction: dbProvider.Transaction);
            if (tempInsertResult > 0)
            {
                var result = dbProvider.Connection.Execute(
                   sql: @"INSERT INTO dbo.Hierarchy_NationalClass (FamilyHCID)
                          SELECT FamilyHCID 
                          FROM #FamilyIds temp
                          WHERE NOT EXISTS
                          (SELECT * FROM dbo.Hierarchy_NationalClass WHERE FamilyHCID = temp.FamilyHCID)",
                   param: null,
                   transaction: dbProvider.Transaction);
            }

            dbProvider.Connection.Execute(sql: @"DROP TABLE #FamilyIds", param: null, transaction: dbProvider.Transaction);
        }

        private void UpdateCategories(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                sql: @"CREATE TABLE #TempNationalHierarchy
                  (
                      FamilyHCID int null,
                      CategoryHCID int null
                  )",
                param: null,
                transaction: dbProvider.Transaction);
            var tempInsertResult = dbProvider.Connection.Execute(
                  sql: @"INSERT INTO #TempNationalHierarchy 
                        VALUES (@FamilyHcid, @CategoryHcid)",
                   param: data.HierarchyClasses.Select(hc => new NationalHierarchyModel
                   {
                       FamilyHcid = hc.HierarchyClassParentId,
                       CategoryHcid = hc.HierarchyClassId
                   }),
                   transaction: dbProvider.Transaction);
            if (tempInsertResult > 0)
            {
                var updateResult = dbProvider.Connection.Execute(
                    sql: @"UPDATE hm
                        SET CategoryHCID = temp.CategoryHCID, ModifiedDate = GETDATE()
                        FROM dbo.Hierarchy_NationalClass hm
                        JOIN #TempNationalHierarchy temp 
                            ON hm.FamilyHCID = temp.FamilyHCID AND hm.CategoryHCID IS NULL",
                    param: null,
                    transaction: dbProvider.Transaction);
                if (tempInsertResult > updateResult)
                {
                    var insertResult = dbProvider.Connection.Execute(
                        sql: @"INSERT INTO dbo.Hierarchy_NationalClass
                                (FamilyHCID, CategoryHCID)
                            SELECT *
                            FROM #TempNationalHierarchy temp
                            WHERE NOT EXISTS
                            (SELECT * FROM dbo.Hierarchy_NationalClass WHERE CategoryHCID = temp.CategoryHCID)",
                        param: null,
                        transaction: dbProvider.Transaction);
                }
            }
            dbProvider.Connection.Execute(
                sql: @"DROP TABLE #TempNationalHierarchy", param: null, transaction: dbProvider.Transaction);
        }

        private void UpdateSubCategories(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempNationalHierarchy
                  (
                      FamilyHCID int null,
                      CategoryHCID int null,
                      SubcategoryHCID int null,
                      ClassHCID int null
                  )", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO #TempNationalHierarchy 
                  VALUES (@FamilyHcid, @CategoryHcid, @SubcategoryHcid, @ClassHcid)",
                data.HierarchyClasses.Select(hc => new NationalHierarchyModel
                {
                    CategoryHcid = hc.HierarchyClassParentId,
                    SubcategoryHcid = hc.HierarchyClassId
                }),
                dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"UPDATE temp
                      SET FamilyHCID = hm.FamilyHCID
                  FROM #TempNationalHierarchy temp
                  JOIN dbo.Hierarchy_NationalClass hm ON temp.CategoryHCID = hm.CategoryHCID", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"UPDATE hm
                      SET hm.SubcategoryHCID = temp.SubcategoryHCID
                  FROM dbo.Hierarchy_NationalClass hm
                  JOIN #TempNationalHierarchy temp ON hm.CategoryHCID = temp.CategoryHCID
                      AND hm.SubcategoryHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)
                  SELECT temp.*
                  FROM #TempNationalHierarchy temp
                  LEFT JOIN dbo.Hierarchy_NationalClass hm on temp.CategoryHCID = hm.CategoryHCID
                  WHERE hm.SubcategoryHCID IS NULL", null, dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"DROP TABLE #TempNationalHierarchy", null, dbProvider.Transaction);
        }

        private void UpdateClasses(AddOrUpdateNationalHierarchyLineageCommand data)
        {
            dbProvider.Connection.Execute(
                @"CREATE TABLE #TempNationalHierarchy
                  (
                      FamilyHCID int null,
                      CategoryHCID int null,
                      SubcategoryHCID int null,
                      ClassHCID int null
                  );

				  INSERT INTO #TempNationalHierarchy 
                  VALUES (@FamilyHcid, @CategoryHcid, @SubcategoryHcid, @ClassHcid);
				  
				  UPDATE temp
                      SET FamilyHCID = hm.FamilyHCID,
                          CategoryHCID = hm.CategoryHCID
                  FROM #TempNationalHierarchy temp
                  JOIN dbo.Hierarchy_NationalClass hm ON temp.SubcategoryHCID = hm.SubcategoryHCID

                  UPDATE hm
                      SET ClassHCID = temp.ClassHCID
                  FROM dbo.Hierarchy_NationalClass hm
                  JOIN #TempNationalHierarchy temp ON hm.SubcategoryHCID = temp.SubcategoryHCID
                      AND hm.ClassHCID IS NULL

                  INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)
                  SELECT temp.*
                  FROM #TempNationalHierarchy temp
                  LEFT JOIN dbo.Hierarchy_NationalClass hm on temp.ClassHCID = hm.ClassHCID
                  WHERE hm.ClassHCID IS NULL;

				  DROP TABLE #TempNationalHierarchy;",
                data.HierarchyClasses.Select(hc => new NationalHierarchyModel
                {
                    SubcategoryHcid = hc.HierarchyClassParentId,
                    ClassHcid = hc.HierarchyClassId
                }),
                dbProvider.Transaction);
        }
    }
}