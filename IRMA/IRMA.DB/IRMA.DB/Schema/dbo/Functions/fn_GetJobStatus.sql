CREATE FUNCTION [dbo].[fn_GetJobStatus]
    (@Classname VARCHAR(50))
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @Status VARCHAR(50)
	
	SELECT @Status=Status  FROM JobStatus WHERE Classname=@Classname

    RETURN @Status
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetJobStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetJobStatus] TO [IRMASchedJobsRole]
    AS [dbo];

