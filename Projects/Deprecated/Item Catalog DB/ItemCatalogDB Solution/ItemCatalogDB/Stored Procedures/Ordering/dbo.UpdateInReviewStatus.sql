IF exists (select 1 from sysobjects where name  ='UpdateInReviewStatus' and xtype = 'P')
begin
	drop procedure [dbo].[UpdateInReviewStatus]
end
GO

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