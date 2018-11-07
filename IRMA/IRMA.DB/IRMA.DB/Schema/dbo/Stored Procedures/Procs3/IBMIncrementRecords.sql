CREATE PROCEDURE dbo.IBMIncrementRecords
@Store_No int,
@Records int
AS 

UPDATE Store 
SET BatchRecords = @Records
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMIncrementRecords] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMIncrementRecords] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMIncrementRecords] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IBMIncrementRecords] TO [IRMAReportsRole]
    AS [dbo];

