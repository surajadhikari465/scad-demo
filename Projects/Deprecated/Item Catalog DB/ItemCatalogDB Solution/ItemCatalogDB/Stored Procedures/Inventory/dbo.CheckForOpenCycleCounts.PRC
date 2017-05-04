SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForOpenCycleCounts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForOpenCycleCounts]
GO


CREATE PROCEDURE dbo.CheckForOpenCycleCounts
	@MasterCountID int

AS 

SET NOCOUNT ON

SELECT
	 Count(CycleCountID) OpenCounts 
FROM 
	CycleCountMaster (nolock)
INNER JOIN 
	CycleCountHeader (nolock) ON CycleCountHeader.MasterCountID = CycleCountMaster.MasterCountID
WHERE
	CycleCountMaster.MasterCountID = @MasterCountID	
	AND CycleCountHeader.ClosedDate = null 


SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

