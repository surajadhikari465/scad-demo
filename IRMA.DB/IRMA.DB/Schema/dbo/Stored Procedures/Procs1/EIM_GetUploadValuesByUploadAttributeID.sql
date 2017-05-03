CREATE PROCEDURE dbo.EIM_GetUploadValuesByUploadAttributeID
	@UploadAttribute_ID int
	
AS

	-- This is a custom stored procedure that is used instead of the
	-- generated CRUD procedures for the UploadValue table in the
	-- \EIM_CRUD_Procedures\EIM_UploadValue_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- It is used to denormalize an UploadValue and its UploadAttribute by pulling
	-- the values of the latter into the former.
	-- Created By:	David Marine
	-- Created   :	Feb 15, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		UploadValue.UploadValue_ID,
		UploadValue.UploadAttribute_ID,
		UploadValue.UploadRow_ID,
		UploadValue.Value,
		[Name],
		UploadAttribute.TableName,
		UploadAttribute.ColumnNameOrKey,
		UploadAttribute.ControlType,
		UploadAttribute.DbDataType,
		UploadAttribute.Size,
		UploadAttribute.IsRequiredValue,
		UploadAttribute.IsActive,
		UploadAttribute.PopulateProcedure,
		UploadAttribute.PopulateIndexField,
		UploadAttribute.PopulateDescriptionField,
		UploadAttribute.SpreadsheetPosition,
		UploadAttribute.OptionalMinValue,
		UploadAttribute.OptionalMaxValue,
		UploadAttribute.ValueListStaticData
	FROM
		UploadValue (NOLOCK) JOIN
		UploadAttributeView UploadAttribute (NOLOCK) ON
			UploadAttribute.UploadAttribute_ID = UploadValue.UploadAttribute_ID
	WHERE UploadValue.UploadAttribute_ID = @UploadAttribute_ID
		AND IsActive = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_GetUploadValuesByUploadAttributeID] TO [IRMAClientRole]
    AS [dbo];

