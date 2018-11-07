
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-11-03
-- Description:	Returns SignQueue details for tag
--				reprint requests, which will ultimately
--				be used for the SLAW API.
-- =============================================

CREATE PROCEDURE [dbo].[GetSignQueueDetailForItemKeys]
	@StoreNumber int,
	@ItemList varchar(max),
	@ItemListSeparator char(1)
AS
BEGIN
	set nocount on

    select
		[PriceBatchHeaderID] = 0,
		[Item_Key] = sq.Item_Key, 
		[Identifier] = sq.Identifier,
		[Store_No] = s.Store_No,
		[Business_Unit] = s.BusinessUnit_ID,
		[StartDate] = sysdatetime(),
		[SaleStartDate] = sq.Sale_Start_Date,
		[ItemChgTypeID] = null,
		[ItemChgTypeDesc] = null,
		[BatchDescription] = null,
		[TprBatchHasPriceChange] = 0
	from
		SignQueue						sq	(nolock)
		inner join	Store				s	(nolock)	on	s.Store_No				= sq.Store_No
		inner join dbo.fn_Parse_List(@ItemList, @ItemListSeparator) il
														on sq.Item_Key				= il.Key_Value
	where 
		sq.Store_No = @StoreNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueueDetailForItemKeys] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueueDetailForItemKeys] TO [IRSUser]
    AS [dbo];

