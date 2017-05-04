IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'ItemVendorAdd')
	BEGIN
		PRINT 'Dropping Trigger ItemVendorAdd'
		DROP  Trigger ItemVendorAdd
	END
GO


PRINT 'Creating Trigger ItemVendorAdd'
GO
CREATE Trigger ItemVendorAdd 
ON ItemVendor
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Add vendor to EXE vendor change queue table if they supply an item to a warehouse that has the EXE system installed
    INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
    SELECT DISTINCT Store.Store_No, Inserted.Vendor_ID, 'A', 0
    FROM Inserted
    INNER JOIN
        Item (nolock) ON Item.Item_Key = Inserted.Item_Key
    INNER JOIN
        ZoneSubTeam (nolock) ON ZoneSubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        Store (nolock) ON Store.Store_No = ZoneSubTeam.Supplier_Store_No
    INNER JOIN
        Vendor (nolock) ON Vendor.Vendor_ID = Inserted.Vendor_ID
    WHERE Item.EXEDistributed = 1
        AND Store.EXEWarehouse IS NOT NULL
        AND EXEWarehouseVendSent = 0
        AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = Store.Store_No AND WVC.Vendor_ID = Inserted.Vendor_ID AND WVC.Customer = 0)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemVendorAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
