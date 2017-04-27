SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckInventoryAdjustmentAbbreviation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckInventoryAdjustmentAbbreviation]
GO

Create PROCEDURE [dbo].[CheckInventoryAdjustmentAbbreviation] 

	@strAbbreviation char(2)

AS

BEGIN

    SET NOCOUNT ON
	
		SELECT * FROM dbo.InventoryAdjustmentCode WHERE Abbreviation = @strAbbreviation		
		
    SET NOCOUNT OFF	

END
GO

