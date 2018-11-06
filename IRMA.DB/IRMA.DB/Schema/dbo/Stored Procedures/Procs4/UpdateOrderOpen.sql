CREATE PROCEDURE dbo.UpdateOrderOpen 
@OrderHeader_ID int,
@Success bit = NULL OUTPUT
AS

-- Reopen the order if it has not already been uploaded to PeopleSoft for processing.
-- This will move the order from the CLOSED or SUSPENDED state back to the SENT state.
UPDATE dbo.OrderHeader
SET CloseDate = NULL, 
    ClosedBy = NULL,
    ApprovedBy = NULL,
    ApprovedDate = NULL,
    MatchingValidationCode = NULL,
    MatchingDate = NULL,
    MatchingUser_ID = NULL,
	RefuseReceivingReasonID = NULL
WHERE OrderHeader_ID = @OrderHeader_ID 
	AND UploadedDate IS NULL

-- Set the success flag based on the current state of the order
SELECT @Success = CASE WHEN (SELECT COUNT(1) FROM dbo.OrderHeader (nolock) WHERE 
								SENT = 1  
								AND CloseDate IS NULL
								AND OrderHeader_ID = @OrderHeader_ID) = 1 
					THEN 1 
					ELSE 0
					END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderOpen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderOpen] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderOpen] TO [IRMAReportsRole]
    AS [dbo];

