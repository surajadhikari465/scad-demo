SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF exists (SELECT * from dbo.sysobjects where id = object_id(N'[dbo].[fn_ReceivedItemsExist]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
	DROP FUNCTION [dbo].[fn_ReceivedItemsExist]
GO

CREATE FUNCTION [dbo].[fn_ReceivedItemsExist](@OrderHeader_ID INT)
-- **************************************************************************
--  Procedure: ReceivedItemsExist()
--     Author: Victoria Afonina, SoPac, 818-987-7617
--       Date: 04/03/2012
--
--  Description:
--  This function takes PO#, and checks if 
--  any item in the PO is received 
--  in order to close an order.
--  4.5.0 Bug # 4737

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 06/29/2012	BBB   	6795	Changed aggregation to look at DateReceived instead of Qty
-- **************************************************************************
   RETURNS BIT

BEGIN
   
   DECLARE @ItemsReceived AS DECIMAL (18,4);
   DECLARE @CanCloseOrder AS BIT;
    
   SELECT @CanCloseOrder = 0

   SELECT @ItemsReceived = COUNT(oi.DateReceived)
   FROM OrderItem (nolock) oi
   WHERE oi.orderHeader_id = @OrderHeader_ID
   AND oi.QuantityReceived IS NOT NULL
   AND oi.DateReceived IS NOT NULL

   IF (@ItemsReceived > 0) 
      SELECT @CanCloseOrder = 1
   
   RETURN (@CanCloseOrder);
END

GO


