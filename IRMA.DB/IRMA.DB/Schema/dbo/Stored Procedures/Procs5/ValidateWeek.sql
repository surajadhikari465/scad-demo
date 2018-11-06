CREATE PROCEDURE dbo.ValidateWeek
@Year smallint,
@Month tinyint
AS

SELECT MIN(Date_Key) AS StartDate 
FROM Date (NOLOCK) 
WHERE ((Period-1)*4) + Week = @Month AND Year = @Year
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateWeek] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateWeek] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateWeek] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateWeek] TO [IRMAReportsRole]
    AS [dbo];

