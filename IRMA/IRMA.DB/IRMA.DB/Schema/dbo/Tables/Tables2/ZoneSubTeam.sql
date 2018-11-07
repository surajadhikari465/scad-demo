CREATE TABLE [dbo].[ZoneSubTeam] (
    [Zone_ID]           INT           NOT NULL,
    [SubTeam_No]        INT           NOT NULL,
    [Supplier_Store_No] INT           NOT NULL,
    [OrderStart]        SMALLDATETIME NULL,
    [OrderEnd]          SMALLDATETIME NULL,
    [OrderEndTransfers] SMALLDATETIME NULL,
    CONSTRAINT [PK_ZoneSubTeam_ZoneId_SubTeam_SupplierStore] PRIMARY KEY CLUSTERED ([Zone_ID] ASC, [SubTeam_No] ASC, [Supplier_Store_No] ASC),
    CONSTRAINT [FK__ZoneSubTe__Suppl__0BBFCFF9] FOREIGN KEY ([Supplier_Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_From_ZoneSubTeam_SubTeam_SubTeam_No] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_Zone_Zone_ID] FOREIGN KEY ([Zone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxSupplierZoneSubTeam]
    ON [dbo].[ZoneSubTeam]([Supplier_Store_No] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER ZoneSubTeamAddUpdate
ON [dbo].[ZoneSubTeam]
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
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSubTeam] TO [IRMAReportsRole]
    AS [dbo];

