SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOCostDifAuto]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOCostDifAuto]
GO


CREATE PROCEDURE dbo.GetPOCostDifAuto
    @StartDate datetime 
AS

BEGIN
    SET NOCOUNT ON

    SELECT 
        Final.FullName, 
        Final.Store_Name, 
        Final.Subteam_Name, 
        Final.OrderHeader_ID, 
        Final.CompanyName, 
        Final.Identifier, 
        Final.Item_Description, 
        Final.New_Item_Cost, 
        Final.Original_Item_Cost,
        (New_Item_Cost - Original_Item_Cost) AS Cost_Difference,  
        Final.Subteam_No 
    FROM 
        (SELECT
            OrderHeader.OrderHeader_ID,
            Store_Name, 
            ReceiveLocation.Store_No, 
            Subteam.Subteam_No, 
            Subteam.Subteam_Name, 
            FullName,  
            Vendor.CompanyName, 
            Identifier, 
            Item_Description,
            OrderItem.UnitCost AS New_Item_Cost, 
            ISNULL((SELECT TOP 1 UnitCost + UnitFreight
                    FROM VendorCostHistory VCH (nolock)
                    INNER JOIN
                        StoreItemVendor SIV (nolock)
                        ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
                    WHERE Promotional = 1
                        AND Item_Key = OrderItem.Item_Key AND Vendor_ID = Vendor.Vendor_ID AND Store_No = Store.Store_No 
                        AND ((OrderHeader.OrderDate >= StartDate) AND (OrderHeader.OrderDate < DATEADD(day, 1, CONVERT(datetime, EndDate))))
                        AND OrderHeader.OrderDate < ISNULL(DeleteDate, DATEADD(day, 1, OrderHeader.OrderDate))
                    ORDER BY VendorCostHistoryID DESC), 
                    ISNULL((SELECT TOP 1 UnitCost + UnitFreight
                            FROM VendorCostHistory VCH (nolock)
                            INNER JOIN
                                StoreItemVendor SIV (nolock)
                                ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
                            WHERE Promotional = 0
                                AND Item_Key = OrderItem.Item_Key AND Vendor_ID = Vendor.Vendor_ID AND Store_No = Store.Store_No 
                                AND ((OrderHeader.OrderDate >= StartDate) AND (OrderHeader.OrderDate < DATEADD(day, 1, CONVERT(datetime, EndDate))))
                                AND OrderHeader.OrderDate < ISNULL(DeleteDate, DATEADD(day, 1, OrderHeader.OrderDate))
                            ORDER BY VendorCostHistoryID DESC), 0)) Original_Item_Cost 
        FROM OrderHeader (nolock)
        INNER JOIN 
            OrderItem (nolock)
            ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
        INNER JOIN 
            Item (nolock)
            ON Item.Item_Key = OrderItem.Item_Key 
        INNER JOIN 
            ItemIdentifier (nolock)
            ON ItemIdentifier.Item_Key = Item.Item_Key AND Default_Identifier = 1
        INNER JOIN 
            Vendor ReceiveLocation (nolock)
            ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID 
        INNER JOIN 
            Store (nolock)
            ON ReceiveLocation.Store_No = Store.Store_No
        INNER JOIN 
            Vendor (nolock)
            ON Vendor.Vendor_ID = OrderHeader.Vendor_id
        INNER JOIN 
            Users (nolock)
            ON Users.User_Id = OrderHeader.CreatedBy
    	INNER JOIN 
            SubTeam (nolock)
            ON SubTeam.SubTeam_No = Item.SubTeam_No
        WHERE CloseDate > DATEADD(d,-7,@StartDate)and CloseDate <= @StartDate
        AND Transfer_Subteam IS NULL 
        AND Return_Order = 0
        ) Final 
    WHERE (Final.New_Item_Cost > Final.Original_Item_Cost) AND (Final.Original_Item_Cost > 0)
    ORDER BY Final.Subteam_No

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


