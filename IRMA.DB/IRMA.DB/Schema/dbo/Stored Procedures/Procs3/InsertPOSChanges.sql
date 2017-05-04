CREATE PROCEDURE dbo.InsertPOSChanges
@Store_No int,
@Sales_Date datetime
AS

INSERT INTO POSChanges (Store_No, Sales_Date) 
VALUES (@Store_No, @Sales_Date)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPOSChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPOSChanges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPOSChanges] TO [IRMAReportsRole]
    AS [dbo];

