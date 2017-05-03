SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].UpdateOrderResetWarehouseSent') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].UpdateOrderResetWarehouseSent
GO


CREATE PROCEDURE dbo.UpdateOrderResetWarehouseSent
	@OrderHeader_ID		int,
	@CancelOrder		bit
AS

-- **************************************************************************
-- Procedure: UpdateOrderResetWarehouseSent
--    Author: n/a
--      Date: n/a
--
-- Description: @CancelOrder = 1 when EXE sends a 6001 file to IRMA for processing.
--				@CancelOrder = 0 when a user clicks the "Undo Warehouse Send" button in the IRMA client.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2009/07/07	RE		9286	EXE to IRMA Cancelled Orders (6001 files)
-- 2011/12/19	KM		3744	Added update history template; extension change; coding standards
-- **************************************************************************
BEGIN
	SET NOCOUNT ON
	
	DECLARE 
		@error_no  int,
		@row_cnt   int
	
	SELECT
		@error_no	= 0,
		@row_cnt	= 0
	
	UPDATE
		OrderHeader
	SET
		WarehouseSentDate	=	NULL,
	    WarehouseSent		=	0,
	    CloseDate			=	CASE 
									WHEN @CancelOrder = 0 THEN CloseDate
									ELSE dbo.fn_GetSystemDateTime() 
								END,
		WarehouseCancelled	=	CASE 
									WHEN @CancelOrder =1 THEN GETDATE() 
									ELSE WarehouseCancelled 
								END
	WHERE  
		OrderHeader_ID = @OrderHeader_ID
		AND CloseDate IS NULL
	
	SELECT
		@error_no = @@ERROR,
		@row_cnt = @@ROWCOUNT
			   
	IF @CancelOrder = 1
		BEGIN
			UPDATE 
				OrderItem
			SET 
				QuantityReceived	= NULL,
				ReceivedItemCost	= 0
			WHERE
				OrderHeader_ID = @OrderHeader_ID 
	
	
			SELECT
				@error_no = @@ERROR,
				@row_cnt = @@ROWCOUNT
		END
	
	SET NOCOUNT OFF
	
	IF	@error_no = 0
		AND @row_cnt = 0
		BEGIN
	    
	    -- CloseDate is not null, so cannot be reset
	    RAISERROR(50004, 16, 1)
	    RETURN
	END
	
	IF @error_no <> 0
		BEGIN
	    IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
	    
	    DECLARE @Severity SMALLINT
	    SELECT @Severity = ISNULL(
	               (
	                   SELECT severity
	                   FROM   MASTER.dbo.sysmessages
	                   WHERE  ERROR = @error_no
	               ),
	               16
	           )
	    
	    RAISERROR (
	        'UpdateOrderResetWarehouseSent failed with @@ERROR: %d',
	        @Severity,
	        1,
	        @error_no
	    )
	END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

