/****** Object:  Trigger [OrderHeaderDel]    Script Date: 05/07/2008 09:01:38 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderDel]'))
DROP TRIGGER [dbo].[OrderHeaderDel]
GO
/****** Object:  Trigger [OrderHeaderDel]    Script Date: 05/07/2008 09:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Trigger [OrderHeaderDel] 
ON dbo.OrderHeader
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	UPDATE OrderExportDeletedQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = NULL
	WHERE OrderHeader_ID in (SELECT OrderHeader_ID FROM Deleted)

	IF @@ROWCOUNT=0
	BEGIN
	    INSERT INTO OrderExportDeletedQueue
		SELECT OrderHeader_ID, GetDate(), NULL FROM Deleted
	END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('OrderHeaderDel trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
