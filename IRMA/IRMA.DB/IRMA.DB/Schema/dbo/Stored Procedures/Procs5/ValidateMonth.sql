CREATE PROCEDURE dbo.ValidateMonth
@sYear smallint,
@sMonth tinyint
AS

SELECT MIN(Date_Key) AS StartDate, MAX(Week) as Weeks
                         FROM Date (NOLOCK)
                         WHERE Period = @sMonth AND Year = @sYear
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateMonth] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateMonth] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateMonth] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateMonth] TO [IRMAReportsRole]
    AS [dbo];

