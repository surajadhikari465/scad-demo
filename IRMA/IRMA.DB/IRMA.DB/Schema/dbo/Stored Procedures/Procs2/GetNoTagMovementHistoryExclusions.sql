
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-05
-- Description:	Returns the set of items that have 
--				no movement in the specified period
--				for the specified store.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagMovementHistoryExclusions]
	@ItemKeys dbo.IntType readonly,
	@StoreNumber int,
	@Days int
AS
BEGIN
	set nocount on

	declare @LookbackDate smalldatetime = cast(cast(getdate() as date) as smalldatetime) - @Days

	if OBJECT_ID('tempdb..#MovementWithinSpecifiedRange') is not null
		begin
			drop table #MovementWithinSpecifiedRange;
		end

	create table #MovementWithinSpecifiedRange(Item_Key int not null primary key clustered)

	insert into #MovementWithinSpecifiedRange
		select distinct
			ssi.Item_Key
		from
			@ItemKeys ik
			join Sales_SumByItem (nolock) ssi on ik.[key] = ssi.Item_Key
		where
			ssi.Store_No = @StoreNumber
			and ssi.Date_Key > @LookbackDate

	select
		ik.[Key] as ItemKey
	from
		@ItemKeys ik
		left join #MovementWithinSpecifiedRange m on ik.[Key] = m.Item_Key
	where
		m.Item_Key is null
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagMovementHistoryExclusions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagMovementHistoryExclusions] TO [IRSUser]
    AS [dbo];

