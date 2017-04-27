SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderItemQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderItemQueue]
GO


CREATE PROCEDURE dbo.UpdateOrderItemQueue 
    @OrderItemQueue_ID int,
    @Quantity decimal,
    @CreditReason_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE OrderItemQueue
    SET Quantity = @Quantity, CreditReason_ID = @CreditReason_ID, Insert_Date = GETDATE()
    WHERE OrderItemQueue_ID = @OrderItemQueue_ID 
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

