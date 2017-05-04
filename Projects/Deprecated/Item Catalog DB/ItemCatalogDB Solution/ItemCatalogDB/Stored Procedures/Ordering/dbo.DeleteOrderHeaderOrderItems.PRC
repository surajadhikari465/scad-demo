SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteOrderHeaderOrderItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteOrderHeaderOrderItems]
GO


CREATE PROCEDURE dbo.DeleteOrderHeaderOrderItems 
@OrderHeader_ID int 
AS 

DELETE 
FROM OrderItem
WHERE OrderHeader_ID = @OrderHeader_ID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


