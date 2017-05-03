-- =============================================
-- Author:		Faisal Ahmed
-- Create date: 10/23/2009
-- Description:	Returns the waste types
-- =============================================
CREATE PROCEDURE dbo.GetWasteTypes
AS
BEGIN
    SET NOCOUNT ON
    
	SELECT 
		Abbreviation AS Waste_Type, 
		AdjustmentDescription AS Description
	FROM InventoryAdjustmentCode
	WHERE Abbreviation in ('SM', 'SP', 'FB')
    
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

