SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderSentToEmailDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderSentToEmailDate]
GO

CREATE PROCEDURE dbo.UpdateOrderSentToEmailDate 
    @OrderHeader_ID int
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/24/2011	Tom Lux			759		Added set of POCostDate (related to lead-time), since it's based on Sent Date.
										Created var for Sent Date and removed @CentralTimeZoneOffset.

	--------------------------------------------
*/

	DECLARE @SentDate datetime

	SELECT @SentDate = DATEADD(hour, CentralTimeZoneOffset, GETDATE())
	FROM Region

    UPDATE
		OrderHeader
    SET
		SentToEmailDate = @SentDate 
		,User_ID = null
		,SentDate = @SentDate
		,POCostDate = DATEADD(DAY, dbo.fn_GetLeadTimeDays(Vendor_ID), @SentDate)
    FROM
		OrderHeader (rowlock)
    WHERE
		OrderHeader_ID = @OrderHeader_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO