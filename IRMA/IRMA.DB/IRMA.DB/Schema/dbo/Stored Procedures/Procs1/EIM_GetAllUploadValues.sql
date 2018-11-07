CREATE PROCEDURE dbo.EIM_GetAllUploadValues
	
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
		UploadAttribute (NOLOCK) ON
			UploadAttribute.UploadAttribute_ID = UploadValue.UploadAttribute_ID
	WHERE IsActive = 1
	UNION
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
		(
			SELECT
				AttributeIdentifier_ID + 10000 AS UploadAttribute_ID,
				Screen_Text AS Name,
				'ItemAttribute' AS TableName,
				dbo.fn_ConvertFromPascalCaseToUnderscore(Field_type) AS ColumnNameOrKey,
				CASE
					WHEN Combo_box = 1 THEN 'ValueList'
					WHEN PATINDEX('%checkbox%', LOWER(Field_type)) > 0 THEN 'CheckBox'
					WHEN PATINDEX('%textbox%', LOWER(Field_type)) > 0 THEN 'TextBox'
					WHEN PATINDEX('%datetime%', LOWER(Field_type)) > 0 THEN 'DateTime'
					ELSE 'TextBox'
				END AS ControlType,
				CASE
					WHEN Combo_box = 1 THEN 'varchar'
					WHEN PATINDEX('%checkbox%', LOWER(Field_type)) > 0 THEN 'bit'
					WHEN PATINDEX('%textbox%', LOWER(Field_type)) > 0 THEN 'varchar'
					WHEN PATINDEX('%datetime%', LOWER(Field_type)) > 0 THEN 'datetime'
					ELSE 'varchar'
				END AS DbDataType,
				CASE
					WHEN Combo_box = 1 THEN 50
					WHEN PATINDEX('%checkbox%', LOWER(Field_type)) > 0 THEN NULL
					WHEN PATINDEX('%textbox%', LOWER(Field_type)) > 0 THEN 50
					WHEN PATINDEX('%datetime%', LOWER(Field_type)) > 0 THEN NULL
					ELSE 50
				END AS Size,
				CAST(0 AS bit) AS IsRequiredValue,
				CAST(0 AS bit) AS IsCalculated,
				NULL AS OptionalMinValue,
				NULL AS OptionalMaxValue,
				CAST(1 AS bit) AS IsActive,
				CASE
					WHEN Combo_box = 1 THEN NULL
					WHEN PATINDEX('%checkbox%', LOWER(Field_type)) > 0 THEN NULL
					WHEN PATINDEX('%textbox%', LOWER(Field_type)) > 0 THEN NULL
					WHEN PATINDEX('%datetime%', LOWER(Field_type)) > 0 THEN 'MM/dd/yyyy'
					ELSE NULL
				END AS DisplayFormatString,
				NULL AS PopulateProcedure,
				NULL AS PopulateIndexField,
				NULL AS PopulateDescriptionField,
				AttributeIdentifier_ID + 1000 AS SpreadsheetPosition,
				Field_values AS ValueListStaticData
			FROM AttributeIdentifier (NOLOCK)
			) UploadAttribute ON
			UploadAttribute.UploadAttribute_ID = UploadValue.UploadAttribute_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_GetAllUploadValues] TO [IRMAClientRole]
    AS [dbo];

