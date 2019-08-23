-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Adds or updates an item-to-nutrifact override mapping
--    for the provided alternate jurisdiction
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE dbo.InsertOrUpdateItemNutrifactOverride
	@ItemKey		int,
	@NutrifactId	int,
	@Jurisdiction   int
AS
BEGIN
	
	set nocount on

	; -- Need this for SQL Compatibility Level 90 because MERGE is NOT recognized as a reserved word at this level, resulting in a parsing error at compat level lower than 100.
    merge 
		ItemNutritionOverride with (updlock, rowlock) ino
	using
		(select @ItemKey as ItemKey, @NutrifactId as NutrifactId, @Jurisdiction as JurisdictionId) as input
	on
		ino.ItemKey = input.ItemKey AND ino.StoreJurisdictionID = input.JurisdictionId
	when matched then
		update set
			ino.NutriFactsID = input.NutrifactId
	when not matched then
		insert (ItemKey, NutriFactsID, StoreJurisdictionID) values (@ItemKey, @NutrifactId, @Jurisdiction);
			
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemNutrifactOverride] TO [IRMAClientRole]
    AS [dbo];
