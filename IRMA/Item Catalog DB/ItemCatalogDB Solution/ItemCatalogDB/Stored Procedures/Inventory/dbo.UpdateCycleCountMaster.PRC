SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCycleCountMaster]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCycleCountMaster]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

