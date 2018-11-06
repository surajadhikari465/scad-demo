/****** Object:  StoredProcedure [dbo].[UpdateOrderClosed]    Script Date: 06/29/2012 11:46:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateOrderClosed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateOrderClosed]
GO

/****** Object:  StoredProcedure [dbo].[UpdateOrderClosed]    Script Date: 06/29/2012 11:46:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateOrderClosed] 
	@OrderHeader_ID		int,
	@User_ID			int,
	@IsSuspended		bit = 0 OUTPUT
AS

-- ********************************************************************************************************
-- Procedure: UpdateOrderClosed
--    Author: n/a
--      Date: n/a
--
-- Description:
-- 
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2008/11/24	RS				new based on FL DC requirements - taken from SO version 
--								EXEC UpdateOrderRefreshCosts @OrderHeader_ID, @User_ID, @IsSuspended OUTPUT
-- 2009/03/31	MU				we only want to refresh costs for an EXE facility
-- 2009/06/26	MD				Commented the code that limits call to the UpdateOrderRefreshCosts stored procedure
--								as with P2P we will need to call it everytime, this call is made after closing the PO
-- 2009/06/26	MD				The matching logic has been moved to a new stored procedure MatchOrderInvoiceCosts as it needs to be called from multiple places.
--								This new stored procedure will be called from UpdateOrderRefreshCosts so will be executed for UpdateOrderClosed from there.
-- 2009/10/30			11432	Sequence for setting closed status and calling UpdateOrderRefreshCosts was flipped so
--								UORC is now called first and then closed status is set.
-- 2009/11/03	TL		11430	The MatchOrderInvoiceCosts call was moved from the UpdateOrderRefreshCosts to here, UpdateOrderClosed, because it must be called after
--								OrderHeader.CloseDate is set and the above was resequenced to fix a DC-related bug, creating the need to move the inv-matching call here.
-- 2011/12/12  	KM		3744 	Added update history template; changed file extension to .sql;
-- 2011/12/27	KM		3744	Coding standards; usage review;
-- 2012/06/29	TD		6664	Changed 'OriginalCloseDate	= @Date' to 
--								'OriginalCloseDate	= (CASE WHEN OriginalCloseDate IS NULL THEN GETDATE() ELSE OriginalCloseDate END)'
--								So that CloseReceiving will list closed orders in the correct sequence 
-- 2012/10/10	BS		8133	Added varchar parameter 'UpdateOrderClosed' for call to UpdateOrderRefreshCost stored procedure
--								This is to track from where UORC is called.
-- 2013/03/18	DN		9844	Added Auto Populate 0 Quantity Functionality after closing POs to IRMA Receiving in IRMA Mobile
-- 2013/05/29	KM		12468	Refine auto-populate 0 logic so that it doesn't affect partial shipments;
-- ********************************************************************************************************

BEGIN
	SET NOCOUNT ON
	
	DECLARE 
		@Date		datetime,
		@Error_No	int

	SELECT @Error_No	= 0
	SELECT @Date		= MIN(DateReceived) FROM dbo.OrderItem WHERE OrderHeader_ID = @OrderHeader_ID
	SELECT @Error_No	= @@ERROR

	BEGIN TRAN
	
		IF @Error_No = 0
			BEGIN
				EXEC UpdateOrderRefreshCosts @OrderHeader_ID, 'UpdateOrderClosed', @User_ID, @IsSuspended OUTPUT
			END
	
		-- Move the order to the closed status.
		IF @Error_No = 0
			BEGIN
				UPDATE 
					dbo.OrderHeader
				SET 
					CloseDate			= @Date,
					OriginalCloseDate	= (CASE WHEN OriginalCloseDate IS NULL THEN GETDATE() ELSE OriginalCloseDate END),
					ClosedBy			= @User_ID
				WHERE 
					OrderHeader_ID		= @OrderHeader_ID

				-- When an order is closed, if it's not a partial shipment, the received quantity should be set to zero.  Received quantity remains 
				-- NULL only if the order is closed as a partial shipment.
				UPDATE oi SET
					oi.QuantityReceived = 0
				FROM
					OrderItem oi
					JOIN OrderHeader oh ON oi.OrderHeader_ID = oh.OrderHeader_ID
				WHERE
					oi.OrderHeader_Id = @OrderHeader_ID AND 
					oi.QuantityReceived IS NULL AND
					oh.PartialShipment = 0
	
				SELECT @Error_No = @@ERROR
			END
	
			DECLARE
				@CloseDate		datetime,
				@ApprovedDate	smalldatetime

			-- Get close and approved dates for MatchOrderInvoiceCosts condition.
			SELECT
				@CloseDate		= CloseDate,
				@ApprovedDate	= ApprovedDate

			FROM 
				OrderHeader OH (nolock)

			WHERE
				OH.OrderHeader_ID = @OrderHeader_ID

		IF @Error_No = 0
			BEGIN
	
			/* Check for the following three conditions: 			
				-- Ensure that the PO is closed (closedate not null)
				-- Ensure that the PO is not approved (approved date is null)
				-- DN we do not need to look for einvoiceid WI 10549
			*/
		
			IF (@CloseDate IS NOT NULL) AND (@ApprovedDate IS NULL)
				BEGIN
					EXEC MatchOrderInvoiceCosts @OrderHeader_ID, @User_ID, @IsSuspended OUTPUT
				END
			END

	IF @Error_No = 0
		BEGIN
			COMMIT TRAN
			SET NOCOUNT OFF
		END
	ELSE
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT	@Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			SET NOCOUNT OFF
			RAISERROR ('UpdateOrderClosed failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
END
GO