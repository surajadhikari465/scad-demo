SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemOrder]
GO


CREATE PROCEDURE dbo.GetItemOrder
@OrderItem_Id int
AS

SELECT RetailStore.Store_Name, Price.Multiple, Price.Price, OrderItem.UnitExtCost,
       CASE WHEN NoDistMarkup = 0 THEN ISNULL(ZoneSupply.Distribution_Markup, 0) ELSE 0 END AS DistributionMarkup, 
       ISNULL(ZoneSupply.CrossDock_Markup, 0) AS CrossDockMarkup, 
       RetailStore.Store_No
FROM ZoneSupply (NOLOCK) RIGHT JOIN (
       Store RetailStore (NOLOCK) INNER JOIN (
         Price INNER JOIN (
           Store FromStore (NOLOCK) INNER JOIN (
             Item INNER JOIN (
               OrderItem INNER JOIN (
                 OrderHeader INNER JOIN Vendor (NOLOCK) ON (Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID)
               ) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
             ) ON (OrderItem.Item_Key = Item.Item_Key)
           ) ON (Vendor.Store_No = FromStore.Store_No)
         ) ON (Price.Item_Key = Item.Item_Key)
       ) ON (RetailStore.Store_No = Price.Store_No)
     ) ON (ZoneSupply.FromZone_ID = FromStore.Zone_Id AND ZoneSupply.ToZone_ID = RetailStore.Zone_ID AND 
           ZoneSupply.SubTeam_No = Item.SubTeam_No)
WHERE OrderItem.OrderItem_ID = @OrderItem_ID AND (RetailStore.WFM_Store = 1 OR RetailStore.Mega_Store = 1) AND
      Price.Price > 0
ORDER BY RetailStore.Store_Name

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

