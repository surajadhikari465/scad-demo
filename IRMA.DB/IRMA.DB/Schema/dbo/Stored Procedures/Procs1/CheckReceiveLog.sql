CREATE PROCEDURE dbo.CheckReceiveLog 
@Store_No int
AS 

SELECT LastRecvLogDate
FROM Store
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceiveLog] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceiveLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceiveLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceiveLog] TO [IRMAReportsRole]
    AS [dbo];

