SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderSentToFaxDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderSentToFaxDate]
GO


CREATE PROCEDURE dbo.UpdateOrderSentToFaxDate 
    @OrderHeader_ID int
AS

	DECLARE @CentralTimeZoneOffset int

	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

    UPDATE OrderHeader
    SET SentToFaxDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE())
    FROM OrderHeader (rowlock)
    WHERE OrderHeader_ID = @OrderHeader_ID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


