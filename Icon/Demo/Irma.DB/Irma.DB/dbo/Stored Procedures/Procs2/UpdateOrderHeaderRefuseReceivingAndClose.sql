CREATE PROCEDURE dbo.UpdateOrderHeaderRefuseReceivingAndClose
@OrderHeader_ID int,
@User_ID int,
@RefuseReceivingReasonID int
AS 
-- **************************************************************************
-- Procedure: UpdateOrderHeaderRefuseReceivingAndClose()
--    Author: Mugdha Deshpande
--      Date: 08/01/2011
--
-- Description:
-- This procedure is meant to close and approve the PO's that have Refuse Receiving 
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 08/01/2011	MD   			2460	Created
-- 11/07/1212   MZ              8331    Added the UPDATE to OrderItem table
-- **************************************************************************

	BEGIN TRY                       
	BEGIN TRANSACTION  

		DECLARE @CentralTimeZoneOffset int
		SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

		UPDATE OrderHeader 
		SET CloseDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()),
			OriginalCloseDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()), 
			ClosedBy =  @User_ID,
			RefuseReceivingReasonID = @RefuseReceivingReasonID,
			ApprovedDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()),
			ApprovedBy = @User_ID
		WHERE OrderHeader_ID = @OrderHeader_ID	
	
		UPDATE OrderItem
		SET QuantityReceived = 0, 
			DateReceived = DATEADD(hour, @CentralTimeZoneOffset, GETDATE())
		WHERE OrderHeader_ID = @OrderHeader_ID	

	COMMIT TRANSACTION      
	END TRY          
    BEGIN CATCH  
		IF @@TRANCOUNT > 0  
			ROLLBACK TRAN                 
        
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
        RAISERROR ('UpdateOrderHeaderRefuseReceivingAndClose failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
    END CATCH
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderRefuseReceivingAndClose] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderRefuseReceivingAndClose] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderRefuseReceivingAndClose] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderRefuseReceivingAndClose] TO [IRMAReportsRole]
    AS [dbo];

