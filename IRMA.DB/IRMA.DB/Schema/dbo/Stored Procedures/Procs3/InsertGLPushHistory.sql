CREATE PROCEDURE dbo.InsertGLPushHistory
@DateStamp DateTime,
@Store_No int, 
@Sales_Date DateTime, 
@Modified_By int, 
@Closed bit, 
@Credit money, 
@Debit money, 
@Account_ID varchar(50)
AS

INSERT INTO GLPushHistory (DateStamp, Store_No, Sales_Date, Modified_By, Closed, Credit, Debit, Account_ID)
VALUES (@DateStamp, @Store_No, @Sales_Date, @Modified_By, @Closed, @Credit, @Debit, @Account_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushHistory] TO [IRMAReportsRole]
    AS [dbo];

