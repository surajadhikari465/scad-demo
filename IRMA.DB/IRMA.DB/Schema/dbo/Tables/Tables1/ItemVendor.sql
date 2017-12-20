CREATE TABLE [dbo].[ItemVendor] (
    [Item_Key]                       INT            NOT NULL,
    [Vendor_ID]                      INT            NOT NULL,
    [Item_ID]                        VARCHAR (20)   NULL,
    [DeleteDate]                     SMALLDATETIME  NULL,
    [DeleteWorkStation]              VARCHAR (255)  NULL,
    [CaseDistHandlingChargeOverride] SMALLMONEY     NULL,
    [InsertDate]                     DATETIME       NULL,
    [VendorItemDescription]          VARCHAR (255)  NULL,
    [RetailCasePack]                 DECIMAL (9, 4) NULL,
    [VendorItemStatus]               INT            CONSTRAINT [DF_ItemVendor_VendorItemStatus] DEFAULT ((0)) NULL,
    [IgnoreCasePack]                 BIT            DEFAULT ((0)) NULL,
    [VendorBrand]                    VARCHAR (24)   NULL,
    [VendorPkgDesc]                  VARCHAR (10)   NULL,
    CONSTRAINT [PK_ItemVendor_ItemKey_VendorID] PRIMARY KEY NONCLUSTERED ([Item_Key] ASC, [Vendor_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemVendor_3__16] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK_ItemVendor_Item1] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
ALTER TABLE [dbo].[ItemVendor] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [idxItemVendorItemID]
    ON [dbo].[ItemVendor]([Item_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ItemVendor_VendorId_ItemKey_INC_DelDate_ItemId]
    ON [dbo].[ItemVendor]([Vendor_ID] ASC, [Item_Key] ASC)
    INCLUDE([DeleteDate], [Item_ID]) WITH (FILLFACTOR = 80);


GO
CREATE Trigger ItemVendorAdd 
ON [dbo].[ItemVendor]
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
CREATE TRIGGER ItemVendorUpdate
    ON [dbo].[ItemVendor]
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
    
 -- DETERMINE IF CURRENT REGION BATCHES VENDOR CHANGES --
    DECLARE @BatchVendorChanges bit
    DECLARE @RowCount bit

    SELECT @BatchVendorChanges = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'BatchVendorChanges'

    CREATE TABLE #tempStoreNumbers
    (
		Store_No INT
    )
    
    IF(@BatchVendorChanges = 0)
		BEGIN
	       -- get stores from InstanceDataFlagsStoreOverride
	       INSERT INTO #tempStoreNumbers (Store_No)
	       SELECT SIV.Store_NO 
		   FROM   StoreItemVendor SIV 
		WHERE PrimaryVendor = 1 
			  AND SIV.item_key IN (SELECT item_key FROM Inserted )
	          AND dbo.fn_InstanceDataValue ('BatchVendorChanges', SIV.Store_No) = 1 

	       SET @RowCount = (SELECT Count(*) FROM #tempStoreNumbers)
		END	
	ELSE
		BEGIN
		   INSERT INTO #tempStoreNumbers (Store_No)
	       SELECT Store_No 
		   FROM Store (nolock) 
		   WHERE WFM_Store = 1 OR Mega_Store = 1
	        -- we really do not need the actual count. Setting to 1 so that Item Maintenance  is created
	        SET @RowCount =  1
		END	
	
    -- Trigger an Item change record to be sent from IRMA to the POS if the Item Vendor ID changes
    IF @Error_No = 0 AND @RowCount > 0
    BEGIN
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
			SELECT Store_No, Inserted.Item_Key, 2, 'ItemVendorUpdate Trigger'
			FROM Inserted
			INNER JOIN 
				Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Vendor_ID = Inserted.Vendor_ID
			CROSS JOIN
				#tempStoreNumbers Store
			WHERE (Inserted.Item_ID <> Deleted.Item_ID)
				AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Store.Store_No) = 0
    END

	DROP TABLE #tempStoreNumbers

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
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IMHARole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemVendor] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [ExtractRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemVendor] TO [iCONReportingRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemVendor] TO [MammothRole]
    AS [dbo];