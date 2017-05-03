/****** Object:  Trigger [VendorUpdate]    Script Date: 03/11/2009 13:23:10 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorUpdate]'))
DROP TRIGGER [dbo].[VendorUpdate]
GO
/****** Object:  Trigger [VendorUpdate]    Script Date: 03/11/2009 13:23:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Trigger [VendorUpdate] 
ON [dbo].[Vendor]
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Add vendor to EXE vendor change queue table if they supply a warehouse that has the EXE system installed
    -- The first part is for Vendors associated with the warehouse's items
    INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
    SELECT DISTINCT Store.Store_No, Inserted.Vendor_ID, CASE WHEN Inserted.EXEWarehouseVendSent = 1 THEN 'M' ELSE 'A' END, 0
    FROM Inserted
    INNER JOIN
        Deleted ON Deleted.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        ItemVendor ON ItemVendor.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        Item ON Item.Item_Key = ItemVendor.Item_Key
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        Store ON Store.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE (Inserted.CompanyName <> Deleted.CompanyName
        OR ISNULL(Inserted.Address_Line_1, '') <> ISNULL(Deleted.Address_Line_1, '')
        OR ISNULL(Inserted.Address_Line_2, '') <> ISNULL(Deleted.Address_Line_2, '')
        OR ISNULL(Inserted.City, '') <> ISNULL(Deleted.City, '')
        OR ISNULL(Inserted.State, '') <> ISNULL(Deleted.State, '')
        OR ISNULL(Inserted.Zip_Code, '') <> ISNULL(Deleted.Zip_Code, '')
        OR ISNULL(Inserted.Country, '') <> ISNULL(Deleted.Country, '')
        OR ISNULL(Inserted.Phone, '') <> ISNULL(Deleted.Phone, ''))
        AND Store.EXEWarehouse IS NOT NULL
        AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = Store.Store_No AND WVC.Vendor_ID = Inserted.Vendor_ID AND WVC.Customer = 0 AND WVC.ChangeType = 'A' AND Inserted.EXEWarehouseVendSent = 0)
    UNION
    -- This part is for Vendors who are stores in the warehouse's zone
    SELECT Supplier_Store_No, Inserted.Vendor_ID, CASE WHEN Inserted.EXEWarehouseCustSent = 1 THEN 'M' ELSE 'A' END, 1
    FROM Inserted
    INNER JOIN
        Deleted ON Deleted.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        Store CustStore ON CustStore.Store_No = Inserted.Store_No
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.Zone_ID = CustStore.Zone_ID
    INNER JOIN
        Store VendStore ON VendStore.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE (Inserted.CompanyName <> Deleted.CompanyName
        OR ISNULL(Inserted.Address_Line_1, '') <> ISNULL(Deleted.Address_Line_1, '')
        OR ISNULL(Inserted.Address_Line_2, '') <> ISNULL(Deleted.Address_Line_2, '')
        OR ISNULL(Inserted.City, '') <> ISNULL(Deleted.City, '')
        OR ISNULL(Inserted.State, '') <> ISNULL(Deleted.State, '')
        OR ISNULL(Inserted.Zip_Code, '') <> ISNULL(Deleted.Zip_Code, '')
        OR ISNULL(Inserted.Country, '') <> ISNULL(Deleted.Country, '')
        OR ISNULL(Inserted.Phone, '') <> ISNULL(Deleted.Phone, '')
        OR ISNULL(Inserted.Store_No, 0) <> ISNULL(Deleted.Store_No, 0))
        AND VendStore.EXEWarehouse IS NOT NULL
        AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = ZoneSubTeam.Supplier_Store_No AND WVC.Vendor_ID = Inserted.Vendor_ID AND WVC.Customer = 1 AND WVC.ChangeType = 'A' AND Inserted.EXEWarehouseCustSent = 0)

    SELECT @Error_No = @@ERROR
    
	IF @Error_No = 0 
    BEGIN
		UPDATE VendorExportQueue
		SET QueueInsertedDate = GetDate(), 
			DeliveredToStoreOpsDate = NULL,
			Old_PS_Vendor_ID = (Select PS_Vendor_ID from Deleted)
		WHERE Vendor_ID in
			(
			SELECT DISTINCT Inserted.Vendor_ID
			FROM 
				Inserted 
			INNER JOIN
				Deleted 
				ON Inserted.Vendor_ID = Deleted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 
			)

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO VendorExportQueue
			SELECT Inserted.Vendor_ID, GetDate(), NULL, Inserted.PS_Vendor_ID
			FROM Inserted
			INNER JOIN
				Deleted
				ON Deleted.Vendor_ID = Inserted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 
				-- AND (Inserted.CompanyName <> Deleted.CompanyName
				--OR ISNULL(Inserted.PS_Vendor_ID, '') <> ISNULL(Deleted.PS_Vendor_ID, '')
				--OR ISNULL(Inserted.PS_Export_Vendor_ID, '') <> ISNULL(Deleted.PS_Export_Vendor_ID, ''))
		END

        SELECT @Error_No = @@ERROR
    END
    
    -- DETERMINE IF CURRENT REGION BATCHES VENDOR CHANGES --
    DECLARE @BatchVendorChanges bit
    SELECT @BatchVendorChanges = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'BatchVendorChanges'
    
    -- CREATE ITEM CHANGE TYPE PRICEBATCHDETAIL RECORD IF THIS REGION BATCHES VENDOR CHANGES
    -- CREATE A BATCH RECORD FOR EVERY ITEM THAT THIS VENDOR IS THE PRIMARY FOR
    IF @Error_No = 0 AND @BatchVendorChanges = 1
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT SIV.Store_No, SIV.Item_Key, 2, 'VendorUpdate Trigger'
		FROM Inserted
		INNER JOIN
			Deleted
			ON Deleted.Vendor_ID = Inserted.Vendor_ID 
		INNER JOIN
			StoreItemVendor SIV
			ON SIV.Vendor_ID = Inserted.Vendor_ID      
		WHERE Inserted.CompanyName <> Deleted.CompanyName
			AND SIV.PrimaryVendor = 1
			AND (DeleteDate IS NULL OR DeleteDate > GetDate())
			AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(SIV.Item_Key,SIV.Store_No) = 0
            
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('VendorUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
