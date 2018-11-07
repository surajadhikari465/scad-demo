CREATE PROCEDURE dbo.UpdateCycleCountHistory 
	@CycleCountItemID int = null
	,@ScanDateTime datetime
	,@Count decimal(18,4) = null
	,@Weight decimal(18,4) = null
	,@PackSize decimal(18,4)
	,@IsCaseCnt bit
AS 

SET NOCOUNT ON

IF EXISTS (SELECT CycleCountItemID FROM CycleCountHistory (NOLOCK) WHERE CycleCountItemID = @CycleCountItemID AND ScanDateTime = @ScanDateTime)

	-- If CycleCountItemID was provided, we are updating.
	BEGIN
		UPDATE 
			CycleCountHistory
		SET 
			ScanDateTime = @ScanDateTime, [Count] = @Count, Weight = @Weight, PackSize = @PackSize, IsCaseCnt = @IsCaseCnt
		WHERE 
			CycleCountItemID = @CycleCountItemID AND ScanDateTime = @ScanDateTime
	END
ELSE

	-- Create new CycleCountHistory.
	BEGIN
		INSERT INTO 
			CycleCountHistory
		VALUES
			(@CycleCountItemID, @ScanDateTime, @Count, @Weight, @PackSize, @IsCaseCnt)
	END

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountHistory] TO [IRMAReportsRole]
    AS [dbo];

