using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Linq;

namespace KitBuilder.Esb.LocaleListener.Commands
{
    public class AddOrUpdateLocalesCommandHandler : ICommandHandler<AddOrUpdateLocalesCommand>
    {
        private IDbProvider dbProvider;
        private string sql;

        public AddOrUpdateLocalesCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            sql = @"MERGE [dbo].[Locale] AS l
                    USING
                    (
	                    SELECT
                            @LocaleID AS LocaleID,
							@LocaleName AS LocaleName,
							@LocaleTypeID AS LocaleTypeID,
		                    @StoreID AS StoreID,
							@MetroID AS MetroID,
                            @RegionID AS RegionID,
		                    @ChainID AS ChainID,
                            @LocaleOpenDate AS LocaleOpenDate,
                            @LocaleCloseDate AS LocaleCloseDate,
							@RegionCode AS RegionCode,
							@BusinessUnitID AS BusinessUnitID,
							@StoreAbbreviation AS StoreAbbreviation,
							@CurrencyCode AS CurrencyCode,
							@Hospitality AS Hospitality

                    ) AS s
                    ON l.LocaleId = s.LocaleId
                    WHEN MATCHED THEN
	                    UPDATE SET LocaleID = s.LocaleID,
                                    LocaleName = s.LocaleName,
                                    LocaleTypeID = s.LocaleTypeID,
									StoreID = s.StoreID,
									MetroID = s.MetroID,
									RegionID = s.RegionID,
									ChainID = s.ChainID,
                                    LocaleOpenDate = s.LocaleOpenDate,
                                    LocaleCloseDate = s.LocaleCloseDate,
									RegionCode = s.RegionCode,
									BusinessUnitID = s.BusinessUnitID,
									StoreAbbreviation = s.StoreAbbreviation,
									CurrencyCode = s.CurrencyCode,
									Hospitality = s.Hospitality
                    WHEN NOT MATCHED THEN
	                    INSERT(
                        LocaleId,
                        LocaleName,
                        LocaleTypeId,
                        StoreId,
                        MetroId,
                        RegionId,
                        ChainId,
                        LocaleOpenDate,
                        LocaleCloseDate,
                        RegionCode,
                        BusinessUnitId,
                        StoreAbbreviation,
						CurrencyCode,
						Hospitality)
                    VALUES (
                        @LocaleID,
                        @LocaleName,
                        @LocaleTypeID,
                        @StoreID,
                        @MetroID,
                        @RegionID,
                        @ChainID,
                        @LocaleOpenDate,
                        @LocaleCloseDate,
                        @RegionCode,
						@BusinessUnitID,
                        @StoreAbbreviation,
						@CurrencyCode,
						@Hospitality);
					";
        }

        public void Execute(AddOrUpdateLocalesCommand data)
        {
			var localeGroups = data.Locales.GroupBy(l => l.LocaleID);
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
