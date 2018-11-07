CREATE PROCEDURE dbo.[UpdatePromoItem] 
	@salPr decimal(8,4),
    @salCo decimal(8,4),
	@ID int,
    @comment varchar(50),
    @qty int
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
PriceBatchPromoID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePromoItem] TO [IRMAPromoRole]
    AS [dbo];

