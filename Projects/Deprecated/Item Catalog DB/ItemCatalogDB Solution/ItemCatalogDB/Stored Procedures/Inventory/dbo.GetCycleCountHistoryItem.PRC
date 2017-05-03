SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCycleCountHistoryItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCycleCountHistoryItem]
GO

CREATE PROCEDURE dbo.GetCycleCountHistoryItem
	@CycleCountItemID as int
	,@ScanDateTime as datetime

AS

SET NOCOUNT ON

SELECT 
	ScanDateTime
	,[Count]
	,Weight
	,PackSize
	,IsCaseCnt

FROM 
	CycleCountHistory (NOLOCK)

WHERE 
	CycleCountItemID = @CycleCountItemID 
	AND ScanDateTime = @ScanDateTime

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


