SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetOrderSentDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetOrderSentDate]
GO


CREATE PROCEDURE dbo.SetOrderSentDate 
@OrderHeader_ID int,
@SentDate smalldatetime
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/18/2011	Tom Lux			759		Added update of POCostDate (related to lead-time), since it's based on Sent Date.

	--------------------------------------------
*/
BEGIN


	UPDATE
		OrderHeader
	SET
		SentDate = @SentDate
		,POCostDate = dateadd(d, dbo.fn_GetLeadTimeDays(OrderHeader.Vendor_ID), convert(date, @SentDate, 101))
	FROM
		OrderHeader (rowlock)
	WHERE
		OrderHeader_ID = @OrderHeader_ID 

END
GO
