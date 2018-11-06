if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateReceivingDiscrepancyReasonCodeID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateReceivingDiscrepancyReasonCodeID]
GO

CREATE PROCEDURE [dbo].[UpdateReceivingDiscrepancyReasonCodeID]
	@ReceivingDiscrepancyReasonCodeList varchar(max), 
	@ListSeparator1 char(1), 
	@ListSeparator2 char(1)
AS

-- **************************************************************************
-- Procedure:   UpdateReceivingDiscrepancyReasonCodeID
--    Author:   Min Zhao
--      Date:   2/8/2012
--
-- Description: This stored procedure first calls the fn_Parse_List_Two function to parse the input string 
--              @ReceivingDiscrepancyReasonCodeList into a OrderItem_ID/ReceivingDiscrepancyReasonCodeID
--              key/value pair(s). Then it updates the corresponding OrderItem record(s) with the 
--              ReceivingDiscrepancyReasonCodeID on the key/value pair(s).
--  
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/02/09	MZ		4317	Initial creation
-- **************************************************************************

BEGIN
    SET NOCOUNT ON
    
    DECLARE
		@Error_No	int

    SELECT 
		@Error_No = 0

    BEGIN TRAN
		BEGIN
			UPDATE
				OrderItem
			SET 
				ReceivingDiscrepancyReasonCodeID = ls.Key_Value2
			FROM OrderItem oi INNER JOIN 
				(
				SELECT Key_Value1, Key_Value2 FROM fn_Parse_List_Two(@ReceivingDiscrepancyReasonCodeList,@ListSeparator1, @ListSeparator2)
				) ls
				ON oi.OrderItem_ID = ls.Key_Value1
    
        SELECT @Error_No = @@ERROR
    
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
            SET NOCOUNT OFF
            RAISERROR ('UpdateReceivingDiscrepancyReasonCodeID failed with @@ERROR: %d', @Severity, 1, @Error_No)
        END
    END
END
GO