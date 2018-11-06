CREATE PROCEDURE dbo.GetWarehouseItemChanges @Store_No INT
AS 
    BEGIN
        SET NOCOUNT ON
	--20090112 - DaveStacey Take Out Deletion of M changes due to fix in code
	
        SELECT  WarehouseItemChangeID,
                ChangeType,
                Item.Item_Key,
                Identifier,
                Item_Description,
                POS_Description,
                Item.SubTeam_No,
                SubTeam_Name,
                SubTeam_Abbreviation,
                VCH.Package_Desc1,
                Package_Desc2,
                IU.Unit_Abbreviation AS Package_Unit_Abbrev,
                Not_Available,
                Item.COOL,
                Item.BIO,
                Item.CatchWeightRequired
        FROM    dbo.WarehouseItemChange(NOLOCK)
                INNER JOIN dbo.Item(NOLOCK) ON Item.Item_Key = WarehouseItemChange.Item_Key
                LEFT JOIN dbo.ItemUnit AS IU ON item.Package_Unit_ID = iu.Unit_ID
                INNER JOIN dbo.SubTeam AS SubTeam ( NOLOCK ) ON SubTeam.SubTeam_No = Item.SubTeam_No
                INNER JOIN dbo.ItemIdentifier(NOLOCK) ON ItemIdentifier.Item_Key = Item.Item_Key
                                                         AND Default_Identifier = 1
                INNER JOIN dbo.StoreItemVendor(NOLOCK) ON StoreItemVendor.Item_Key = Item.Item_Key
                                                          AND StoreItemVendor.Store_No = @Store_No
                                                          AND StoreItemVendor.PrimaryVendor = 1
                INNER JOIN dbo.VendorCostHistory AS VCH ( NOLOCK ) ON VCH.StoreItemVendorId = StoreItemVendor.StoreItemVendorId
                                                                      AND VCH.VendorCostHistoryId = ( SELECT    MAX(VI.VendorCostHistoryId)
                                                                                                      FROM      dbo.VendorCostHistory VI ( NOLOCK )
                                                                                                      WHERE     VI.StoreItemVendorID = VCH.StoreItemVendorID
                                                                                                                AND VI.StartDate = ( SELECT MAX(VD.StartDate)
                                                                                                                                     FROM   VendorCostHistory VD ( NOLOCK )
                                                                                                                                     WHERE  VD.StoreItemVendorID = VCH.StoreItemVendorID
                                                                                                                                   )
                                                                                                    )
        WHERE   WarehouseItemChange.Store_No = @Store_No
                AND NOT EXISTS ( SELECT *
                                 FROM   dbo.WarehouseItemChange WIC
                                 WHERE  WIC.Item_Key = WarehouseItemChange.Item_Key
                                        AND WIC.Store_No = WarehouseItemChange.Store_No
                                        AND WIC.ChangeType = 'A'
                                        AND DATEDIFF(hour, WIC.InsertDate,
                                                     GETDATE()) < 1 )
        ORDER BY Item.Item_Key,
                WarehouseItemChange.InsertDate    
	
        SET NOCOUNT OFF
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseItemChanges] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseItemChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseItemChanges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseItemChanges] TO [IRMAReportsRole]
    AS [dbo];

