SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CloseCycleCountHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CloseCycleCountHeader]
GO


CREATE PROCEDURE dbo.CloseCycleCountHeader
	@CycleCountID int
	,@ClosedDate datetime

AS

SET NOCOUNT ON

UPDATE 
	 CycleCountHeader
SET
	ClosedDate = @ClosedDate
WHERE
	CycleCountID = @CycleCountID		

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
