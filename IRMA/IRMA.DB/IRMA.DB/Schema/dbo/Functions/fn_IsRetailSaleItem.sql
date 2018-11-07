
CREATE FUNCTION [dbo].[fn_IsRetailSaleItem]
(
	@Item_Key int
)
RETURNS bit
AS

BEGIN
	declare @retVal bit

	if @Item_Key <= 0
		select @retVal = 0
	else
	begin
		if exists
			(
			SELECT top 1 i.Item_Key
			 FROM Item i (nolock)
			WHERE
				i.item_key = @Item_key
			  AND 
				i.Retail_Sale = 1
			)
			select @retVal = 1
		else
			select @retVal = 0
	end

	return @retVal
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailSaleItem] TO [IRSUser]
    AS [dbo];

