CREATE PROCEDURE dbo.LoadPMPriceChange

AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM PMPriceChangeLoad

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        BULK INSERT PMPriceChangeLoad
        FROM 'E:\PMPriceChange.txt'
        WITH ( FIELDTERMINATOR = '|',
               ROWTERMINATOR = '|\n' )

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO PMPriceChange (Item_Key, Price, Org_Level, Level_ID)
        SELECT * FROM PMPriceChangeLoad

        SELECT @Error_No = @@ERROR
    END


    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('LoadPMPriceChange failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadPMPriceChange] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadPMPriceChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadPMPriceChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadPMPriceChange] TO [IRMAReportsRole]
    AS [dbo];

