SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateCycleCountMaster]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateCycleCountMaster]
GO


CREATE PROCEDURE dbo.CheckForDuplicateCycleCountMaster
	@MasterCountID int
	,@Store_No int
	,@SubTeam_No int
	,@EndScan datetime

AS 

SET NOCOUNT ON

IF @MasterCountID > 0 
	--If a RecID was passed in, then we are looking for a match on a record where the RecID does not match.
	BEGIN
		SELECT Count(MasterCountID) AS Found 
		FROM CycleCountMaster (nolock)
		WHERE Store_No = @Store_No and SubTeam_No = @SubTeam_No and EndScan = @EndScan and MasterCountID <> @MasterCountID
	END
ELSE
	--If no RecID was passed in, then search for a match regardless of the RecID.
	BEGIN
		SELECT Count(MasterCountID) AS Found 
		FROM CycleCountMaster (nolock)
		WHERE Store_No = @Store_No and SubTeam_No = @SubTeam_No and EndScan = @EndScan
	END 

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

