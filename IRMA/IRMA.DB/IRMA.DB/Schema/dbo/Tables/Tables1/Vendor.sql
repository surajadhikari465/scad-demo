CREATE TABLE [dbo].[Vendor] (
    [Vendor_ID]                    INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Vendor_Key]                   VARCHAR (10)  NULL,
    [CompanyName]                  VARCHAR (50)  NOT NULL,
    [Address_Line_1]               VARCHAR (50)  NULL,
    [Address_Line_2]               VARCHAR (50)  NULL,
    [City]                         VARCHAR (30)  NULL,
    [State]                        VARCHAR (2)   NULL,
    [Zip_Code]                     VARCHAR (10)  NULL,
    [Country]                      VARCHAR (10)  NULL,
    [Phone]                        VARCHAR (20)  NULL,
    [Fax]                          VARCHAR (20)  NULL,
    [PayTo_CompanyName]            VARCHAR (50)  NULL,
    [PayTo_Attention]              VARCHAR (50)  NULL,
    [PayTo_Address_Line_1]         VARCHAR (50)  NULL,
    [PayTo_Address_Line_2]         VARCHAR (50)  NULL,
    [PayTo_City]                   VARCHAR (30)  NULL,
    [PayTo_State]                  VARCHAR (2)   NULL,
    [PayTo_Zip_Code]               VARCHAR (10)  NULL,
    [PayTo_Country]                VARCHAR (10)  NULL,
    [PayTo_Phone]                  VARCHAR (20)  NULL,
    [PayTo_Fax]                    VARCHAR (20)  NULL,
    [Comment]                      VARCHAR (255) NULL,
    [Customer]                     BIT           CONSTRAINT [DF__Vendor1__Custome__1C873BEC] DEFAULT ((0)) NOT NULL,
    [InternalCustomer]             BIT           CONSTRAINT [DF__Vendor1__Interna__1D7B6025] DEFAULT ((0)) NOT NULL,
    [ActiveVendor]                 BIT           CONSTRAINT [DF__Vendor1__ActiveV__1E6F845E] DEFAULT ((0)) NOT NULL,
    [Store_no]                     INT           NULL,
    [Order_By_Distribution]        BIT           CONSTRAINT [DF__Vendor1__User4__2334397B] DEFAULT ((0)) NOT NULL,
    [Electronic_Transfer]          BIT           CONSTRAINT [DF__Vendor1__User5__24285DB4] DEFAULT ((0)) NOT NULL,
    [User_ID]                      INT           NULL,
    [Phone_Ext]                    VARCHAR (5)   NULL,
    [PayTo_Phone_Ext]              VARCHAR (5)   NULL,
    [PS_Vendor_ID]                 VARCHAR (10)  NULL,
    [PS_Location_Code]             VARCHAR (10)  CONSTRAINT [DF_Vendor_PS_Location_Code] DEFAULT ('DEFAULT') NULL,
    [PS_Address_Sequence]          VARCHAR (2)   NULL,
    [WFM]                          BIT           CONSTRAINT [DF__vendor__WFM__2E82F504] DEFAULT ((0)) NOT NULL,
    [FTP_Addr]                     VARCHAR (255) NULL,
    [FTP_Path]                     VARCHAR (255) NULL,
    [FTP_User]                     VARCHAR (255) NULL,
    [FTP_Password]                 VARCHAR (255) NULL,
    [Non_Product_Vendor]           TINYINT       CONSTRAINT [DF__vendor__Non_Prod__7CAD38FC] DEFAULT ((0)) NOT NULL,
    [Default_GLNumber]             VARCHAR (10)  NULL,
    [Email]                        VARCHAR (50)  NULL,
    [EFT]                          BIT           CONSTRAINT [DF_Vendor_EFT] DEFAULT ((0)) NOT NULL,
    [InStoreManufacturedProducts]  BIT           CONSTRAINT [DF_Vendor_InStoreManufacturedProducts] DEFAULT ((0)) NOT NULL,
    [EXEWarehouseVendSent]         BIT           CONSTRAINT [DF_Vendor_EXEWarehouseVendSent] DEFAULT ((0)) NOT NULL,
    [EXEWarehouseCustSent]         BIT           CONSTRAINT [DF_Vendor_EXEWarehouseCustSent] DEFAULT ((0)) NOT NULL,
    [County]                       VARCHAR (20)  NULL,
    [PayTo_County]                 VARCHAR (20)  NULL,
    [AddVendor]                    BIT           CONSTRAINT [DF_Vendor_AddVendor] DEFAULT ((0)) NOT NULL,
    [Po_Note]                      VARCHAR (150) NULL,
    [Receiving_Authorization_Note] VARCHAR (150) NULL,
    [Other_Name]                   VARCHAR (35)  NULL,
    [PS_Export_Vendor_ID]          VARCHAR (10)  NULL,
    [File_Type]                    VARCHAR (15)  NULL,
    [CaseDistHandlingCharge]       SMALLMONEY    NULL,
    [EInvoicing]                   BIT           DEFAULT ((0)) NOT NULL,
    [POTransmissionTypeID]         INT           NULL,
    [CurrencyID]                   INT           NULL,
    [AccountingContactEmail]       VARCHAR (50)  NULL,
    [LeadTimeDays]                 INT           CONSTRAINT [DF_Vendor_LeadTimeDays] DEFAULT ((0)) NOT NULL,
    [LeadTimeDayOfWeek]            TINYINT       NULL,
    [PaymentTermID]                INT           NULL,
    [EinvoiceRequired]             BIT           DEFAULT ((0)) NOT NULL,
    [AllowReceiveAll]              BIT           CONSTRAINT [DF_Vendor_AllowReceiveAll] DEFAULT ((0)) NOT NULL,
    [ShortpayProhibited]           BIT           DEFAULT ((0)) NOT NULL,
    [AllowBarcodePOReport]         BIT           CONSTRAINT [DF_Vendor_AllowBarcodePOReport] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Vendor_Vendor_ID] PRIMARY KEY CLUSTERED ([Vendor_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Currency_Vendor] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[Currency] ([CurrencyID]),
    CONSTRAINT [FK_Vendor_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_Vendor_POTransmissionType] FOREIGN KEY ([POTransmissionTypeID]) REFERENCES [dbo].[POTransmissionType] ([POTransmissionTypeID]),
    CONSTRAINT [FK_Vendor_Store1] FOREIGN KEY ([Store_no]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
ALTER TABLE [dbo].[Vendor] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [idxVendorCompany]
    ON [dbo].[Vendor]([CompanyName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxVendorStoreNo]
    ON [dbo].[Vendor]([Store_no] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxOrganizationUserID]
    ON [dbo].[Vendor]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxBatchHeaderVendor]
    ON [dbo].[Vendor]([Vendor_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_CompanyName]
    ON [dbo].[Vendor]([CompanyName] ASC)
    INCLUDE([Vendor_ID], [Store_no], [PS_Vendor_ID], [WFM]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_StoreNo_VendorID]
    ON [dbo].[Vendor]([Store_no] ASC, [Vendor_ID] ASC)
    INCLUDE([State]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_Store_no_CompanyName]
    ON [dbo].[Vendor]([Store_no] ASC, [CompanyName] ASC)
    INCLUDE([Vendor_ID], [Vendor_Key]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_ID_Store_no]
    ON [dbo].[Vendor]([Vendor_ID] ASC, [Store_no] ASC);


GO
CREATE STATISTICS [_dta_stat_Vendor_001]
    ON [dbo].[Vendor]([Store_no], [CompanyName]);


GO
CREATE STATISTICS [_dta_stat_InternalCustomer_Vendor_ID]
    ON [dbo].[Vendor]([InternalCustomer], [Vendor_ID]);


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

	IF @Error_No = 0
	BEGIN
		DECLARE @mammothUpdates dbo.ItemKeyAndStoreNoType

		INSERT INTO @mammothUpdates(
			Item_Key, 
			Store_No)
		SELECT 
			siv.Item_Key, 
			siv.Store_no
		FROM inserted i
		JOIN deleted d on i.Vendor_ID = d.Vendor_ID
		JOIN StoreItemVendor siv ON i.Vendor_ID = siv.Vendor_ID
		WHERE (i.CompanyName <> d.CompanyName
			OR i.Vendor_Key <> d.Vendor_Key)
			AND siv.PrimaryVendor = 1
			AND siv.DeleteDate IS NULL

		IF EXISTS (SELECT TOP 1 1 FROM @mammothUpdates)
			EXEC mammoth.GenerateEventsByItemKeyAndStoreNoType @mammothUpdates, 'ItemLocaleAddOrUpdate'

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
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [ExtractRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Vendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Vendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Vendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [spice_user]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Vendor] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[Vendor] TO [MammothRole]
    AS [dbo];

GO
GRANT ALTER
    ON OBJECT::[dbo].[Vendor] TO [IRMAAdminRole]
    AS [dbo];

GO
GRANT ALTER
    ON OBJECT::[dbo].[Vendor] TO [IRMAClientRole]
    AS [dbo];