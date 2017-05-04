IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetSystemDateTime]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetSystemDateTime]
GO

CREATE FUNCTION dbo.fn_GetSystemDateTime 
	()
RETURNS datetime
AS
BEGIN
    DECLARE @Result datetime, @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
	
	SELECT @Result = DATEADD(HOUR, @CentralTimeZoneOffset, GETDATE())
	
	RETURN @Result
END
GO
