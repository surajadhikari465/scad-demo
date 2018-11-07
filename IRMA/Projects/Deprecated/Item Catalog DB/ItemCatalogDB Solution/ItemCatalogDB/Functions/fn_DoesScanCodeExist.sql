SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'fn_DoesScanCodeExist')) 
    DROP FUNCTION [dbo].[fn_DoesScanCodeExist]
GO

CREATE FUNCTION [dbo].[fn_DoesScanCodeExist] (@ScanCode varchar(13))
RETURNS bit
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE @ScanCodeExists bit;
     IF EXISTS(SELECT * FROM ItemIdentifier ii WHERE ii.Identifier = @ScanCode)
		SET @ScanCodeExists = 1;
	 ELSE
		SET @ScanCodeExists = 0;
     RETURN(@ScanCodeExists);
END;
go
