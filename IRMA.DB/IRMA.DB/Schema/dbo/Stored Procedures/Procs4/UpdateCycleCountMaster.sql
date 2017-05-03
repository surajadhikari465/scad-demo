CREATE PROCEDURE dbo.UpdateCycleCountMaster
	@MasterCountID int = NULL,
	@Store_No int,
	@SubTeam_No int,
	@EndScan datetime,
	@ClosedDate datetime,
	@EndofPeriod as bit

AS

SET NOCOUNT ON

--See if there is a current record.
IF @MasterCountID IS NOT NULL
	BEGIN
		--UPDATE.
		UPDATE CycleCountMaster SET 
			EndofPeriod = @EndofPeriod
			,Store_No = @Store_No
			,SubTeam_No = @SubTeam_No
			,EndScan = @EndScan
			,ClosedDate = @ClosedDate
		WHERE 
			MasterCountID = @MasterCountID
	END
ELSE
	BEGIN
		--INSERT.
		INSERT INTO CycleCountMaster
			(EndofPeriod
			,Store_No
			,SubTeam_No
			,EndScan
			,ClosedDate)
		VALUES
			(@EndofPeriod
			,@Store_No
			,@SubTeam_No
			,@EndScan
			,@ClosedDate)
		SET @MasterCountID =  scope_identity();
	END

EXEC GetCycleCountMaster @MasterCountID

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountMaster] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountMaster] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCycleCountMaster] TO [IRMAReportsRole]
    AS [dbo];

