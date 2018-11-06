﻿SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetColumnNamesBySchemaTable]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].GetColumnNamesBySchemaTable
GO

CREATE PROCEDURE dbo.GetColumnNamesBySchemaTable
@Schema VARCHAR(50),
@Table VARCHAR(64)
AS
BEGIN
		SELECT Column_Name
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= @Table and TABLE_SCHEMA = @Schema

END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 