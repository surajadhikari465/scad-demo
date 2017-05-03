SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryAdjustmentIDFromCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryAdjustmentIDFromCode]
GO


CREATE PROCEDURE dbo.GetInventoryAdjustmentIDFromCode
	(@Abbreviation char(2),
	 @InventoryAdjustmentCode_ID int OUTPUT)
AS 

SET NOCOUNT ON

/*
	if the Abbreviation doesnt exist in InventoryAdjustmentCode -99 will be returned. The output parameter expects and integer and was failing when NULL
	was being returned. 
*/

SELECT @InventoryAdjustmentCode_ID = InventoryAdjustmentCode_ID
  FROM dbo.InventoryAdjustmentCode
WHERE Abbreviation = @Abbreviation

set @InventoryAdjustmentCode_ID = isnull(@InventoryAdjustmentCode_ID,-99)

SET NOCOUNT OFF

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

