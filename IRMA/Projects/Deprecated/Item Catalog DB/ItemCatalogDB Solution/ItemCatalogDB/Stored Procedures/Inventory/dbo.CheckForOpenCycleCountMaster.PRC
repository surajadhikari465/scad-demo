SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForOpenCycleCountMaster]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForOpenCycleCountMaster]
GO


CREATE PROCEDURE dbo.CheckForOpenCycleCountMaster
	@MasterCountID int,
	@Store_No int,
	@SubTeam_No int

AS 

SET NOCOUNT ON

IF @MasterCountID > 0 
	--If a RecID was passed in, then we are looking for an open count where the RecID does not match.
	BEGIN
		SELECT Count(MasterCountID) AS Found 
		FROM CycleCountMaster (nolock)
		WHERE Store_No = @Store_No 
			and SubTeam_No = @SubTeam_No 
			and MasterCountID <> @MasterCountID
			and ClosedDate IS NULL
	END
ELSE
	--If no RecID was passed in, then search for an open count regardless of the RecID.
	BEGIN
		SELECT Count(MasterCountID) AS Found 
		FROM CycleCountMaster (nolock)
		WHERE Store_No = @Store_No 
			and SubTeam_No = @SubTeam_No
			and ClosedDate IS NULL
	END 

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

