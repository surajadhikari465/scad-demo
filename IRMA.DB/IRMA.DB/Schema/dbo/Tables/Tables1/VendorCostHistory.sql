CREATE TABLE [dbo].[VendorCostHistory] (
    [VendorCostHistoryID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StoreItemVendorID]   INT            NOT NULL,
    [Promotional]         BIT            CONSTRAINT [DF_CostHistory_Promotional] DEFAULT ((0)) NOT NULL,
    [UnitCost]            SMALLMONEY     NOT NULL,
    [UnitFreight]         SMALLMONEY     NULL,
    [Package_Desc1]       DECIMAL (9, 4) NOT NULL,
    [StartDate]           SMALLDATETIME  CONSTRAINT [DF_VendorCostHistory_StartDate] DEFAULT (CONVERT([smalldatetime],CONVERT([varchar](255),getdate(),(101)),0)) NOT NULL,
    [EndDate]             SMALLDATETIME  CONSTRAINT [DF_VendorCostHistory_EndDate] DEFAULT ('2079-06-06') NOT NULL,
    [FromVendor]          BIT            CONSTRAINT [DF_CostHistory_FromVendor] DEFAULT ((0)) NOT NULL,
    [MSRP]                SMALLMONEY     NULL,
    [InsertDate]          DATETIME       CONSTRAINT [DF_CostHistory_InsertDate] DEFAULT (getdate()) NOT NULL,
    [InsertWorkStation]   VARCHAR (255)  CONSTRAINT [DF_VendorCostHistory_InsertWorkStation] DEFAULT (host_name()) NOT NULL,
    [CostUnit_ID]         INT            NOT NULL,
    [FreightUnit_ID]      INT            NOT NULL,
    [IsFromJDASync]       BIT            DEFAULT ((0)) NULL,
    [Currency]            INT            NULL,
    CONSTRAINT [PK_CostHistory] PRIMARY KEY CLUSTERED ([VendorCostHistoryID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CostHistory_StoreItemVendor] FOREIGN KEY ([StoreItemVendorID]) REFERENCES [dbo].[StoreItemVendor] ([StoreItemVendorID]),
    CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID] FOREIGN KEY ([CostUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID] FOREIGN KEY ([FreightUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID])
);


GO
ALTER TABLE [dbo].[VendorCostHistory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_VendorCostHistoryStoreItemVendor]
    ON [dbo].[VendorCostHistory]([StoreItemVendorID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_VendorCostHistoryStartEndDates]
    ON [dbo].[VendorCostHistory]([StartDate] ASC, [EndDate] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_VendorCostHistory_VendorCostHistoryID_StoreItemVendorID]
    ON [dbo].[VendorCostHistory]([VendorCostHistoryID] ASC, [StoreItemVendorID] ASC)
    INCLUDE([Promotional], [UnitFreight], [StartDate], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_VendorCostHistory_StoreItemVendorID_VendorCostHistoryID]
    ON [dbo].[VendorCostHistory]([StoreItemVendorID] ASC, [VendorCostHistoryID] ASC)
    INCLUDE([Promotional], [UnitFreight], [StartDate], [EndDate]) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER [dbo].[VendorCostHistoryAdd]
ON [dbo].[VendorCostHistory]
FOR INSERT
AS 
BEGIN
    DECLARE @error_no int
    SET @error_no = 0
    
       -- Create average cost history if costing in-store manufactured products (vendor is "in-store manufacturer" or the vendor is the store)
        -- NOTE: This insert also happens in the Item table update trigger if the Item SubTeam_No changes
        INSERT INTO AvgCostHistory(Item_Key, Store_No, SubTeam_No, AvgCost)
        SELECT	SIV.Item_Key,
				SIV.Store_No, 
				Item.SubTeam_No, 
				Inserted.UnitCost + ISNULL(Inserted.UnitFreight, 0)
        FROM Inserted
        INNER JOIN StoreItemVendor SIV (nolock)
            ON SIV.StoreItemVendorID = Inserted.StoreItemVendorID
        INNER JOIN Vendor (nolock)
            ON Vendor.Vendor_ID = SIV.Vendor_ID
        INNER JOIN Item (nolock)
            ON Item.Item_Key = SIV.Item_Key
		INNER JOIN Store (nolock)
            ON Store.Store_No = SIV.Store_No
        WHERE	
         -- There are no other vendors other than the InStoreManufacturer for this store
		 NOT EXISTS (SELECT * FROM StoreItemVendor SIV2 (nolock) WHERE SIV2.Store_No = SIV.Store_No AND SIV2.Item_Key = SIV.Item_Key 
					AND SIV2.Vendor_ID <> SIV.Vendor_ID AND DeleteDate IS NULL)
		 AND Vendor.InStoreManufacturedProducts = 1 
		 AND Store.UseAvgCostHistory = 1
         AND ISNULL((SELECT TOP 1 AvgCost
                        FROM AvgCostHistory H (nolock)
                        WHERE H.Item_Key = SIV.Item_Key
                            AND H.Store_No = SIV.Store_No
                            AND H.SubTeam_No = Item.SubTeam_No
                        ORDER BY Effective_Date DESC), 0) <> (Inserted.UnitCost + ISNULL(Inserted.UnitFreight, 0))


        SELECT @Error_No = @@ERROR
    
    -- DETERMINE IF CURRENT REGION BATCHES COST CHANGES --
    DECLARE @BatchCostChanges bit
    SELECT @BatchCostChanges = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'BatchCostChanges'
           
    IF @Error_No = 0 AND @BatchCostChanges = 1
	BEGIN
	
		-- CREATE BATCH RECORD FOR ALL PRIMARY VENDOR COST CHANGES WHEN InstanceDataFlags BatchCostChanges = 1 --
		-- CREATE COST CHANGE RECORD			
		INSERT INTO PriceBatchDetail (Item_Key, Store_No, ItemChgTypeID, StartDate, InsertApplication)
		SELECT SIV.Item_Key, SIV.Store_No, 2, Inserted.StartDate, 'VendorCostHistoryAdd Trigger'
		FROM Inserted
		INNER JOIN
			StoreItemVendor SIV (nolock)
			ON SIV.StoreItemVendorID = Inserted.StoreItemVendorID
		INNER JOIN
			Store (nolock)
			ON Store.Store_No = SIV.Store_No			
		WHERE SIV.PrimaryVendor = 1
			AND (Mega_Store = 1 OR WFM_Store = 1)
			AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(SIV.Item_Key, SIV.Store_No) = 0

		SELECT @Error_No = @@ERROR
		
		-- Insert the Off Cost record - the process will get the price from the Price record when the record is batched
		IF (@Error_No = 0)
		BEGIN
			DECLARE @InsertedStoreItemVendorID int
			DECLARE @InsertedStartDate datetime
			DECLARE @InsertedEndDate datetime
			
			SELECT @InsertedStoreItemVendorID = StoreItemVendorID, @InsertedStartDate = StartDate, @InsertedEndDate = EndDate FROM Inserted
				
			-- Clean up Off Cost changes within this promo date range		
			DELETE FROM PriceBatchDetail
			WHERE Item_Key IN (SELECT Item_Key 
								FROM StoreItemVendor									
								WHERE StoreItemVendorID = @InsertedStoreItemVendorID)
				AND Store_No IN (SELECT Store_No 
								FROM StoreItemVendor									
								WHERE StoreItemVendorID = @InsertedStoreItemVendorID)
				AND ItemChgTypeID = 6
				AND StartDate >= @InsertedStartDate
				AND StartDate <= DATEADD(day, 1, @InsertedEndDate)
				AND (PriceBatchHeaderID IS NULL 
					  OR (PriceBatchHeaderID IS NOT NULL AND (SELECT PriceBatchStatusID FROM PriceBatchHeader PBH 
															 WHERE PBH.PriceBatchHeaderID = PriceBatchHeaderID) < 2))
			
			SELECT @Error_No = @@ERROR
					
			IF @Error_No = 0
			BEGIN
				-- IF COST IS A 'PROMO' COST --> CREATE COST CHANGE END-PROMO RECORD FOR PROMO END DATE (+1 DAY)
				INSERT INTO PriceBatchDetail (Item_Key, Store_No, ItemChgTypeID, StartDate, InsertApplication)
				SELECT SIV.Item_Key, SIV.Store_No, 6, DATEADD(day, 1, Inserted.EndDate), 'VendorCostHistoryAdd Trigger'
				FROM Inserted
				INNER JOIN
					StoreItemVendor SIV (nolock)
					ON SIV.StoreItemVendorID = Inserted.StoreItemVendorID
				INNER JOIN
					Store (nolock)
					ON Store.Store_No = SIV.Store_No			
				WHERE SIV.PrimaryVendor = 1
					AND Inserted.Promotional = 1 --ONLY CREATE OFF-COST RECORDS FOR PROMO COST
					AND (Mega_Store = 1 OR WFM_Store = 1)
					AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(SIV.Item_Key, SIV.Store_No) = 0

				SELECT @Error_No = @@ERROR
			END			
		END    
	END

	--MD 07/01/2009: Added update to SIV for P2P process	
	IF @Error_No = 0
	BEGIN
		UPDATE StoreItemVendor
			SET LastCostAddedDate = GETDATE()
			FROM StoreItemVendor SIV
			INNER JOIN Inserted INS ON SIV.StoreItemVendorID = INS.StoreItemVendorID    
		SELECT @Error_No = @@ERROR
	END

    IF @error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('VendorCostHistoryAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAReports]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IMHARole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorCostHistory] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistory] TO [MammothRole]
    AS [dbo];