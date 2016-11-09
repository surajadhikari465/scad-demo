CREATE PROCEDURE dbo.[UpdatePromoItemRev] 
	@salPr decimal(8,4),
    @salCo decimal(8,4),
	@ID int,
    @comment varchar(50),
    @qty int,
	@key int,
	@store int,
	@endDt smalldatetime,
	@strtDt smalldatetime

AS
BEGIN
	
	SET NOCOUNT ON;

update 
PriceBatchPromo  
set 
comment1 = @comment, 
Sale_Price=@salPr,
Sale_Cost=@salCo, 
projunits = @qty 
where 
PriceBatchPromoID = @ID;

update 
PriceBatchDetail set Sale_Price= @salPr, POSSale_Price= @salPr
where 
Item_Key = @key and 
StartDate = @strtDt and 
Sale_End_Date = @endDt and 
Store_No = @store;

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePromoItemRev] TO [IRMAPromoRole]
    AS [dbo];

