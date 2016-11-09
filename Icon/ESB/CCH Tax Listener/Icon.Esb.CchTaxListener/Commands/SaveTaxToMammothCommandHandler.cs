using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Icon.Esb.CchTax.Infrastructure;

namespace Icon.Esb.CchTax.Commands
{
    public class SaveTaxToMammothCommandHandler : ICommandHandler<SaveTaxToMammothCommand>
    {
        private IDataConnectionManager dataConnectionManager;
        private CchTaxListenerApplicationSettings settings;

        public SaveTaxToMammothCommandHandler(
            IDataConnectionManager dataConnectionManager,
            CchTaxListenerApplicationSettings settings)
        {
            this.dataConnectionManager = dataConnectionManager;
            this.settings = settings;
        }

        public void Execute(SaveTaxToMammothCommand data)
        {
            if (settings.UpdateMammoth)
            {
                IDataConnection mammoth = dataConnectionManager.Connection;

                DateTime now = DateTime.Now;
                int taxHierarchyId = mammoth.Query<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Tax'").First();

                mammoth.Execute(@"
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
                        VALUES (t.HierarchyClassID, t.TaxCode, t.AddedOrModifiedDate);",
                    data.TaxHierarchyClasses.Select(hc =>
                    new
                    {
                        Now = now,
                        TaxHierarchyId = taxHierarchyId,
                        HierarchyClassId = hc.HierarchyClassId,
                        HierarchyClassName = hc.HierarchyClassName,
                        TaxCode = hc.TaxCode
                    }));
            }
        }
    }
}
