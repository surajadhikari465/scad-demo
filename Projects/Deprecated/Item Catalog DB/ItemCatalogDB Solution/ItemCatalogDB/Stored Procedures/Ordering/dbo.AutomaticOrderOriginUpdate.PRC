SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AutomaticOrderOriginUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AutomaticOrderOriginUpdate]
GO


CREATE PROCEDURE dbo.AutomaticOrderOriginUpdate
@OrderHeader_ID int
AS 

UPDATE Item
Set Item.Origin_ID = T1.Origin_ID
FROM Item INNER JOIN (
       SELECT OrderItem.Item_Key, MAX(OrderItem.Origin_ID) AS Origin_ID
       FROM OrderItem INNER JOIN ItemOrigin ON (OrderItem.Origin_ID = ItemOrigin.Origin_ID)
       WHERE OrderItem.OrderHeader_ID = @OrderHeader_ID AND OrderItem.QuantityReceived > 0 
       GROUP BY OrderItem.Item_Key
     ) T1 ON (Item.Item_Key = T1.Item_Key)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


