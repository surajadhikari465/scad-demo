CREATE PROCEDURE dbo.BRC_ReceiveOrder
@OrderHeader_ID int,
@User_ID int
as 
begin

	declare @Items table(ItemsID int identity(1,1), OrderItem_ID int, Quantity decimal(18,4), Weight decimal(18,4), User_ID int)

	insert into @Items(OrderItem_ID, Quantity, Weight, User_ID)
	select 
	OrderItem_ID
	, QuantityReceived
	, case when QuantityReceived is null then null else Total_Weight end as Total_Weight
	, @User_ID
	from OrderItem
	where OrderHeader_ID = @OrderHeader_ID

	declare @record_counter int
	declare @loop_counter int
	set @loop_counter = isnull((select count(*) from @Items),0)
	set @record_counter = 1

    while @loop_counter > 0 AND @record_counter <= @loop_counter
		begin 
		declare @OrderItem_ID int
		declare @Today datetime
		declare @Quantity decimal(18,4)
		declare @Weight decimal(18,4)

		select @OrderItem_ID = OrderItem_ID, @Today = getdate(), @Quantity = Quantity, @Weight = Weight 
		from @Items where @record_counter = ItemsID

		exec ReceiveOrderItem4 @OrderItem_ID, @Today, @Quantity, @Weight, null, @User_ID

		set @record_counter = @record_counter + 1
		end

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BRC_ReceiveOrder] TO [IRMAClientRole]
    AS [dbo];

