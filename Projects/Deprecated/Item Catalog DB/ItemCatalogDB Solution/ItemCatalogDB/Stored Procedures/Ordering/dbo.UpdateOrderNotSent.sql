SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderNotSent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderNotSent]
GO

CREATE PROCEDURE dbo.UpdateOrderNotSent  
	@OrderHeader_ID int
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/24/2011	Tom Lux			759		Added reset of POCostDate (related to lead-time), since it's based on Sent Date.
*/
BEGIN
    SET NOCOUNT ON
    
    UPDATE
		OrderHeader
    SET
		SentDate = NULL
		,POCostDate = NULL
    WHERE
		OrderHeader_ID = @OrderHeader_ID
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

