CREATE PROCEDURE dbo.InsertCostAdjustmentReason
	@Description VARCHAR(50),
	@IsDefault BIT,
	@CostAdjustmentReason_ID INT OUTPUT
AS
BEGIN
    
	INSERT INTO CostAdjustmentReason	(
		Description,
		IsDefault
	)	VALUES	(
		@Description,
		@IsDefault
	)
    
	SET @CostAdjustmentReason_ID = SCOPE_IDENTITY()
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCostAdjustmentReason] TO [IRMAClientRole]
    AS [dbo];

