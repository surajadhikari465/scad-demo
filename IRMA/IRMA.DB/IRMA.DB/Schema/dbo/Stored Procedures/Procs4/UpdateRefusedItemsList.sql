CREATE PROCEDURE [dbo].[UpdateRefusedItemsList]
	@DataList varchar(max), 
	@ListSeparator1 char(1), 
	@ListSeparator2 char(1)
AS

-- **************************************************************************
-- Procedure:   UpdateRefusedItemsList
--    Author:   Faisal Ahmed
--      Date:   03/18/2013
--
-- Description: This stored procedure first calls the fn_Parse_List_Four function to parse the input string 
--              @DataList into a OrderItemRefused_ID/DiscrepancyCodeID/Refused Cost/Refused Quantity list.
--              Then it updates the corresponding OrderItemRefused record(s).
--  
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/18/2013	FA		8457	Initial creation
-- 03/31/2013	FA		8457	Added code to delete records with refused quantity = 0
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
				OrderItemRefused
			SET 
				DiscrepancyCodeID	= ls.Key_Value2,
				InvoiceCost			= ls.Key_value3,
				RefusedQuantity		= ls.Key_Value4
			FROM OrderItemRefused oir 
			INNER JOIN (
				SELECT Key_Value1, Key_Value2, Key_Value3, Key_Value4 
				FROM fn_Parse_List_Four(@DataList,@ListSeparator1, @ListSeparator2)
				) ls
				ON oir.OrderItemRefusedID =  ls.Key_Value1
    
			DELETE 
				OrderItemRefused
			FROM
				OrderItemRefused oir
			INNER JOIN (
				SELECT Key_Value1, Key_Value2, Key_Value3, Key_Value4 
				FROM fn_Parse_List_Four(@DataList,@ListSeparator1, @ListSeparator2)
				) ls
				ON oir.OrderItemRefusedID =  ls.Key_Value1 and oir.RefusedQuantity = 0
				
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
				RAISERROR ('UpdateRefusedItemsList failed with @@ERROR: %d', @Severity, 1, @Error_No)
			END
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedItemsList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedItemsList] TO [IRMAReportsRole]
    AS [dbo];

