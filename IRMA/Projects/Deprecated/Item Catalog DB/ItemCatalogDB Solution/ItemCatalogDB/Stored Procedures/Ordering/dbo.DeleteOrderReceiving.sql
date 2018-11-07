SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].DeleteOrderReceiving') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].DeleteOrderReceiving
GO


CREATE PROCEDURE dbo.DeleteOrderReceiving
        @OrderHeader_ID		int
AS

-- **************************************************************************
-- Procedure: DeleteOrderReceiving
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/19	KM		3744	Added update history template; code formatting; changed extension to .sql.
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE
		@Error_No	int
    
    SELECT
		@Error_No = 0

    BEGIN TRAN   

    SELECT @Error_No = @@ERROR   

    IF @Error_No = 0
    BEGIN
        DELETE 
			ItemHistory
        FROM
			ItemHistory				(nolock)	ih
			INNER JOIN OrderItem	(nolock)	oi	ON ih.OrderItem_ID = oi.OrderItem_ID
        WHERE
			OrderHeader_ID = @OrderHeader_ID
				
	  DELETE 
			ItemHistoryQueue
        FROM
			ItemHistoryQueue				(nolock)	ih
			INNER JOIN OrderItem	(nolock)	oi	ON ih.OrderItem_ID = oi.OrderItem_ID
        WHERE
			OrderHeader_ID = @OrderHeader_ID
    
    
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE
			OrderItem
        SET 
			QuantityReceived		= 0,
            Total_Weight			= 0,
            ReceivedItemCost		= 0,
            ReceivedItemFreight		= 0,
            ReceivedItemHandling	= 0,
            DateReceived			= NULL,
            OriginalDateReceived	= NULL,
            ExpirationDate			= NULL,
            UnitsReceived			= 0,
            ReceivedFreight			= 0
        FROM 
			OrderItem (rowlock)
        WHERE
			OrderHeader_ID = @OrderHeader_ID

        SELECT @Error_No = @@ERROR
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
        RAISERROR ('DeleteOrderReceiving failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END 
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

