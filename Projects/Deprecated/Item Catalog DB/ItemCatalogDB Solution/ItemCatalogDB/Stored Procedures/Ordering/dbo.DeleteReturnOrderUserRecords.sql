if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteReturnOrderUserRecords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteReturnOrderUserRecords]
GO

CREATE PROCEDURE dbo.DeleteReturnOrderUserRecords (
@Instance INT,
@User_ID  INT)
AS
BEGIN
    DELETE FROM ReturnOrder
    WHERE ReturnOrder.[Instance] = @Instance AND
          ReturnOrder.[User_ID]  = @User_ID
END
GO
