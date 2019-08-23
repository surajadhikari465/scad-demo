-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description:	Removes an item-to-nutrifact override mapping.
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE dbo.DeleteItemNutrifactOverride
	@ItemKey		int,
	@Jurisdiction AS INT
AS
BEGIN
	
	set nocount on

    UPDATE ItemNutritionOverride 
		SET NutriFactsID = NULL 
	WHERE ItemKey = @ItemKey AND StoreJurisdictionID = @Jurisdiction
			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemNutrifactOverride] TO [IRMAClientRole]
    AS [dbo];

