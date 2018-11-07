

--=====================================================================
--*********      dbo.EIM_Regen_GetAllUploadAttributes                              
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetAllUploadAttributes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetAllUploadAttributes]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetAllUploadAttributes
	
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
	FROM UploadAttribute (NOLOCK)
	WHERE IsActive = 1
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
