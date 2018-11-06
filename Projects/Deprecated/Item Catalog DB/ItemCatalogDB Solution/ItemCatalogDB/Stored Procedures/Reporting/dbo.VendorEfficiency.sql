SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VendorEfficiency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[VendorEfficiency]
GO


CREATE PROCEDURE dbo.VendorEfficiency
@SubTeam_No int
AS
--**************************************************************************
-- Procedure: VendorEfficiency
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
SELECT Vendor.CompanyName, Identifier, Item.Item_Description, 
       AVG(DateDiff(d, SentDate, datereceived)) AS Average_Days, 
       AVG(QuantityReceived) AS Average_Received, ItemUnit.Unit_Name, 
       AVG(QuantityReceived)/AVG(CASE WHEN QuantityOrdered = 0 THEN 0.1 ELSE QuantityOrdered END) * 100AS Percentage_Received, 
       COUNT(*) AS Number_Of_Orders
FROM ItemUnit (NOLOCK) INNER JOIN (
       ItemIdentifier (NOLOCK) INNER JOIN (
         Vendor (NOLOCK) INNER JOIN (
           Item (NOLOCK) INNER JOIN (
             OrderItem (NOLOCK) INNER JOIN OrderHeader (NOLOCK) ON (OrderItem.Orderheader_ID = Orderheader.Orderheader_ID)
           ) ON (Item.Item_Key = OrderItem.Item_Key)
         ) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
       ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
     ) ON (ItemUnit.Unit_ID = OrderItem.QuantityUnit)
WHERE Item.SubTeam_No = @SubTeam_No AND dbo.fn_GetDiscontinueStatus(Item.Item_Key,NULL, Vendor.Vendor_ID) = 0 AND PurchaseLocation_ID = 3360 AND (DateReceived IS NOT NULL) AND 
      SentDate > DateAdd(d,-90,GetDate()) AND Transfer_SubTeam IS NULL
GROUP BY Vendor.CompanyName, Identifier, Item.Item_Description, ItemUnit.Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

