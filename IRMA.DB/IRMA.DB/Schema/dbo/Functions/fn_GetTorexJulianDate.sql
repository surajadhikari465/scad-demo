CREATE  FUNCTION [dbo].[fn_GetTorexJulianDate]
	(@Date smalldatetime)
RETURNS int

	BEGIN
		-- returns # days since 1900-01-01 + 109208 to adjust for TOREX (UK)
		-- system''s julian date starting at 01/01/1601
		RETURN CONVERT(int, dbo.fn_JulianDate(@Date)) + CONVERT(int,109208)
  	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetTorexJulianDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetTorexJulianDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetTorexJulianDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetTorexJulianDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetTorexJulianDate] TO [IRMAReportsRole]
    AS [dbo];

