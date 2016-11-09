using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Linq;

namespace Mammoth.Esb.LocaleListener.Commands
{
    public class AddOrUpdateLocalesCommandHandler : ICommandHandler<AddOrUpdateLocalesCommand>
    {
        private IDbProvider dbProvider;
        private string sql;

        public AddOrUpdateLocalesCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            sql = @"MERGE dbo.Locales_{0} AS l
                    USING
                    (
	                    SELECT
                            @BusinessUnitID AS BusinessUnitID,
		                    @StoreName AS StoreName,
		                    @StoreAbbrev AS StoreAbbrev
                    ) AS s
                    ON l.BusinessUnitID = s.BusinessUnitId
                    WHEN MATCHED THEN
	                    UPDATE SET StoreName = s.StoreName,
                                    StoreAbbrev = s.StoreAbbrev,
                                    ModifiedDate = GETDATE()
                    WHEN NOT MATCHED THEN
	                    INSERT (BusinessUnitID, StoreName, StoreAbbrev)
                        VALUES (s.BusinessUnitID, s.StoreName, s.StoreAbbrev);";
        }

        public void Execute(AddOrUpdateLocalesCommand data)
        {
            var localeGroups = data.Locales.GroupBy(l => l.Region);
            foreach (var localeGroup in localeGroups)
            {
                string formattedSql = string.Format(sql, localeGroup.Key);

                dbProvider.Connection.Execute(formattedSql,
                    localeGroup,
                    dbProvider.Transaction);
            }
        }
    }
}
