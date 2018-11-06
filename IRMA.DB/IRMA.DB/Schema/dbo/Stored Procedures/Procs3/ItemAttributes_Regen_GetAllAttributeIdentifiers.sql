CREATE PROCEDURE dbo.ItemAttributes_Regen_GetAllAttributeIdentifiers
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	SELECT
		[AttributeIdentifier_ID],
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	FROM AttributeIdentifier (NOLOCK)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_GetAllAttributeIdentifiers] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_GetAllAttributeIdentifiers] TO [IRMAClientRole]
    AS [dbo];

