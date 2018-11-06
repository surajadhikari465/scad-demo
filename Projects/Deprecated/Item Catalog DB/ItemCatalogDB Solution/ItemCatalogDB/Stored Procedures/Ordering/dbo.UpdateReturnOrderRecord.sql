SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateReturnOrderRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateReturnOrderRecord]
GO


CREATE PROCEDURE dbo.UpdateReturnOrderRecord (
    @Instance        INT,
    @User_ID         INT,
    @OrderItem_ID    INT,
    @Quantity        MONEY,
    @Total_Weight    MONEY,
    @CreditReason_ID INT)
AS
BEGIN
    IF @CreditReason_ID = -1
        SET @CreditReason_ID = (SELECT CreditReason_ID 
                                FROM   ReturnOrder 
                                WHERE  [Instance]   = @Instance AND
                                       [User_ID]    = @User_ID  AND
                                       OrderItem_ID = @OrderItem_ID)
    
    UPDATE ReturnOrder
    SET    Quantity        = @Quantity,
           Total_Weight    = @Total_Weight,
           CreditReason_ID = @CreditReason_ID
    WHERE  Instance        = @Instance AND
           [User_ID]       = @User_ID  AND
           OrderItem_ID    = @OrderItem_ID
END

GO