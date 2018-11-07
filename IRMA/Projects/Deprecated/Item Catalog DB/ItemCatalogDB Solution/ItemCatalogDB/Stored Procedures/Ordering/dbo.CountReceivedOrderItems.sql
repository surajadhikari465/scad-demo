SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CountReceivedOrderItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CountReceivedOrderItems]
GO

CREATE PROCEDURE dbo.CountReceivedOrderItems

@OrderHeader_ID int 

AS 
-- ****************************************************************************************************************
-- Procedure: CountReceivedOrderItems()
--    Author: Faisal Ahmed
--      Date: 02/01/2013
--
-- Description:
-- Returns the count of the number of items on an order that does not have a zero or null received quantity.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 02/01/2013	FA		9805	Initial Version
-- ****************************************************************************************************************

SELECT 
	COUNT(*) AS ReceivedOrderItemCount 
FROM 
	OrderItem 
WHERE 
	OrderHeader_ID = @OrderHeader_ID AND QuantityReceived IS NOT NULL AND QuantityReceived <> 0 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO