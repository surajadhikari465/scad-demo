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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvailableWasteTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvailableWasteTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvailableWasteTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvailableWasteTypes] TO [IRMAReportsRole]
    AS [dbo];

