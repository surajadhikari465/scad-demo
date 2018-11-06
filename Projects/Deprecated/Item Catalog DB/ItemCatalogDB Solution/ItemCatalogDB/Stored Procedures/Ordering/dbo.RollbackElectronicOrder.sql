 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.RollbackElectronicOrder') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.RollbackElectronicOrder
GO

CREATE PROCEDURE dbo.RollbackElectronicOrder
    @OrderHeader_ID int
AS

BEGIN
    SET NOCOUNT ON

   UPDATE OrderHeader 
   SET Sent = 0, User_Id = NULL
   WHERE OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  