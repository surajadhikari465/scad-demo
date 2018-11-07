CREATE PROCEDURE dbo.ItemAttributes_Regen_UpdateAttributeIdentifier
		@AttributeIdentifier_ID int,
		@Screen_Text varchar(50),
		@field_type varchar(50),
		@combo_box bit,
		@max_width int,
		@default_value varchar(50),
		@field_values varchar(8000)
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	UPDATE AttributeIdentifier
	SET
		[Screen_Text] = @Screen_Text,
		[field_type] = @field_type,
		[combo_box] = @combo_box,
		[max_width] = @max_width,
		[default_value] = @default_value,
		[field_values] = @field_values
	WHERE
		AttributeIdentifier_ID = @AttributeIdentifier_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_UpdateAttributeIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_UpdateAttributeIdentifier] TO [IRMAClientRole]
    AS [dbo];

