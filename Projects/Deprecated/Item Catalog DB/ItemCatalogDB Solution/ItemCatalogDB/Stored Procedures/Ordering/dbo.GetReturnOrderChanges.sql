if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReturnOrderChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetReturnOrderChanges]
GO

CREATE PROCEDURE dbo.GetReturnOrderChanges (
@Instance INT,
@User_ID  INT)
AS
BEGIN
    SELECT * 
    FROM   ReturnOrder 
    WHERE ReturnOrder.[Instance] = @Instance AND
          ReturnOrder.[User_ID]  = @User_ID  AND
          (Quantity > 0 OR Total_Weight > 0)
END
GO

