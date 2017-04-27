if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetNoTagReceivingHistoryExclusions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetNoTagReceivingHistoryExclusions]
GO

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-05
-- Description:	Returns the set of items that have 
--				no receiving history in the specified period
--				for the specified store.
-- =============================================

ALTER PROCEDURE [dbo].[GetNoTagReceivingHistoryExclusions]
	@ItemKeys dbo.IntType readonly,
	@StoreNumber int,
	@Days int
AS
BEGIN
	set nocount on;

	declare @LookbackDate datetime = cast(cast(getdate() as date) as datetime) - @Days

	if OBJECT_ID('tempdb..#ReceivingHistoryWithinSpecifiedRange') is not null
		begin
			drop table #ReceivingHistoryWithinSpecifiedRange;
		end

	create table #ReceivingHistoryWithinSpecifiedRange (Item_Key int not null primary key clustered)

	insert into #ReceivingHistoryWithinSpecifiedRange
		select distinct
			oi.Item_Key
		from
			@ItemKeys ik
			join OrderItem (nolock) oi on ik.[Key] = oi.Item_Key
			join OrderHeader (nolock) oh on oi.OrderHeader_ID = oh.OrderHeader_ID
			join Vendor (nolock) v on oh.ReceiveLocation_ID = v.Vendor_ID
		where
			v.Store_no = @StoreNumber
			and oi.DateReceived > @LookbackDate

	select
		ik.[Key] as ItemKey
	from
		@ItemKeys ik
		left join #ReceivingHistoryWithinSpecifiedRange r on ik.[Key] = r.Item_Key
	where
		r.Item_Key is null
END
GO
