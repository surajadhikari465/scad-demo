create procedure dbo.EXEInterfaces_ZeroReceiveAndCloseOrder
@OrderHeader_Id int
AS
BEGIN


/*
	Get All NON Received Items from an order and zero receive them. 
	Close Order if all items on the order have been received in some way. (0 or otherwise. NOT NULL)
*/

	declare @RowCnt int
	declare @MaxCnt int
	declare @CurrentOrderItem int
	declare @FoundError int 
	declare @ReceivedDate datetime
	declare @IsSuccessful as varchar(5)
	set @IsSuccessful = 'false'

	set @ReceivedDate = getdate()
	set @FoundError = 0

	DECLARE @OrderItems TABLE 
	(
		RowNum int IDENTITY (1, 1) Primary key NOT NULL , 
		OrderItemId int	
	)

	Select @RowCnt = 1
	
	Insert Into @OrderItems
	select	OrderItem_Id 
	from	OrderItem 
	where	OrderHeader_Id = @OrderHeader_ID and 
			quantityreceived is null 
	order by orderitem_id 

	select @MaxCnt = count(*) from @OrderItems

	while @RowCnt <= @MaxCnt
	begin
		select  @CurrentOrderItem = OrderItemId from @OrderItems where rownum = @rowCnt
		print  'processing ' + cast(@CurrentOrderItem as varchar(100))
		begin try
			exec receiveOrderitem4 @CurrentOrderItem, @ReceivedDate,0,0,null,0
		end try
		begin catch
			set @FoundError =1 
		end catch

		select @RowCnt = @RowCnt + 1
	end
	if @FoundError = 0
	begin
		-- only close the orders if all items were received with out error.
		-- and all items for the order have been received.
		print 'closing'
		
		if not exists (select OrderItem_Id from OrderItem where OrderHeader_Id = @OrderHeader_Id and QuantityReceived is null)
		begin
			exec updateorderclosed @OrderHeader_ID,0
			set @IsSuccessful = 'true'
		end
	end
	
	select @IsSuccessful as ReturnValue
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroReceiveAndCloseOrder] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroReceiveAndCloseOrder] TO [IRMAClientRole]
    AS [dbo];

