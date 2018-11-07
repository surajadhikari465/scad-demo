CREATE PROCEDURE dbo.PosQueueGLClose
@Store_No int,
@Sales_Date datetime,
@User_ID int
AS

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    UPDATE POSChanges
    SET GL_Pushed = 1, Modified_By = @User_ID
    WHERE Aggregated = 1 AND Store_No = @Store_No AND Sales_Date = @Sales_Date

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        INSERT INTO GLPushHistory (Store_No, Sales_Date, Modified_By, Closed, Account_ID)
        VALUES (@Store_No, @Sales_Date, @User_ID, 1, '3000')

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PosQueueGLClose failed with @@ERROR: %d', @Severity, 1, @Error_No)       
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLClose] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLClose] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLClose] TO [IRMAReportsRole]
    AS [dbo];

