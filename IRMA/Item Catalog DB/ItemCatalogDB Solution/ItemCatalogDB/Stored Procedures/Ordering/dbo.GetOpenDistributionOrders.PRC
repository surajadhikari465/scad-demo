SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOpenDistributionOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOpenDistributionOrders]
GO


CREATE PROCEDURE dbo.GetOpenDistributionOrders 
@Item_Key int,
@Store_No int
AS 
SELECT OrderDate, 
       QuantityOrdered, 
       QuantityUnit
FROM Orderitem INNER JOIN OrderHeader ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
WHERE Item_Key = @Item_Key AND 
      CloseDate IS NULL AND 
      DateReceived IS NULL AND
      (Transfer_SubTeam IS NOT NULL) AND
      OrderHeader.ReceiveLocation_ID IN (SELECT Vendor_ID FROM Vendor WHERE Store_No = @Store_No)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


