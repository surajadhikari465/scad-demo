CREATE FUNCTION [dbo].[fn_TrimLeadingZeros] ( @Input VARCHAR(50) )
RETURNS VARCHAR(50)
AS
BEGIN
    RETURN REPLACE(LTRIM(REPLACE(@Input, '0', ' ')), ' ', '0')
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_TrimLeadingZeros] TO [IRMAClientRole]
    AS [dbo];

