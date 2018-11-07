CREATE PROCEDURE dbo.InsertPriceBatchHeader
    @ItemChgTypeID tinyint,
    @PriceChgTypeID tinyint,
    @StartDate smalldatetime,
	@BatchDescription varchar(30),
	@AutoApplyFlag bit,
	@ApplyDate datetime, 
	@POSBatchId as int,
	@ItemUploadHeaderID int = NULL 
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @newIdentity int
    DECLARE @error_no int
    SELECT @error_no = 0

    INSERT INTO PriceBatchHeader (PriceBatchStatusID, ItemChgTypeID, PriceChgTypeID, StartDate, BatchDescription, AutoApplyFlag, ApplyDate, POSBatchId)
    VALUES (1, @ItemChgTypeID, @PriceChgTypeID, @StartDate,	@BatchDescription, @AutoApplyFlag, @ApplyDate, @POSBatchId)

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        SET @newIdentity = SCOPE_IDENTITY() 

		-- if there is a @ItemUploadHeaderID then insert that in its table
		IF @ItemUploadHeaderID IS NOT NULL
		BEGIN
			INSERT INTO dbo.ItemUploadHeaderBatch (ItemUploadHeader_ID, PriceBatchHeaderID)
				VALUES(@ItemUploadHeaderID, @newIdentity)
		END

        SELECT @newIdentity AS PriceBatchHeaderID

        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        SET NOCOUNT OFF
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('InsertPriceBatchHeader failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPriceBatchHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPriceBatchHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPriceBatchHeader] TO [IRMAReportsRole]
    AS [dbo];

