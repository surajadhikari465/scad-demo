CREATE PROCEDURE dbo.CheckForExternalCycleCount
	@MasterCountID int

AS

SET NOCOUNT ON
	
IF EXISTS(SELECT 
		CycleCountID 
	FROM 
		CycleCountMaster CCM (NOLOCK)
	INNER JOIN
		CycleCountHeader CCH (NOLOCK)  ON  CCH.MasterCountID = CCM.MasterCountID
	INNER JOIN
		StoreSubTeam SST ON CCM.Store_No = SST.Store_No AND CCM.SubTeam_No = SST.SubTeam_No
	WHERE 
		CCM.MasterCountID = @MasterCountID 
		AND SST.ICVID IS NOT NULL
		AND CCH.[External] = 1) 

	BEGIN
		SELECT 1 AS Found
	END
ELSE
	BEGIN
		SELECT 0 AS Found
	END

SET NOCOUNT ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForExternalCycleCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForExternalCycleCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForExternalCycleCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForExternalCycleCount] TO [IRMAReportsRole]
    AS [dbo];

