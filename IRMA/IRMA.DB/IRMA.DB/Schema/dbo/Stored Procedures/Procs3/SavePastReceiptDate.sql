CREATE PROCEDURE [dbo].[SavePastReceiptDate] 
	@OrderHeader_ID				int,  
    @PastReceiptDate			DateTime
AS   

-- **************************************************************************
-- Procedure: SavePastReceiptDate
--    Author: MZ
--      Date: 6/12/2019
--
-- Description:
-- This procedure is called from IRMA Client Receiving List screen to save 
-- Past Receipt Date if entered.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
--
-- ***************************************************************************************

BEGIN      
    SET NOCOUNT ON   
    BEGIN TRY
	
	IF NOT EXISTS (SELECT 1 FROM [dbo].[OrderHeaderExtended] WHERE OrderHeader_ID = @OrderHeader_ID)	
		INSERT INTo [dbo].[OrderHeaderExtended] (OrderHeader_ID, PastReceiptDate)
		VALUES (@OrderHeader_ID, @PastReceiptDate)
    ELSE
		UPDATE [dbo].[OrderHeaderExtended]
		   SET PastReceiptDate = @PastReceiptDate,
			   LastUpdatedDate = GETDATE()
		 WHERE OrderHeader_ID = @OrderHeader_ID
	
	END TRY  

	--**************************************************************************
	--SQL Error Catch
	--**************************************************************************
	BEGIN CATCH  
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
        RAISERROR ('SavePastReceiptDate failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
    END CATCH  
      
    SET NOCOUNT OFF  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SavePastReceiptDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SavePastReceiptDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SavePastReceiptDate] TO [IRMAReportsRole]
    AS [dbo];

