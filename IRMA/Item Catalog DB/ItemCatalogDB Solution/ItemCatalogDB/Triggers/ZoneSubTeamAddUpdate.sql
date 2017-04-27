IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ZoneSubTeamAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER ZoneSubTeamAddUpdate
GO

CREATE TRIGGER ZoneSubTeamAddUpdate
ON ZoneSubTeam
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Send SubTeam Items to EXE if applicable
    INSERT INTO WarehouseItemChange (Store_No, Item_Key, ChangeType)
    SELECT DISTINCT Store.Store_No, Item.Item_Key, 'A'
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Deleted.Zone_ID = Inserted.Zone_ID AND Deleted.SubTeam_No = Inserted.SubTeam_No
    INNER JOIN
        Item ON Item.SubTeam_No = Inserted.SubTeam_No
    INNER JOIN
        SubTeam ON SubTeam.SubTeam_No = Inserted.SubTeam_No
    INNER JOIN
        Store ON Store.Store_No = Inserted.Supplier_Store_No
    WHERE Deleted_Item = 0 AND Item.EXEDistributed = 1
        AND Store.EXEWarehouse IS NOT NULL
        AND SubTeam.EXEWarehouseSent = 0
        AND NOT EXISTS (SELECT * FROM WarehouseItemChange WIC WHERE WIC.Store_No = Store.Store_No AND WIC.Item_Key = Item.Item_Key AND WIC.ChangeType = 'A')
        AND Inserted.Supplier_Store_No <> ISNULL(Deleted.Supplier_Store_No, 0)

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        -- Add vendor to EXE vendor change queue table if they supply a warehouse that has the EXE system installed
        -- The first part is for Vendors associated with the warehouse's items
        INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
        SELECT DISTINCT Store.Store_No, ItemVendor.Vendor_ID, 'A', 0
        FROM Inserted
        LEFT JOIN
            Deleted
            ON Deleted.Zone_ID = Inserted.Zone_ID AND Deleted.SubTeam_No = Inserted.SubTeam_No
        INNER JOIN
            Item ON Item.SubTeam_No = Inserted.SubTeam_No
        INNER JOIN
            Store ON Store.Store_No = Inserted.Supplier_Store_No
        INNER JOIN
            ItemVendor ON ItemVendor.Item_Key = Item.Item_Key
        INNER JOIN
            Vendor ON Vendor.Vendor_ID = ItemVendor.Vendor_ID
        WHERE Deleted_Item = 0 AND Item.EXEDistributed = 1
            AND Store.EXEWarehouse IS NOT NULL
            AND EXEWarehouseVendSent = 0
            AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = Store.Store_No AND WVC.Vendor_ID = ItemVendor.Vendor_ID AND WVC.ChangeType = 'A' AND WVC.Customer = 0)
            AND Inserted.Supplier_Store_No <> ISNULL(Deleted.Supplier_Store_No, 0)
        UNION
        -- This part is for Vendors who are stores in the warehouse's zone
        SELECT Inserted.Supplier_Store_No, CustVend.Vendor_ID, 'A', 1
        FROM Inserted
        LEFT JOIN
            Deleted
            ON Deleted.Zone_ID = Inserted.Zone_ID AND Deleted.SubTeam_No = Inserted.SubTeam_No
        INNER JOIN
            Store CustStore ON CustStore.Zone_ID = Inserted.Zone_ID
        INNER JOIN
            Vendor CustVend ON CustVend.Store_No = CustStore.Store_No
        INNER JOIN
            Store VendStore ON VendStore.Store_No = Inserted.Supplier_Store_No
        WHERE VendStore.EXEWarehouse IS NOT NULL
            AND EXEWarehouseCustSent = 0
            AND EXISTS (SELECT * FROM Item (nolock) WHERE Item.SubTeam_No = Inserted.SubTeam_No AND Item.EXEDistributed = 1)
            AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = Inserted.Supplier_Store_No AND WVC.Vendor_ID = CustVend.Vendor_ID AND WVC.ChangeType = 'A' AND WVC.Customer = 1)
            AND Inserted.Supplier_Store_No <> ISNULL(Deleted.Supplier_Store_No, 0)        

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ZoneSubTeamAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

