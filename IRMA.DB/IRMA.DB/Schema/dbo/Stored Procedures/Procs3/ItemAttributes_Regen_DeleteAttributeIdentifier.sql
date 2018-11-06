CREATE PROCEDURE dbo.ItemAttributes_Regen_DeleteAttributeIdentifier
		@AttributeIdentifier_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007
	
	DELETE FROM AttributeIdentifier
	WHERE
		AttributeIdentifier_ID = @AttributeIdentifier_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_DeleteAttributeIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_DeleteAttributeIdentifier] TO [IRMAClientRole]
    AS [dbo];

