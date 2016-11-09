CREATE PROCEDURE dbo.ItemAttributes_Regen_DeleteItemAttribute
		@ItemAttribute_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007
	
	DELETE FROM ItemAttribute
	WHERE
		ItemAttribute_ID = @ItemAttribute_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_DeleteItemAttribute] TO [IRMAClientRole]
    AS [dbo];

