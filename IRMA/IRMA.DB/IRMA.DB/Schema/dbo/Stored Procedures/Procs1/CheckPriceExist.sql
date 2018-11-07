CREATE PROCEDURE dbo.CheckPriceExist
	@Identifier varchar(20),
    @StoreList varchar(2000),
    @ListSeparator char(1)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT count(*) PriceCnt
    FROM Price
        INNER JOIN
            ItemIdentifier
            on ItemIdentifier.Item_key = Price.Item_Key
        INNER JOIN
            dbo.FN_Parse_List(@StoreList, @ListSeparator) Stores
            on Stores.Key_Value = Price.Store_no 
    WHERE Price.Price > 0 and ItemIdentifier.Identifier = @Identifier     
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckPriceExist] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckPriceExist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckPriceExist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckPriceExist] TO [IRMAReportsRole]
    AS [dbo];

