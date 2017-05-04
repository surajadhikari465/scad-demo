using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdatePricesCommandHandler : ICommandHandler<AddOrUpdatePricesCommand>
    {
        private IDbProvider db;

        public AddOrUpdatePricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        /// <summary>
        /// Updates or Inserts prices into the Price_XX table.
        /// Since most of the time prices will be inserted, it keeps track of when an UPDATE
        /// needs to happen to help with performance.
        /// </summary>
        /// <param name="data">@Region, @TransactionId, and @Timestamp</param>
        public void Execute(AddOrUpdatePricesCommand data)
        {
            string sql = $@"-- keeping track of what prices need to be inserted vs. updated
                            DECLARE @totalPriceCount int;
                            DECLARE @insertPriceCount int;

                            -- put all prices from staging to temp table
                            SELECT
                                @Region as Region,
	                            i.ItemID,
	                            sp.BusinessUnitID,
                                sp.Multiple,
	                            sp.Price,
	                            sp.PriceType,
	                            sp.StartDate,
	                            sp.EndDate,
                                sp.PriceUOM,
                                c.CurrencyID,
	                            @Timestamp as Timestamp
                            INTO #allPrices
                            FROM
	                            stage.Price	        sp
	                            JOIN Items			i	on sp.ScanCode = i.ScanCode
                                JOIN Currency       c   on sp.CurrencyCode = c.CurrencyCode
                            WHERE
	                            sp.TransactionId = @TransactionId
                                AND sp.Region = @Region
                            
                            -- keep track of total price count to determine if updates are needed
                            SET @totalPriceCount = @@ROWCOUNT

                            CREATE NONCLUSTERED INDEX IX_Region_Item_BU_StartDate_PriceType_#allPrices on #allPrices (Region ASC, ItemID ASC, BusinessUnitID ASC, StartDate ASC, PriceType ASC)
	                            INCLUDE (Multiple, Price, EndDate, PriceUOM, CurrencyID, Timestamp)

                            -- put all new prices into its own temp table for the inserts                        
                            SELECT
	                            ap.Region,
	                            ap.ItemID,
	                            ap.BusinessUnitID,
                                ap.Multiple,
	                            ap.Price,
	                            ap.PriceType,
	                            ap.StartDate,
	                            ap.EndDate,
                                ap.PriceUOM,
                                ap.CurrencyID,
	                            ap.Timestamp
                            INTO #insertPrices
                            FROM #allPrices ap
                            WHERE NOT EXISTS
                            (
	                            SELECT 1
	                            FROM Price_{data.Region} p
	                            WHERE ap.Region			= p.Region
	                            AND ap.ItemID			= p.ItemID
	                            AND ap.BusinessUnitID	= p.BusinessUnitID
	                            AND ap.StartDate		= p.StartDate
	                            AND ap.PriceType		= p.PriceType
                            )

                            -- for determining if an UPDATE needs to happen
                            SET @insertPriceCount = @@ROWCOUNT

                            CREATE NONCLUSTERED INDEX IX_Region_Item_BU_StartDate_PriceType_#insertPrices on #insertPrices (Region ASC, ItemID ASC, BusinessUnitID ASC, StartDate ASC, PriceType ASC)
	                            INCLUDE (Multiple, Price, EndDate, PriceUOM, CurrencyID, Timestamp)

                            BEGIN TRY
                            BEGIN TRANSACTION

	                            IF @insertPriceCount <> @totalPriceCount
		                            UPDATE p
		                            SET 
			                            Multiple = prc.Multiple,
			                            Price = prc.Price,
			                            EndDate = prc.EndDate,
			                            PriceUom = prc.PriceUom,
			                            CurrencyID = prc.CurrencyID,
			                            ModifiedDate = prc.Timestamp
		                            FROM
			                            Price_{data.Region} 		p
			                            JOIN #allPrices	prc on p.Region = prc.Region
								                            AND p.ItemID = prc.ItemID
								                            AND p.BusinessUnitID = prc.BusinessUnitID
								                            AND p.StartDate = prc.StartDate
								                            AND p.PriceType = prc.PriceType

                                IF @insertPriceCount > 0
	                                INSERT INTO Price_{data.Region}
	                                (
		                                ItemID,
		                                BusinessUnitID,
		                                Multiple,
		                                Price,
		                                PriceType,
		                                StartDate,
		                                EndDate,
		                                PriceUom,
		                                CurrencyID,
		                                AddedDate
	                                )
	                                SELECT
		                                p.ItemID,
		                                p.BusinessUnitID,
		                                p.Multiple,
		                                p.Price,
		                                p.PriceType,
		                                p.StartDate,
		                                p.EndDate,
		                                p.PriceUom,
		                                p.CurrencyID,
		                                p.Timestamp
	                                FROM
		                                #insertPrices p

	                            COMMIT TRANSACTION
                            END TRY
                            BEGIN CATCH
	                            ROLLBACK TRANSACTION;
	                            THROW
                            END CATCH";

            int affectedRows = this.db.Connection
                .Execute(
                    sql, 
                    new
                    {
                        Timestamp = data.Timestamp,
                        Region = new DbString { Value = data.Region, Length = 2 },
                        TransactionId = data.TransactionId
                    },
                    transaction: this.db.Transaction);
        }
    }
}
