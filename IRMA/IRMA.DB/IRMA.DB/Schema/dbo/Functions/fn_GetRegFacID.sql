CREATE  FUNCTION fn_GetRegFacID
	(
    )
RETURNS int
AS
BEGIN
    DECLARE @Result int

    SELECT @Result = Store_no from Store where regional = 1

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRegFacID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRegFacID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRegFacID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRegFacID] TO [IRMAReportsRole]
    AS [dbo];

