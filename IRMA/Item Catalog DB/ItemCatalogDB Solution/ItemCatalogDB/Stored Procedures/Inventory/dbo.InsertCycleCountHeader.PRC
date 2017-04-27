SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountHeader]
GO



CREATE PROCEDURE dbo.InsertCycleCountHeader
	@MasterCountID int
	,@InvLocID int = null
	,@StartScan datetime
    ,@External bit = 0

AS

	DECLARE @Added int


    SET NOCOUNT ON

    SET @Added = 0

    --See if there is a current record.
    IF NOT EXISTS (SELECT * 
		           FROM CycleCountHeader (nolock)
		           WHERE MasterCountID = @MasterCountID and InvLocID = @InvLocID and [External] = @External)
	BEGIN
		--No current record, INSERT.
		INSERT INTO CycleCountHeader
			(MasterCountID
			,InvLocID
			,StartScan
			,[External]
			,ClosedDate)
		VALUES
			(@MasterCountID
			,@InvLocID
			,@StartScan
			,@External
			,null)

		SET @Added =  SCOPE_IDENTITY()
	END

    SELECT @Added Added

    SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

