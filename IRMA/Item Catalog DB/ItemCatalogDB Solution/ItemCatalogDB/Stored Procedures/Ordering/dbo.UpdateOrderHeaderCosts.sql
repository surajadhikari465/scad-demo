SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderHeaderCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[UpdateOrderHeaderCosts]
GO

CREATE PROCEDURE dbo.UpdateOrderHeaderCosts
	@OrderHeader_ID INT
AS
-- **************************************************************************
-- Procedure: UpdateOrderHeaderCosts()
--    Author: Min Zhao
--      Date: 01.09.12
--
-- Description:
-- This procedure is called from multiple locations within IRMA client to update
-- ordered costs as needed.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 01/12/2012	BBB   			4242	Added additional orderheader costs to replace
--										extra calls to UORC;
-- 2012/01/25	KM				4484	Added TotalPaidCost to Update set;
-- 2012/01/31	KM				4484	Make correction to TotalPaidCost update calculation;
-- 2012/10/23	RDE				7430	Removed NULLIF() clause for PaidCost. If PaidCost is 0 we want to leave it as is.
-- 2013/03/12	FA				8325	Added code to update TotalRefused
-- 2013/04/16	KM				11974	Disable enhanced refusal functionality;
-- **************************************************************************

BEGIN
	
	-- 4.8 Disable enhanced refusal functionality until final requirements are delivered.
	--DECLARE @TotalRefusedAmount as decimal(18,4)
	
	--SELECT @TotalRefusedAmount = 0.0
	--EXEC dbo.GetTotalRefused @OrderHeader_ID, @TotalRefusedAmount output
	
	UPDATE 
		OrderHeader
	SET
		OrderedCost				=	ISNULL((SELECT SUM(LineItemCost) FROM OrderItem WHERE OrderHeader_ID = @OrderHeader_ID), 0),
		AdjustedReceivedCost	=	ISNULL((SELECT SUM(ReceivedItemCost) FROM OrderItem WHERE OrderHeader_ID = @OrderHeader_ID), 0),
		OriginalReceivedCost	=	ISNULL((SELECT SUM(OrigReceiveditemCost * ISNULL(QuantityReceived, 0)) FROM OrderItem WHERE OrderHeader_ID = @OrderHeader_ID), 0),
		TotalPaidCost			=	(
										SELECT
											SUM(ISNULL(ISNULL(oi.PaidCost, oi.ReceivedItemCost), 0))
										FROM
											OrderItem (nolock) oi
										WHERE
											oi.OrderHeader_ID = OrderHeader.OrderHeader_ID
									)
		--TotalRefused			= @TotalRefusedAmount
									
	WHERE
		OrderHeader_ID = @OrderHeader_ID
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO