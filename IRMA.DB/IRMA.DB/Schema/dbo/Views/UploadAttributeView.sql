CREATE VIEW [dbo].[UploadAttributeView]

-- ****************************************************************************************************************
-- Procedure: UploadAttributeView
--    Author: n/a
--      Date: n/a
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-05-30	KM		12505	For the AttributeIdentifier portion of the UNION, select null as the DisplayFormatString
--								to maintain date interoperability with the EIM code;
-- ****************************************************************************************************************

AS
	SELECT
		[UploadAttribute_ID],
		[Name],
		[TableName],
		[ColumnNameOrKey],
		[ControlType],
		[DbDataType],
		[Size],
		[IsRequiredValue],
		[IsCalculated],
		[OptionalMinValue],
		[OptionalMaxValue],
		[IsActive],
		[DisplayFormatString],
		[PopulateProcedure],
		[PopulateIndexField],
		[PopulateDescriptionField],
		[SpreadsheetPosition],
		[ValueListStaticData],
		[DefaultValue]
	FROM 
		UploadAttribute (NOLOCK) 

	UNION

	SELECT
		AttributeIdentifier_ID + 10000 AS UploadAttribute_ID,
		Screen_Text AS Name,
		'itemattribute' AS TableName,
		LOWER(dbo.fn_ConvertFromPascalCaseToUnderscore(Field_type)) AS ColumnNameOrKey,
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
		--CASE
		--	WHEN Combo_box = 1 THEN NULL
		--	WHEN PATINDEX('%checkbox%', LOWER(Field_type)) > 0 THEN NULL
		--	WHEN PATINDEX('%textbox%', LOWER(Field_type)) > 0 THEN NULL
		--	WHEN PATINDEX('%datetime%', LOWER(Field_type)) > 0 THEN 'MM/dd/yyyy'
		--	ELSE NULL
		--END AS DisplayFormatString,
		NULL AS DisplayFormatString,
		NULL AS PopulateProcedure,
		NULL AS PopulateIndexField,
		NULL AS PopulateDescriptionField,
		AttributeIdentifier_ID + 1000 AS SpreadsheetPosition,
		Field_values AS ValueListStaticData,
		Default_value As DefaultValue
	FROM 
		AttributeIdentifier (NOLOCK)
	WHERE 
		Screen_Text IS NOT NULL AND LEN(Screen_Text) > 0
GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadAttributeView] TO [IRMAReportsRole]
    AS [dbo];

