 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].CommitGLUploadInventoryAdjustment') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].CommitGLUploadInventoryAdjustment
GO

CREATE PROCEDURE [dbo].[CommitGLUploadInventoryAdjustment]
    @Store_No int,
    @CurrDate datetime = null,
	@StartDate datetime = null,
	@EndDate datetime = null
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

	IF @StartDate = NULL OR @EndDate = NULL
		BEGIN
			-- previous Monday thru Sunday
			SELECT @StartDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 1 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())) - 6, ISNULL(@CurrDate, GETDATE())), 101)),
				   @EndDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 2 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())), ISNULL(@CurrDate, GETDATE())), 101))
		END
    
    BEGIN TRAN

	UPDATE ItemHistoryUpload SET ItemHistoryUpload.AccountingUploadDate = GetDate()
	FROM ItemHistoryUpload
	INNER JOIN ItemHistory 
		ON ItemHistory.ItemHistoryId = ItemHistoryUpload.ItemHistoryId
	WHERE ItemHistoryUpload.AccountingUploadDate IS NULL 
	  AND ItemHistory.InventoryAdjustmentCode_Id IS NOT NULL
	  AND DATEDIFF(day, @StartDate, DateStamp) >= 0
	  AND DATEDIFF(day, DateStamp, @EndDate) >= 0

    
    SELECT @Error_No = @@ERROR
    
    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('CommitGLUploadInventoryAdjustment failed with @@ERROR: %d', @Severity, 1, @Error_No)       
    END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 