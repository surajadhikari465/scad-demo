
--=====================================================================
--*********      dbo.EIM_InsertUploadAttribute                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_InsertUploadAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_InsertUploadAttribute]
GO
CREATE PROCEDURE dbo.EIM_InsertUploadAttribute
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
		@UploadAttribute_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadAttribute table in the
	-- \EIM_CRUD_Procedures\EIM_UploadAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 10, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadAttribute
	(
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
	)
	VALUES (
		@Name,
		@TableName,
		@ColumnNameOrKey,
		@ControlType,
		@DbDataType,
		@Size,
		@IsRequiredValue,
		@IsCalculated,
		@OptionalMinValue,
		@OptionalMaxValue,
		@IsActive,
		@DisplayFormatString,
		@PopulateProcedure,
		@PopulateIndexField,
		@PopulateDescriptionField,
		@SpreadsheetPosition,
		@ValueListStaticData,
		@DefaultValue
	)
	
		SELECT @UploadAttribute_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO