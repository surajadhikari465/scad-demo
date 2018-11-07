
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-03-23
-- Description:	Removes an
--				item-to-nutrifact mapping.
-- Date			Modified By		TFS		Description
-- 07/31/2015	DN				16306	Set ItemNutrifact ID to NULL
-- =============================================

CREATE PROCEDURE dbo.DeleteItemNutrifact
	@ItemKey		int
AS
BEGIN
	
	set nocount on

    UPDATE ItemNutrition 
		SET NutriFactsID = NULL 
	WHERE ItemKey = @ItemKey
			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemNutrifact] TO [IRMAClientRole]
    AS [dbo];

