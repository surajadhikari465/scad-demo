IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemVendorUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemVendorUpdate
GO

CREATE TRIGGER ItemVendorUpdate
    ON ItemVendor
    FOR UPDATE 
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0


    UPDATE ItemVendor
    SET DeleteWorkStation = CASE WHEN Inserted.DeleteDate IS NOT NULL THEN HOST_NAME() ELSE NULL END
    FROM ItemVendor
    INNER JOIN Inserted ON Inserted.Item_Key = ItemVendor.Item_Key AND Inserted.Vendor_ID = ItemVendor.Vendor_ID
    INNER JOIN Deleted ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Vendor_ID = Inserted.Vendor_ID
    WHERE ISNULL(Inserted.DeleteDate, 0) <> ISNULL(Deleted.DeleteDate, 0)
    AND (Inserted.DeleteDate IS NULL OR Deleted.DeleteDate IS NULL)

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        UPDATE StoreItemVendor
        SET DeleteDate = Inserted.DeleteDate, PrimaryVendor = 0 
        FROM StoreItemVendor SIV
        INNER JOIN 
            Inserted 
            ON Inserted.Item_Key = SIV.Item_Key AND Inserted.Vendor_ID = SIV.Vendor_ID
        INNER JOIN 
            Deleted 
            ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Vendor_ID = Inserted.Vendor_ID
        WHERE ISNULL(Inserted.DeleteDate, 0) <> ISNULL(Deleted.DeleteDate, 0)
        AND ISNULL(SIV.DeleteDate, 0) <> ISNULL(Inserted.DeleteDate, 0)
    END
    
    SELECT @Error_No = @@ERROR

    -- Trigger an Item change record to be sent from IRMA to the POS if the Item Vendor ID changes
    IF @Error_No = 0
    BEGIN
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
			SELECT Store_No, Inserted.Item_Key, 2, 'ItemVendorUpdate Trigger'
			FROM Inserted
			INNER JOIN 
				Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Vendor_ID = Inserted.Vendor_ID
			CROSS JOIN
				(SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
			WHERE (Inserted.Item_ID <> Deleted.Item_ID)
				AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Store.Store_No) = 0
    END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemVendorUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO

