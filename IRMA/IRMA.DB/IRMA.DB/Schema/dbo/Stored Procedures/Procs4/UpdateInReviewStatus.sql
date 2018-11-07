CREATE PROCEDURE [dbo].[UpdateInReviewStatus]
	@OrderHeader_ID int,
	@UserId int,
	@Status bit
AS
BEGIN
	UPDATE	OrderHeader
	SET		InReview = @Status,
			InReviewUser = case when @Status = 1 then @UserId else null end
	WHERE	OrderHeader_ID = @OrderHeader_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInReviewStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInReviewStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInReviewStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInReviewStatus] TO [IRMAReportsRole]
    AS [dbo];

