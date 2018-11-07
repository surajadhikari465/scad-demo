CREATE PROCEDURE dbo.EIM_GetAllUploadAttributes
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadAttribute table in the
	-- \EIM_CRUD_Procedures\EIM_UploadAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 10, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadAttribute_ID],
		[Name],
		[TableName],
		lower([ColumnNameOrKey]) as ColumnNameOrKey,
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
	FROM UploadAttributeView UploadAttribute (NOLOCK)
	WHERE IsActive = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_GetAllUploadAttributes] TO [IRMAClientRole]
    AS [dbo];

