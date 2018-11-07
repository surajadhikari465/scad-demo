CREATE PROCEDURE dbo.ItemCatalogExport 
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @StoreItem TABLE (Store_No int, Item_Key int, SubTeam_No int, AvgCost money, Price smallmoney, Multiple tinyint)

    INSERT INTO @StoreItem (Store_No, Item_Key, SubTeam_No, Price, Multiple)
    SELECT Store_No, Item.Item_Key, Item.SubTeam_No, Price, Multiple
    FROM
        Item (nolock)
    INNER JOIN
        Price (nolock)
        ON (Price.Item_Key = Item.Item_Key)
    INNER JOIN
        SubTeam (nolock)
        ON (Item.SubTeam_No = SubTeam.SubTeam_No)
    WHERE (Shipper_Item = 0 AND Deleted_Item = 0)
        AND Price.Store_No IN (106, 108)
        AND SubTeam.SubTeamType_ID IN (1, 3) 

    UPDATE @StoreItem
    SET AvgCost = (SELECT TOP 1 AvgCost
                   FROM AvgCostHistory (nolock)
                   WHERE Item_Key = SI.Item_Key
                       AND Store_No = SI.Store_No
                       AND SubTeam_No = SI.SubTeam_No
                   ORDER BY Effective_Date DESC)
    FROM @StoreItem SI

    UPDATE @StoreItem
    SET AvgCost = (SELECT TOP 1 (ReceivedItemCost + ReceivedItemFreight) / UnitsReceived
                   FROM OrderItem OI (nolock)
                   INNER JOIN
                       OrderHeader OH (nolock)
                       ON OH.OrderHeader_ID = OI.OrderHeader_ID
                   INNER JOIN
                       Vendor StoreVend (nolock)
                       ON OH.ReceiveLocation_ID = StoreVend.Vendor_ID
                   WHERE OI.Item_Key = SI.Item_Key AND StoreVend.Store_No = SI.Store_No AND OH.Transfer_To_SubTeam = SI.SubTeam_No
                      AND Return_Order = 0
                      AND UnitsReceived > 0
                   ORDER BY DateReceived DESC)
    FROM @StoreItem SI
    WHERE AvgCost IS NULL

    UPDATE @StoreItem
    SET AvgCost = CASE WHEN Multiple > 0 THEN Price / Multiple ELSE Price END * ISNULL(CostFactor, 0)
    FROM @StoreItem SI
    INNER JOIN
        StoreSubTeam (nolock)
        ON StoreSubTeam.Store_No = SI.Store_No AND StoreSubTeam.SubTeam_No = SI.SubTeam_No
    WHERE AvgCost IS NULL

    SELECT Store.BusinessUnit_ID, Identifier, POS_Description, SUBSTRING ('000000', 1, 6 - LEN(CAST(Item.ITEM_KEY AS VARCHAR(6)))) + CAST(Item.ITEM_KEY AS VARCHAR(6))  AS Item_Key, 
		SUBSTRING ('000000', 1, 6 - LEN (CONVERT(varchar(6), AvgCost, 0))) +  CONVERT(varchar(6), AvgCost, 0) As AvgCost,
		CAST(Item.SubTeam_No AS Varchar(4)) AS SubTeam_No
    FROM
        Item (nolock)
    INNER JOIN
        ItemIdentifier (nolock)
        ON (Item.Item_Key = ItemIdentifier.Item_Key)
    INNER JOIN
        @StoreItem SI
        ON SI.Item_Key = Item.Item_Key
    INNER JOIN
        Store (nolock)
        ON (Store.Store_No = SI.Store_No)
    ORDER BY Store.BusinessUnit_ID, Item.Item_Key

    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemCatalogExport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemCatalogExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemCatalogExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemCatalogExport] TO [IRMAReportsRole]
    AS [dbo];

