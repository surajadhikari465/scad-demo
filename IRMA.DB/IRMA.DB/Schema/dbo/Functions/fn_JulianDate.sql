CREATE   FUNCTION [dbo].[fn_JulianDate]
	(@Date smalldatetime)
RETURNS varchar(7)

	BEGIN
		-- returns # days since 1900-01-01
		RETURN DATEDIFF (day, CONVERT(datetime, '1900-01-01', 110), @Date)
  	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_JulianDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_JulianDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_JulianDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_JulianDate] TO [IRMAReportsRole]
    AS [dbo];

