SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemComments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemComments]
GO


CREATE PROCEDURE dbo.GetOrderItemComments
@OrderItem_ID int 
AS 

SELECT Comments 
FROM OrderItem 
WHERE OrderItem_ID = @OrderItem_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

