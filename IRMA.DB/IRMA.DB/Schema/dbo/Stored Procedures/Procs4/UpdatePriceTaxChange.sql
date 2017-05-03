CREATE PROCEDURE dbo.[UpdatePriceTaxChange]
    @Item_Key int,
    @Store_No int,
    @Price smallmoney,
    @Sale_Price smallmoney
AS

UPDATE	PRICE
SET		Price = @Price, 
		Sale_Price = @Sale_Price
WHERE	Item_Key = @Item_Key	
		AND Store_No = Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePriceTaxChange] TO [IRMAClientRole]
    AS [dbo];

