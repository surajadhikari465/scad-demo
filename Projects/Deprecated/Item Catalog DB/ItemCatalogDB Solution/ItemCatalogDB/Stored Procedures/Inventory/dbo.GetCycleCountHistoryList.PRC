SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCycleCountHistoryList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCycleCountHistoryList]
GO

CREATE PROCEDURE dbo.GetCycleCountHistoryList
	@CycleCountItemID as int

AS

SET NOCOUNT ON

SELECT 
	ScanDateTime
	,[Count]
	,Weight
	,PackSize	

FROM 
	CycleCountHistory (NOLOCK)

WHERE 
	CycleCountItemID = @CycleCountItemID

ORDER BY 
	ScanDateTime
	
SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


