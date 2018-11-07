CREATE PROCEDURE dbo.InsertSalesExportQueue
    @Store_No int,
    @Date datetime
AS

BEGIN
    SET NOCOUNT ON

    IF NOT EXISTS(SELECT * FROM SalesExportQueue WHERE Store_No = @Store_no and Date_Key = @Date)
        INSERT INTO SalesExportQueue VALUES(@Store_No, @Date)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSalesExportQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSalesExportQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSalesExportQueue] TO [IRMAReportsRole]
    AS [dbo];

