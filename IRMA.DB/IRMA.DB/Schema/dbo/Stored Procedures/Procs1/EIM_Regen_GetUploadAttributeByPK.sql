CREATE PROCEDURE dbo.EIM_Regen_GetUploadAttributeByPK
	@UploadAttribute_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

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
	FROM UploadAttribute (NOLOCK) 
	WHERE UploadAttribute_ID = @UploadAttribute_ID
		AND IsActive = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadAttributeByPK] TO [IRMAClientRole]
    AS [dbo];

