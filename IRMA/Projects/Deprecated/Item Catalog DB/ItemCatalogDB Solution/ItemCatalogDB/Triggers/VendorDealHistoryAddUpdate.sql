
-- VendorDealHistory
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'VendorDealHistoryAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[VendorDealHistoryAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[VendorDealHistoryAddUpdate] ON [dbo].[VendorDealHistory] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    UPDATE VendorDealHistory
		SET LastUpdateTimestamp = GetDate()
	FROM Inserted i
	WHERE VendorDealHistory.VendorDealHistoryId = i.VendorDealHistoryId

	UPDATE StoreItemVendor
		SET LastCostAddedDate = GETDATE()
		FROM StoreItemVendor SIV
		INNER JOIN Inserted INS ON SIV.StoreItemVendorID = INS.StoreItemVendorID    

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('VendorDealHistoryAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
