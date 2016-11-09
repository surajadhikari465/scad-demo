
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-12-22
-- Description:	Returns item keys that are batched as
--				off-sale regular price changes but that
--				have no actual price change.  Used in
--				no-tag logic.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagOffSaleExclusions]
	@ItemKeys dbo.IntType readonly,
	@PriceBatchHeaderId int
AS
BEGIN
	set nocount on

    select
		ik.[Key] as ItemKey
	from
		@ItemKeys ik
		join PriceBatchDetail (nolock) pbd on ik.[Key] = pbd.Item_Key and pbd.PriceBatchHeaderID = @PriceBatchHeaderId
		join Price (nolock) p on pbd.Item_Key = p.Item_Key and pbd.Store_No = p.Store_No
	where
		(pbd.CancelAllSales = 1 or pbd.InsertApplication = 'Sale Off')
		and pbd.Price = p.Price
END