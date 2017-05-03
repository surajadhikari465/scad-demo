SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[ValidateIfTableExists]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].ValidateIfTableExists
GO

CREATE PROCEDURE dbo.ValidateIfTableExists
(
	@Schema VARCHAR(50),
	@Table VARCHAR(64),
	@IsTableValid bit output
)
AS

BEGIN

	IF EXISTS(SELECT table_name , table_schema
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_TYPE = 'BASE TABLE' and table_name =@Table AND table_schema =  @Schema
	)

	BEGIN
	   
	    SET @IsTableValid=1

	END

	ELSE

	BEGIN
		   SET @IsTableValid=0
	END
	RETURN  @IsTableValid
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 