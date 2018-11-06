if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvailableWasteTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvailableWasteTypes]
GO

CREATE PROCEDURE dbo.GetAvailableWasteTypes  
AS
BEGIN
	DECLARE @IsShrinkSplit bit
	
	SELECT @IsShrinkSplit = (SELECT FlagValue from InstanceDataFlags where FlagKey = 'SplitWasteCategory')

	IF @IsShrinkSplit = 1 
		BEGIN
			(SELECT InventoryAdjustmentCode_ID,
					Abbreviation,
					AdjustmentDescription,
					IAC.Adjustment_ID
			FROM   ItemAdjustment IA (Nolock)
			LEFT JOIN inventoryadjustmentcode IAC (Nolock) ON IAC.Adjustment_ID = IA.Adjustment_ID
					
			WHERE	
					IA.Adjustment_Name = 'Waste')
		END	
	 
	IF @IsShrinkSplit = 0
		BEGIN
			(SELECT InventoryAdjustmentCode_ID,
					Abbreviation,
					AdjustmentDescription,
					IAC.Adjustment_ID
			FROM   ItemAdjustment IA (Nolock)
			LEFT JOIN inventoryadjustmentcode IAC (Nolock) ON IAC.Adjustment_ID = IA.Adjustment_ID
					
			WHERE	
					IA.Adjustment_Name = 'Waste'	
					AND AdjustmentDescription = 'Spoilage')
		END
END	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

