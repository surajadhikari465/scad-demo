
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
