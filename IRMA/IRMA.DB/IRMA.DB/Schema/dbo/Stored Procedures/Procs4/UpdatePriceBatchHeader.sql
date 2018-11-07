CREATE PROCEDURE dbo.[UpdatePriceBatchHeader]
    @PriceBatchHeaderID int,
	@BatchDescription as varchar(30),
	@AutoApplyFlag as bit,
	@ApplyDate as datetime,
	@POSBatchId as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE PriceBatchHeader 
	SET
		BatchDescription = @BatchDescription,
		AutoApplyFlag = @AutoApplyFlag,
		ApplyDate = @ApplyDate,
		POSBatchId = @POSBatchId
	WHERE 
		PriceBatchHeaderID = @PriceBatchHeaderID

    SET NOCOUNT OFF;

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePriceBatchHeader] TO [IRMAClientRole]
    AS [dbo];

