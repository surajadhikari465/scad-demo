CREATE PROCEDURE dbo.ItemAttributes_Regen_InsertAttributeIdentifier
		@Screen_Text varchar(50),
			@field_type varchar(50),
			@combo_box bit,
			@max_width int,
			@default_value varchar(50),
			@field_values varchar(8000)
		,
		@AttributeIdentifier_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	INSERT INTO AttributeIdentifier
	(
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	)
	VALUES (
		@Screen_Text,
		@field_type,
		@combo_box,
		@max_width,
		@default_value,
		@field_values
	)
	
		SELECT @AttributeIdentifier_ID  = SCOPE_IDENTITY()
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_InsertAttributeIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_InsertAttributeIdentifier] TO [IRMAClientRole]
    AS [dbo];

