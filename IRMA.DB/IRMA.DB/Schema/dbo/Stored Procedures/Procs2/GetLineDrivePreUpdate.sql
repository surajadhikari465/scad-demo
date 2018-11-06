CREATE PROCEDURE dbo.GetLineDrivePreUpdate
    @Family varchar(255),
    @Brand_ID int,
    @Percent decimal(10,4),
    @StoreList varchar(8000),
    @StoreListSeparator char(1)
AS

BEGIN
    SET NOCOUNT ON

    -- NOTE: The following SELECT is repeated (except the SELECT clause) in stored procedure UpdateLineDrive
    -- The inner part of the Round (FixedLineDrive) is as follows: X + (0.09 - (((X * 10) - FLOOR(X * 10)) / 10)) where X = Round(Price - (Price * @percent), 2)
    SELECT Identifier, Brand_Name, Item_Description, isnull(Price.ExceptionSubteam_No, Item.SubTeam_No) as Subteam_No, Price.Store_No, Store_Name,
           Price, 
           Round(Price - (Price * @Percent), 2) As OrigLineDrive, 
           Round(Round(Price - (Price * @Percent), 2) + (0.09 - (Round(Price - (Price * @Percent), 2) * 10 - FLOOR(Round(Price - (Price * @Percent), 2) * 10)) / 10), 2) As FixedLineDrive
    FROM ItemIdentifier II (nolock)
    INNER JOIN Item (nolock) ON Item.Item_Key = II.Item_Key
    INNER JOIN ItemBrand (nolock) ON ItemBrand.Brand_ID = Item.Brand_ID
    INNER JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key
    INNER JOIN dbo.fn_Parse_List(@StoreList, @StoreListSeparator) S ON S.Key_Value = Price.Store_No
    INNER JOIN Store (nolock) ON S.Key_Value = Store.Store_No
    WHERE (Identifier LIKE @Family + '%') AND (LEN(Identifier) >= 10)
        AND Deleted_Item = 0
        AND Item.Brand_ID = ISNULL(@Brand_ID, Item.Brand_ID)
--        AND Price.PriceChgTypeID = dbo.fn_LineDriveType() 
-- Probably not necessary - it used to be EDLP = 0, but that was when we couldn't have overlapping sales.  
    ORDER BY Identifier, Store_Name

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLineDrivePreUpdate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLineDrivePreUpdate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLineDrivePreUpdate] TO [IRMAReportsRole]
    AS [dbo];

