SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteItemInventory')
	EXEC('CREATE PROCEDURE [dbo].[DeleteItemInventory] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE dbo.DeleteItemInventory 
    @Item_Key int,
	@UserID int,
    @StartDate smalldatetime
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

    UPDATE Item
    SET Remove_Item = 1,
		LastModifiedUser_ID = @UserID,
		LastModifiedDate = @StartDate
    WHERE Item_Key = @Item_Key

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        DELETE PriceBatchDetail
        FROM PriceBatchDetail PBD
        LEFT JOIN
            PriceBatchHeader PBH
            ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
        WHERE PBD.Item_Key = @Item_Key
            AND ISNULL(PriceBatchStatusID, 0) < 2
            AND ISNULL(PBD.StartDate, @StartDate) >= @StartDate
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, StartDate)
        SELECT Store_No, @Item_Key, 3, @StartDate
        FROM 
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        
            
        SELECT @error_no = @@ERROR
    END

	IF @error_no = 0
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemDelete'
		SELECT @error_no = @@ERROR
	END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('DeleteItemInventory failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

