SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateAvgCostAdjReasonStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateAvgCostAdjReasonStatus]
GO


CREATE PROCEDURE dbo.UpdateAvgCostAdjReasonStatus 
	@ID int,
	@Active bit
AS 

SET NOCOUNT ON

UPDATE AvgCostAdjReason SET Active = @Active WHERE ID = @ID

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

