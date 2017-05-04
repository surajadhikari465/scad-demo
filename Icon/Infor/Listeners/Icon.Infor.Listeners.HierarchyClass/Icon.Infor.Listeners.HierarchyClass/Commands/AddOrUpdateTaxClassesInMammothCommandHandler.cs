using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class AddOrUpdateTaxClassesInMammothCommandHandler : ICommandHandler<AddOrUpdateTaxClassesInMammothCommand>
    {
        private readonly string mammothConnectionString;

        public AddOrUpdateTaxClassesInMammothCommandHandler()
        {
            mammothConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
        }

        public void Execute(AddOrUpdateTaxClassesInMammothCommand data)
        {
            using (var mammoth = new SqlConnection(mammothConnectionString))
            {
                DateTime now = DateTime.Now;
                int taxHierarchyId = mammoth.Query<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Tax'").First();
                string updateTaxSql = @"
                    DECLARE @taxes table
                    (
                        HierarchyClassID int,
                        HierarchyClassName nvarchar(255),
                        HierarchyID int,
                        TaxCode nvarchar(7),
                        AddedOrModifiedDate datetime
                    )

                    INSERT INTO @taxes(HierarchyClassID, HierarchyID, HierarchyClassName, TaxCode, AddedOrModifiedDate)
                    VALUES (@HierarchyClassId, @TaxHierarchyId, @HierarchyClassName, @TaxCode, @Now)

                    MERGE 
                        dbo.HierarchyClass hc
                    USING
                        @taxes t
                    ON
                        t.HierarchyClassID = hc.HierarchyClassID
                    WHEN MATCHED THEN
                        UPDATE SET
                            hc.HierarchyClassName = t.HierarchyClassName,
                            hc.ModifiedDate = t.AddedOrModifiedDate
                    WHEN NOT MATCHED THEN
                        INSERT (HierarchyClassID, HierarchyID, HierarchyClassName, AddedDate)
                        VALUES (t.HierarchyClassID, t.HierarchyID, t.HierarchyClassName, t.AddedOrModifiedDate);

                    MERGE 
                        dbo.Tax_Attributes ta
                    USING 
                        @taxes t
                    ON 
                        ta.TaxHCID = t.HierarchyClassId
                    WHEN MATCHED THEN 
                        UPDATE SET
                            ta.TaxCode = t.TaxCode,
                            ta.ModifiedDate = t.AddedOrModifiedDate
                    WHEN NOT MATCHED THEN
                        INSERT (TaxHCID, TaxCode, AddedDate)
                        VALUES (t.HierarchyClassID, t.TaxCode, t.AddedOrModifiedDate);";

                mammoth.Execute(
                    updateTaxSql,
                    data.TaxHierarchyClasses.Select(hc =>
                    new
                    {
                        Now = now,
                        TaxHierarchyId = taxHierarchyId,
                        HierarchyClassId = hc.HierarchyClassId,
                        HierarchyClassName = hc.HierarchyClassName,
                        TaxCode = hc.HierarchyClassName.Substring(0, 7)
                    }).ToList());
            }
        }
    }
}
