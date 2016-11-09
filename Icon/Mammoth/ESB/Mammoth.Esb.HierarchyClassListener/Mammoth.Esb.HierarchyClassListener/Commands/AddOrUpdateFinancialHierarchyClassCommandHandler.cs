using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateFinancialHierarchyClassCommandHandler : ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>
    {
        private IDbProvider dbProvider;

        public AddOrUpdateFinancialHierarchyClassCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(AddOrUpdateFinancialHierarchyClassCommand data)
        {
            dbProvider.Connection.Execute(
                @"
                DECLARE @now datetime = GETDATE();
                DECLARE @financialHierarchyClasses table
                        (
                            PSNumber int,
                            Name nvarchar(255)
                        );

                INSERT INTO @financialHierarchyClasses
                VALUES (@HierarchyClassId, @HierarchyClassName);

                MERGE dbo.Financial_Subteam fs
                USING 
                    @financialHierarchyClasses fhc
                ON fs.PSNumber = fhc.PSNumber
                WHEN MATCHED THEN
                    UPDATE
                    SET 
                        fs.Name = fhc.Name,
                        fs.ModifiedDate = @now
                WHEN NOT MATCHED THEN
                    INSERT (PSNumber, Name, AddedDate)
                    VALUES (fhc.PSNumber, fhc.Name, @now);",
                data.HierarchyClasses,
                dbProvider.Transaction);
        }
    }
}
