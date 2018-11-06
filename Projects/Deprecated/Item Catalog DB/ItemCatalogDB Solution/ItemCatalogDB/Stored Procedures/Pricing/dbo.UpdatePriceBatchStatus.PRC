SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchStatus') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchStatus
GO

CREATE PROCEDURE dbo.UpdatePriceBatchStatus
    @PriceBatchHeaderID int,
    @PriceBatchStatusID tinyint
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    IF @PriceBatchStatusID = 3 
		IF (SELECT ISNULL(ItemChgTypeID, 0) FROM PriceBatchHeader (NOLOCK) WHERE PriceBatchHeaderID = @PriceBatchHeaderID) = 3
			SELECT @PriceBatchStatusID = 4

    UPDATE PriceBatchHeader 
    SET PriceBatchStatusID = @PriceBatchStatusID,
        PrintedDate = CASE WHEN @PriceBatchStatusID = 4 THEN GETDATE() ELSE PrintedDate END,
        SentDate = CASE WHEN (@PriceBatchStatusID = 5) OR ((@PriceBatchStatusID = 6) AND (SentDate IS NULL)) THEN GETDATE() ELSE SentDate END,
        ProcessedDate = CASE WHEN @PriceBatchStatusID = 6 THEN GETDATE() ELSE ProcessedDate END
    WHERE PriceBatchHeaderID = @PriceBatchHeaderID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        -- Send the description back for display purposes
        SELECT @PriceBatchStatusID, PriceBatchStatusDesc FROM PriceBatchStatus (NOLOCK) WHERE PriceBatchStatusID = @PriceBatchStatusID

        SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no <> 0
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchStatus failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

