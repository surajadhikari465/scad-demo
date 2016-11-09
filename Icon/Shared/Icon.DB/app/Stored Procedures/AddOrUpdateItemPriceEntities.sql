
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-09-30
-- Description:	Takes a collection of ItemPrice entities
--				and performs a MERGE to either add or
--				update the values in ItemPrice.
-- =============================================

CREATE PROCEDURE app.AddOrUpdateItemPriceEntities
	@ItemPriceEntities app.ItemPriceEntityType readonly
AS
BEGIN
	set nocount on

	if OBJECT_ID('tempdb..#ItemPriceEntities') is not null
		begin
			drop table #ItemPriceEntities;
		end

	create table #ItemPriceEntities
	(
		[ItemId]					INT			NOT NULL,
		[LocaleId]					INT			NOT NULL,
		[ItemPriceTypeId]			INT			NOT NULL,
		[UomId]						INT			NOT NULL,
		[CurrencyTypeId]			INT			NOT NULL,
		[ItemPriceAmount]			MONEY		NOT NULL,
		[BreakPointStartQuantity]   INT         NULL,
		[StartDate]					DATE		NULL,
		[EndDate]					DATE		NULL
	)

	insert into #ItemPriceEntities with (tablock) select * from @ItemPriceEntities

	create clustered index [IX_ItemPriceKey_ItemPriceEntities] on #ItemPriceEntities ([ItemId], [LocaleId], [ItemPriceTypeId])

	-- The locking hints here are designed to help with merge concurrency.
	merge
		dbo.ItemPrice with (updlock, rowlock) ip
	using
		#ItemPriceEntities ipe
	on
		ip.itemID			= ipe.ItemId and
		ip.localeID			= ipe.LocaleId and
		ip.itemPriceTypeID	= ipe.ItemPriceTypeId
	when matched then
		update set 
			ip.uomID				= ipe.UomId,
			ip.currencyTypeID		= ipe.CurrencyTypeId,
			ip.itemPriceAmt			= ipe.ItemPriceAmount,
			ip.breakPointStartQty	= ipe.BreakPointStartQuantity,
			ip.startDate			= ipe.StartDate,
			ip.endDate				= ipe.EndDate
	when not matched then
		insert 
			(itemID, localeID, itemPriceTypeID, uomID, currencyTypeID, itemPriceAmt, breakPointStartQty, startDate, endDate) 
		values 
			(ipe.ItemId, ipe.LocaleId, ipe.ItemPriceTypeId, ipe.UomId, ipe.CurrencyTypeId, ipe.ItemPriceAmount, ipe.BreakPointStartQuantity, ipe.StartDate, ipe.EndDate);
END
GO
