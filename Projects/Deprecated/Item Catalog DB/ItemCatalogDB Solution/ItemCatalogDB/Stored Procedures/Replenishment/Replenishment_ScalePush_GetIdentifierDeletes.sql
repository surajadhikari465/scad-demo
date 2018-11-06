IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Replenishment_ScalePush_GetIdentifierDeletes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC ('create PROCEDURE [dbo].[Replenishment_ScalePush_GetIdentifierDeletes] (@foo int) as select 1')
GO

ALTER PROCEDURE [dbo].[Replenishment_ScalePush_GetIdentifierDeletes]
    @Date				datetime,
    @IsScaleZoneData	bit  -- USED TO LIMIT OUTPUT TO SCALE ITEMS 
AS 
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT
	-- *************************************************************************************
	-- Procedure: Replenishment_POSPush_GetIdentifierDeletes()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- Gets Identifier Delete info as part of POS Push routine
	--
	-- Modification History:
	-- Date      Init	TFS			Comment
	-- 20110824	 BBB	2866		Minor format tweak; removed select * from fn call;
	-- 20110824	 DBS	2866		Took out loop and table var for tax values for performance
	-- 20160718  Jamali PBI16859	Replace the table variable @itemStoreTable with the Temp table #itemStoreTable
	-- 20161030  Jamali PBI18909	Removed the code that generated the tax flag data
	-- *************************************************************************************
	--**************************************************************************
	-- Returns the list of items associated with the added identifiers and their details
	--**************************************************************************
	EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
		@NewItemVal				= 0,
		@ItemChangeVal			= 0,
		@RemoveItemVal			= 1,
		@PIRUSHeaderActionVal	= 'D ',
		@Deletes				= 0,
		@IsPOSPush				= 0,
		@IsScaleZoneData		= @IsScaleZoneData,
		@POSDeAuthData			= 0,	
		@ScaleDeAuthData		= 0, 
		@ScaleAuthData			= 0,
		@IdentifierAdds			= 0,
		@IdentifierDeletes		= 1,
		@Date					= @Date,
		@AuditReport			= 0,
		@Store_No				= NULL
END
GO