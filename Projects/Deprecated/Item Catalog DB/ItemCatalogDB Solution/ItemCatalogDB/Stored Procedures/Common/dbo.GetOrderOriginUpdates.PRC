SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderOriginUpdates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderOriginUpdates]
GO


CREATE PROCEDURE dbo.GetOrderOriginUpdates
@OrderHeader_ID int
AS 

SELECT Item_Key, MAX(Origin_ID) AS Origin_ID
FROM OrderItem
WHERE OrderHeader_ID = @OrderHeader_ID AND (Origin_ID IS NOT NULL) AND QuantityReceived > 0
GROUP BY Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


