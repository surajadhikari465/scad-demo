Create PROCEDURE [dbo].[CheckInventoryAdjustmentAbbreviation] 

	@strAbbreviation char(2)

AS

BEGIN

    SET NOCOUNT ON
	
		SELECT * FROM dbo.InventoryAdjustmentCode WHERE Abbreviation = @strAbbreviation		
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckInventoryAdjustmentAbbreviation] TO [IRMAClientRole]
    AS [dbo];

