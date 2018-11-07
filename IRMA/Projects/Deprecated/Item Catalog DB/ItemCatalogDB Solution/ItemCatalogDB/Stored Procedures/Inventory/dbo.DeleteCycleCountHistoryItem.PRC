SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCycleCountHistoryItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCycleCountHistoryItem]
GO

CREATE PROCEDURE dbo.DeleteCycleCountHistoryItem
	@CycleCountItemID int
	,@ScanDateTime datetime

AS

SET NOCOUNT ON

DELETE FROM 
	CycleCountHistory
WHERE
	CycleCountItemID = @CycleCountItemID 
	AND ScanDateTime = @ScanDateTime

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

