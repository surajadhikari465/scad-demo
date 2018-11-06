
CREATE PROCEDURE [dbo].[IsRetailSaleItem] 
	@ItemKey int,
	@IsRetailSaleItem bit OUTPUT
AS 
BEGIN

    SET @IsRetailSaleItem = 
		(SELECT
			i.Retail_Sale
		FROM
			Item i
		WHERE
			i.Item_Key = @ItemKey)

	SELECT @IsRetailSaleItem

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsRetailSaleItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsRetailSaleItem] TO [IRSUser]
    AS [dbo];

