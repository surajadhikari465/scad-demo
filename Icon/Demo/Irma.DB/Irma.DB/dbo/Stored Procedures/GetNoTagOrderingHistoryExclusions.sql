
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-05
-- Description:	Returns the set of items that have no 
--				ordering history in the specified period
--				for the specified store.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagOrderingHistoryExclusions]
	@ItemKeys dbo.IntType readonly,
	@StoreNumber int,
	@Days int
AS
BEGIN
	set nocount on

	declare @LookbackDate smalldatetime = cast(cast(getdate() as date) as smalldatetime) - @Days

	if OBJECT_ID('tempdb..#OrderHistoryWithinSpecifiedRange') is not null
		begin
			drop table #OrderHistoryWithinSpecifiedRange;
		end

	create table #OrderHistoryWithinSpecifiedRange(Item_Key int not null primary key clustered)

	insert into #OrderHistoryWithinSpecifiedRange
		select distinct
			oi.Item_Key
		from
			OrderItem (nolock) oi
			join OrderHeader (nolock) oh on oi.OrderHeader_ID = oh.OrderHeader_ID
			join Vendor (nolock) v on oh.ReceiveLocation_ID = v.Vendor_ID
		where
			v.Store_no = @StoreNumber
			and oh.SentDate > @LookbackDate

	select
		ik.[Key] as ItemKey
	from
		@ItemKeys ik
		left join #OrderHistoryWithinSpecifiedRange o on ik.[Key] = o.Item_Key
	where
		o.Item_Key is null
END
GO