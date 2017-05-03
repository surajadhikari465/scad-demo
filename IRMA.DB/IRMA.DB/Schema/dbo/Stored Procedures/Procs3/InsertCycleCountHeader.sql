CREATE PROCEDURE dbo.InsertCycleCountHeader
	@MasterCountID int
	,@InvLocID int = null
	,@StartScan datetime
    ,@External bit = 0

AS

	DECLARE @Added int


    SET NOCOUNT ON

    SET @Added = 0

    --See if there is a current record.
    IF NOT EXISTS (SELECT * 
		           FROM CycleCountHeader (nolock)
		           WHERE MasterCountID = @MasterCountID and InvLocID = @InvLocID and [External] = @External)
	BEGIN
		--No current record, INSERT.
		INSERT INTO CycleCountHeader
			(MasterCountID
			,InvLocID
			,StartScan
			,[External]
			,ClosedDate)
		VALUES
			(@MasterCountID
			,@InvLocID
			,@StartScan
			,@External
			,null)

		SET @Added =  SCOPE_IDENTITY()
	END

    SELECT @Added Added

    SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCycleCountHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCycleCountHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCycleCountHeader] TO [IRMAReportsRole]
    AS [dbo];

