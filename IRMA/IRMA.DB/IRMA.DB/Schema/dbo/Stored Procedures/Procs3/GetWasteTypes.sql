-- =============================================
-- Author:		Faisal Ahmed
-- Create date: 10/23/2009
-- Description:	Returns the waste types
--
-- Modifications:
--  2/05/2018    EM		Included InventoryAdjustmentCode_ID column
-- =============================================
CREATE PROCEDURE dbo.GetWasteTypes
AS
BEGIN
    SET NOCOUNT ON
    
	SELECT 
		Abbreviation AS Waste_Type, 
		AdjustmentDescription AS 'Description',
		InventoryAdjustmentCode_ID As InventoryAdjustmentCode_ID
	FROM InventoryAdjustmentCode
	WHERE Abbreviation in ('SM', 'SP', 'FB', 'IL')
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteTypes] TO [IRMAReportsRole]
    AS [dbo];

