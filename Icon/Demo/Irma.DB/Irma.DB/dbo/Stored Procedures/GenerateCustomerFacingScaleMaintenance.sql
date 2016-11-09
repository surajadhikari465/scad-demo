
-- =============================================
-- Author:		Cake
-- Create date: 2016-02-03
-- Description:	Inserts scale maintenance for 365 non-scale PLUs.
-- =============================================

create PROCEDURE dbo.GenerateCustomerFacingScaleMaintenance
	@ItemKey int,
	@ActionCode nvarchar(1)
AS
BEGIN
	set nocount on

	declare @filterActionCode nvarchar(4) = case 
												when @ActionCode = 'A' then 'A'
												when @ActionCode = 'C' then '[AC]'
												else @ActionCode --Only A and C are currently supported so defaulting the filter to ActionCode if not A or C.
											end

	insert into 
		PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
    select 
		@ItemKey, @ActionCode, s.Store_No
	from
		Store s
	join 
		StoreItem si on si.Item_Key = @ItemKey and si.Store_No = s.Store_No
	where
		(s.Mega_Store = 1 and s.WFM_Store = 0)
		and si.Authorized = 1 
		and not exists (select pq.item_key from PlumCorpChgQueue pq (nolock) where pq.Item_Key = @ItemKey AND pq.store_no = si.store_no AND pq.ActionCode like @filterActionCode) 
		and not exists (select pqt.item_key from PlumCorpChgQueueTmp pqt (nolock) where pqt.Item_Key = @ItemKey AND pqt.store_no = si.store_no AND pqt.ActionCode like @filterActionCode)
END

