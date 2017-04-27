SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderItemComments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderItemComments]
GO


CREATE PROCEDURE dbo.UpdateOrderItemComments
@OrderItem_ID int, 
@Comments varchar(255)
AS

UPDATE OrderItem 
SET Comments = @Comments
WHERE OrderItem_ID = @OrderItem_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


