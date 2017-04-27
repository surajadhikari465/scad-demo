SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForExternalCycleCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForExternalCycleCount]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

