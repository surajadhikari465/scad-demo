CREATE PROCEDURE dbo.SalesAggregation
    @Store_No int,
    @Date datetime,
    @InsertItemHistory bit
AS

BEGIN
    DECLARE @CurrLastSalesUpdateDate datetime, @Error_No int, @SectionName varchar(255)

    SELECT @Error_No = 0
    BEGIN TRAN

    SELECT @SectionName  = 'ClearLastSalesUpdateDate'

    SELECT @CurrLastSalesUpdateDate = ISNULL(LastSalesUpdateDate, 0) FROM Store WHERE Store_No = @Store_No

    UPDATE Store
    SET LastSalesUpdateDate = null
    WHERE Store_No = @Store_No
    SELECT @Error_No = @@Error

    IF @Error_No = 0
    BEGIN
        SELECT @SectionName  = 'UpdateSalesAggregates'
        EXEC UpdateSalesAggregates @Store_No, @Date
        SELECT @Error_No = @@Error
    END

    IF (@Error_No = 0) AND (@InsertItemHistory = 1)
    BEGIN
        SELECT @SectionName  = 'UpdateItemHistoryFromSales'
        EXEC UpdateItemHistoryFromSales @Store_No, @Date
        SELECT @Error_No = @@Error
    END

    IF @Error_No = 0
    BEGIN
        SELECT @SectionName  = 'UpdatePosChangesAggregated'
        EXEC UpdatePosChangesAggregated @Store_No, @Date
        SELECT @Error_No = @@Error
    END

    IF @Error_No = 0
    BEGIN
        SELECT @SectionName  = 'SettingLastSalesUpdateDate'
        UPDATE Store
        SET LastSalesUpdateDate = CASE WHEN (@Date > @CurrLastSalesUpdateDate) AND (DATEDIFF(day, @Date, GETDATE()) > 0) THEN @Date ELSE @CurrLastSalesUpdateDate END
        WHERE Store_No = @Store_No
        SELECT @Error_No = @@Error
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
        SET NOCOUNT OFF
        RAISERROR ('%s failed with @@ERROR: %d', @Severity, 1, @SectionName, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesAggregation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesAggregation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesAggregation] TO [IRMAReportsRole]
    AS [dbo];

