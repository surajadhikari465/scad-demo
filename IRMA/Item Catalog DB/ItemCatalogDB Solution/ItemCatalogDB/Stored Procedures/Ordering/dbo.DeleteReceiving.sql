SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].DeleteReceiving') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].DeleteReceiving
GO


CREATE PROCEDURE dbo.DeleteReceiving
    @OrderItem_ID int
AS

-- **************************************************************************
-- Procedure: DeleteReceiving
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/19	KM		3744	Added update history template; changed extension to .sql.
-- 03/26/2013	FA		8325	Added code to delete OrderItemRefused table
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE
		@Error_No	int
    SELECT
		@Error_No = 0

    BEGIN TRAN

    UPDATE
		OrderItem
    SET 
		QuantityReceived					= NULL,
        Total_Weight						= 0,
        ReceivedItemCost					= 0,
        ReceivedItemFreight					= 0,
        ReceivedItemHandling				= 0,
        DateReceived						= NULL,
        OriginalDateReceived				= NULL,
        ExpirationDate						= NULL,
        UnitsReceived						= 0,
        ReceivedFreight						= 0,
		ReceivingDiscrepancyReasonCodeID	= NULL
    FROM
		OrderItem
    WHERE
		OrderItem.OrderItem_ID = @OrderItem_ID

    SELECT
		@Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		
       SELECT
			@Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
		DELETE 
			OrderItemRefused
		WHERE 
			OrderItem_ID = @OrderItem_ID

        DELETE
			ItemHistory
        FROM
			ItemHistory
        WHERE
			OrderItem_ID = @OrderItem_ID

		 DELETE
			ItemHistoryQueue
        FROM
			ItemHistoryQueue
        WHERE
			OrderItem_ID = @OrderItem_ID

        SELECT
			@Error_No = @@ERROR
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
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('DeleteReceiving failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
    
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO