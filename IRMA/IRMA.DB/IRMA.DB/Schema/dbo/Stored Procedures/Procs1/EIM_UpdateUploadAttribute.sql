CREATE PROCEDURE dbo.EIM_UpdateUploadAttribute
		@UploadAttribute_ID int,
		@Name varchar(50),
		@TableName varchar(50),
		@ColumnNameOrKey varchar(50),
		@ControlType varchar(50),
		@DbDataType varchar(50),
		@Size int,
		@IsRequiredValue bit,
		@IsCalculated bit,
		@OptionalMinValue varchar(50),
		@OptionalMaxValue varchar(50),
		@IsActive bit,
		@DisplayFormatString varchar(50),
		@PopulateProcedure varchar(50),
		@PopulateIndexField varchar(50),
		@PopulateDescriptionField varchar(50),
		@SpreadsheetPosition int,
		@ValueListStaticData varchar(4000),
		@DefaultValue varchar(4500)
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadAttribute table in the
	-- \EIM_CRUD_Procedures\EIM_UploadAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 10, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadAttribute
	SET
		[Name] = @Name,
		[TableName] = @TableName,
		[ColumnNameOrKey] = @ColumnNameOrKey,
		[ControlType] = @ControlType,
		[DbDataType] = @DbDataType,
		[Size] = @Size,
		[IsRequiredValue] = @IsRequiredValue,
		[IsCalculated] = @IsCalculated,
		[OptionalMinValue] = @OptionalMinValue,
		[OptionalMaxValue] = @OptionalMaxValue,
		[IsActive] = @IsActive,
		[DisplayFormatString] = @DisplayFormatString,
		[PopulateProcedure] = @PopulateProcedure,
		[PopulateIndexField] = @PopulateIndexField,
		[PopulateDescriptionField] = @PopulateDescriptionField,
		[SpreadsheetPosition] = @SpreadsheetPosition,
		[ValueListStaticData] = @ValueListStaticData,
		[DefaultValue] = @DefaultValue
	WHERE
		UploadAttribute_ID = @UploadAttribute_ID
		
	SELECT @UpdateCount = @@ROWCOUNT