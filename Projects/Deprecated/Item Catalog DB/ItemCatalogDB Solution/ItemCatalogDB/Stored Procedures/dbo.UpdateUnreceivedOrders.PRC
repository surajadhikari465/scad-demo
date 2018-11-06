SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateUnreceivedOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateUnreceivedOrders]
GO


CREATE PROCEDURE dbo.UpdateUnreceivedOrders 
@OrderHeader_ID int
AS 

UPDATE OrderItem 
SET LineItemCost = 0,
    LineItemHandling = 0,
    LineItemFreight = 0

WHERE OrderHeader_ID = @OrderHeader_ID AND QuantityReceived = 0


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

