CREATE PROCEDURE dbo.UpdateOrderApproved 
@OrderHeader_ID int,
@User_ID int,
@MatchingValidationCode int,
@ResolutionCodeId int
AS

-- **************************************************************************
-- Procedure: UpdateOrderApproved()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple locations within IRMA client to update
-- OrderHeader.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 10/10/2012	BAS   			8133	Added Edit History & Coding Standards.  Added varchar value
--										to pass into dbo.UpdateOrderRefreshCost. This addition to 
--										UORC was to track where UORC was called.
-- 10/19/2012	BAS				8133	renamed the file to .sql
-- **************************************************************************

BEGIN TRY

	DECLARE @PayByAgreedCost bit
	DECLARE @UpdatedPayByAgreedCost bit
	DECLARE @CentralTimeZoneOffset int
	

	SET @UpdatedPayByAgreedCost =  CASE @MatchingValidationCode 
												WHEN 502 THEN 1
												WHEN 503 THEN 0
												ELSE NULL
												END

	--SET @UpdatedPayByAgreedCost = CASE WHEN @MatchingValidationCode = 502 THEN 1 ELSE 0 END
	SELECT @PayByAgreedCost = PayByAgreedCost FROM OrderHeader (NOLOCK) WHERE OrderHeader_ID = @OrderHeader_ID
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

	
	

	IF (@MatchingValidationCode = 0)
		BEGIN
			SELECT @ResolutionCodeID = -1
		END

	BEGIN TRANSACTION   
	
	-- Set the approved date and matching results in the OrderHeader table for an approved order.
	UPDATE OrderHeader
	SET ApprovedDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE()),
		ApprovedBy = @User_ID,
		MatchingValidationCode = @MatchingValidationCode,
		MatchingUser_Id = @User_ID,
		MatchingDate = GETDATE(),
		PayByAgreedCost = ISNULL(@UpdatedPayByAgreedCost, PayByAgreedCost),
		ResolutionCodeID = CASE WHEN @ResolutionCodeID = -1 
                           THEN null
                           ELSE @ResolutionCodeID END 
	WHERE OrderHeader_ID = @OrderHeader_ID
		AND CloseDate IS NOT NULL
		AND UploadedDate IS NULL

	IF ISNULL(@UpdatedPayByAgreedCost, @PayByAgreedCost) <> @PayByAgreedCost
		Begin 
			EXEC UpdateOrderRefreshCosts @OrderHeader_ID, 'UpdateOrderApproved', null, 0  
		End

	COMMIT TRANSACTION   
END TRY
  
BEGIN CATCH
	 IF @@TRANCOUNT > 0
				ROLLBACK TRAN 
	            
			DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
			SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
			RAISERROR ('UpdateOrderApproved failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
END CATCH
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderApproved] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderApproved] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderApproved] TO [IRMAReportsRole]
    AS [dbo];

