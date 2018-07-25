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
		                    @StoreAbbrev AS StoreAbbrev,
                            @PhoneNumber AS PhoneNumber,
                            @LocaleOpenDate AS LocaleOpenDate,
                            @LocaleCloseDate AS LocaleCloseDate
                    ) AS s
                    ON l.BusinessUnitID = s.BusinessUnitId
                    WHEN MATCHED THEN
	                    UPDATE SET StoreName = s.StoreName,
                                    StoreAbbrev = s.StoreAbbrev,
                                    PhoneNumber = s.PhoneNumber,
                                    LocaleOpenDate = s.LocaleOpenDate,
                                    LocaleCloseDate = s.LocaleCloseDate,
                                    ModifiedDate = GETDATE()
                    WHEN NOT MATCHED THEN
	                    INSERT (BusinessUnitID, StoreName, StoreAbbrev, PhoneNumber, LocaleOpenDate, LocaleCloseDate)
                        VALUES (s.BusinessUnitID, s.StoreName, s.StoreAbbrev, s.PhoneNumber, s.LocaleOpenDate, s.LocaleCloseDate);

                    MERGE dbo.StoreAddress AS sa
                    USING
                    (
                        SELECT
                            @BusinessUnitID AS BusinessUnitID,
                            @Address1 AS Address1,
                            @Address2 AS Address2,
                            @Address3 AS Address3,
                            @City AS City,
                            @Territory AS Territory,
                            @TerritoryAbbrev AS TerritoryAbbrev,
                            @PostalCode AS PostalCode,
                            @Country AS Country,
                            @CountryAbbrev AS CountryAbbrev,
                            @Timezone AS Timezone
                    ) AS a
                    ON sa.BusinessUnitID = a.BusinessUnitID
                    WHEN MATCHED THEN
                        UPDATE SET 
                                Address1 = a.Address1,
                                Address2 = a.Address2,
                                Address3 = a.Address3,
                                City = a.City,
                                Territory = a.Territory,
                                TerritoryAbbrev = a.TerritoryAbbrev,
                                PostalCode = a.PostalCode,
                                Country = a.Country,
                                CountryAbbrev = a.CountryAbbrev,
                                Timezone = a.Timezone,
                                ModifiedDate = GETDATE()
                    WHEN NOT MATCHED THEN
                        INSERT (BusinessUnitID,
                                Address1,
                                Address2,
                                Address3,
                                City,
                                Territory,
                                TerritoryAbbrev,
                                PostalCode,
                                Country,
                                CountryAbbrev,
                                Timezone,
                                AddedDate)
                        VALUES (a.BusinessUnitID,
                                a.Address1,
                                a.Address2,
                                a.Address3,
                                a.City,
                                a.Territory,
                                a.TerritoryAbbrev,
                                a.PostalCode,
                                a.Country,
                                a.CountryAbbrev,
                                a.Timezone,
                                GETDATE());";
        }

        public void Execute(AddOrUpdateLocalesCommand data)
        {
            var localeGroups = data.Locales.GroupBy(l => l.Region);
            foreach (var localeGroup in localeGroups)
            {
                string formattedSql = string.Format(sql, localeGroup.Key);

                dbProvider.Connection.Execute(
                    formattedSql,
                    localeGroup,
                    dbProvider.Transaction);
            }
        }
    }
}
