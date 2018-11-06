SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBackOrderItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBackOrderItems]
GO


CREATE PROCEDURE dbo.GetBackOrderItems
@OrderHeader_ID int
AS

SELECT OrderItem.Item_key, (OrderItem.QuantityOrdered - OrderItem.QuantityReceived) AS Quantity, OrderItem.QuantityUnit
FROM Item INNER JOIN OrderItem ON (Item.Item_Key = OrderItem.Item_Key) 
WHERE (OrderItem.QuantityOrdered > OrderItem.QuantityReceived) AND OrderItem.OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

