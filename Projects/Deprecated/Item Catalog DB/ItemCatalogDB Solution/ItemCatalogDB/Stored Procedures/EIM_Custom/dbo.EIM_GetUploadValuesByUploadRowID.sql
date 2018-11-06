
--=====================================================================
--*********      dbo.EIM_GetUploadValuesByUploadRowID                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_GetUploadValuesByUploadRowID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_GetUploadValuesByUploadRowID]
GO

CREATE PROCEDURE dbo.EIM_GetUploadValuesByUploadRowID
	@UploadRow_ID int
	
AS

	-- This is a custom stored procedure that is used instead of the
	-- generated CRUD procedures for the UploadValue table in the
	-- \EIM_CRUD_Procedures\EIM_UploadValue_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- It is used to denormalize an UploadValue and its UploadAttribute by pulling
	-- the values of the latter into the former.
	-- Created By:	David Marine
	-- Created   :	Feb 15, 2007
	-- 20100215 - Dave Stacey - Add string manipulation to manage different date string data to grid than what's stored
	-- 20100422 - Dave Stacey - Split into seperate files
	SELECT
		UploadValue.UploadValue_ID,
		UploadValue.UploadAttribute_ID,
		UploadValue.UploadRow_ID,
		CASE WHEN (UploadAttribute.DisplayFormatString = 'dd/MM/yyyy' AND LEN(UploadValue.Value) > 5)
			then  substring(UploadValue.Value, 4, 3) + substring(UploadValue.Value, 1, 2)+ Right(UploadValue.Value, Len(UploadValue.Value) - 5)
			else UploadValue.Value 
			end as Value,
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
	WHERE UploadRow_ID = @UploadRow_ID
		AND IsActive = 1
		
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO