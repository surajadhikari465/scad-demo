/****** Object:  Trigger [VendorInsert]    Script Date: 03/11/2009 13:23:07 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorInsert]'))
DROP TRIGGER [dbo].[VendorInsert]
GO
/****** Object:  Trigger [VendorInsert]    Script Date: 03/11/2009 13:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [VendorInsert]
ON [dbo].[Vendor]
FOR INSERT 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Add to queue for EXE for Vendors who are stores in an EXE warehouse's zone
    INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
	SELECT Supplier_Store_No, Inserted.Vendor_ID, 'A', 1
    FROM Inserted
    INNER JOIN
        Store CustStore ON CustStore.Store_No = Inserted.Store_No
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.Zone_ID = CustStore.Zone_ID
    INNER JOIN
        Store VendStore ON VendStore.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE VendStore.EXEWarehouse IS NOT NULL

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		UPDATE VendorExportQueue
		SET QueueInsertedDate = GetDate(), 
			DeliveredToStoreOpsDate = NULL,
			Old_PS_Vendor_ID = (Select PS_Vendor_ID From Deleted)
		WHERE Vendor_ID in
			(
			SELECT DISTINCT Inserted.Vendor_ID
			FROM 
				Inserted 
			INNER JOIN
				Deleted 
				ON Inserted.Vendor_ID = Deleted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 AND
				(Inserted.PS_Vendor_ID IS NOT NULL OR Inserted.PS_Export_Vendor_ID IS NOT NULL)
			)

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO VendorExportQueue 
			SELECT Inserted.Vendor_ID, GetDate(), NULL, Inserted.PS_Vendor_ID
			FROM Inserted
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 AND 
				(PS_Vendor_ID IS NOT NULL OR PS_Export_Vendor_ID IS NOT NULL)
		END    
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('VendorInsert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
