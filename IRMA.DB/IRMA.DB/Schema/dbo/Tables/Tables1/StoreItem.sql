CREATE TABLE [dbo].[StoreItem] (
    [StoreItemAuthorizationID] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Store_No]                 INT NOT NULL,
    [Item_Key]                 INT NOT NULL,
    [Authorized]               BIT DEFAULT ((0)) NOT NULL,
    [POSDeAuth]                BIT DEFAULT ((0)) NOT NULL,
    [ScaleAuth]                BIT DEFAULT ((0)) NOT NULL,
    [ScaleDeAuth]              BIT DEFAULT ((0)) NOT NULL,
    [Refresh]                  BIT CONSTRAINT [DF_StoreItem_Refresh] DEFAULT ((0)) NOT NULL,
    [ECommerce]                BIT DEFAULT ((0)) NULL,
    CONSTRAINT [PK_StoreItem_StoreItemAuthorizationID] PRIMARY KEY CLUSTERED ([StoreItemAuthorizationID] ASC),
    CONSTRAINT [FK__StoreItem_ItemKey] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK__StoreItem_StoreNo] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
ALTER TABLE [dbo].[StoreItem] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItem_ItemKeyStoreNo]
    ON [dbo].[StoreItem]([Store_No] ASC, [Item_Key] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItem_ItemKey_INC_StoreNo_Auth_ScaleAuth]
    ON [dbo].[StoreItem]([Item_Key] ASC)
    INCLUDE([Store_No], [Authorized], [ScaleAuth]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItem_POSDeAuth]
    ON [dbo].[StoreItem]([POSDeAuth] ASC)
    INCLUDE([StoreItemAuthorizationID], [Store_No], [Item_Key]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItem_ScaleAuth]
    ON [dbo].[StoreItem]([ScaleAuth] ASC)
    INCLUDE([StoreItemAuthorizationID], [Store_No], [Item_Key]) WITH (FILLFACTOR = 80);


GO

CREATE TRIGGER [dbo].[StoreItemAddUpdate]
ON [dbo].[StoreItem]
FOR INSERT, UPDATE 
AS 
BEGIN
    -- WHEN THE ITEM AUTHORIZATION FOR A STORE CHANGES FROM DE-AUTHORIZED TO AUTHORIZED:
    --		1. If the de-auth has already been communicated to the POS (StoreItem.POSDeAuth = 0),
    --				a 'New' item change type is inserted into PBD to send the auth as a new item record to the POS;
    --				if there is already an 'Item' item change type record in PBD, it is updated from 'Item' to 'New' change type
    --		   If the de-auth has not been communicated to the POS (StoreItem.POSDeAuth = 1),
    --				no changes are made to PBD
    --		2. For scale items, if the de-auth has already been communicated to the scale (StoreItem.ScaleDeAuth = 0),
    --				the StoreItem.ScaleAuth is set to 1 to send the auth to the scale
    --		   If the de-auth has not been communicated to the scale (StoreItem.ScaleDeAuth = 1),
    --				no changes are made to StoreItem.ScaleAuth
    --		3. The StoreItem.POSDeAuth and StoreItem.ScaleDeAuth flags are set to 0
    
	BEGIN TRY
		-- Communicate authorization to the POS if necessary (#1)
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication,ReAuthFlag)
			SELECT Inserted.Store_No, Inserted.Item_Key, 1, 'StoreItemAddUpdate Trigger',Inserted.Authorized
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN Store (nolock) ON
				Store.Store_No = Inserted.Store_No
			WHERE (WFM_Store = 1 OR Mega_Store = 1)
			AND Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
			AND Inserted.Authorized = 1						-- the item is now authorized
			AND Deleted.POSDeAuth = 0						-- the de-auth was already communicated to the POS
			AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0
    
		UPDATE PriceBatchDetail 
			SET ItemChgTypeID = 1,ReAuthFlag = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN PriceBatchDetail PBD ON
				PBD.Item_Key = Inserted.Item_Key
				AND PBD.Store_No = Inserted.Store_No
				AND PBD.ItemChgTypeID = 2
				AND PriceBatchHeaderID IS NULL
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 1					-- the item is now authorized
				AND Deleted.POSDeAuth = 0					-- the de-auth was already communicated to the POS
          
		-- Communicate authorization to the scale if necessary (#2)         
		UPDATE StoreItem SET
				ScaleAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN ItemIdentifier ON
				ItemIdentifier.Item_Key = Inserted.Item_Key
				AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 1					-- the item is now authorized
				AND Deleted.ScaleDeAuth = 0					-- the de-auth was already communicated to the scale
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No

		-- And add it to PLUMCorpChgQueue.  Using distinct will prevent duplicate entries caused by an item with multiple identifiers.
		INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT distinct
				Inserted.Item_Key, 'A', Inserted.Store_No
			FROM 
				Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN ItemIdentifier ON
				ItemIdentifier.Item_Key = Inserted.Item_Key
				AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
			INNER JOIN Store S ON
				Inserted.Store_No = S.Store_No
			WHERE 
				(S.WFM_Store = 1 OR S.Mega_Store = 1)
				AND Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 1						-- the item is now authorized
				AND Deleted.ScaleDeAuth = 0						
				AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')
				AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')

		-- 365 non-scale PLUs which are marked Send to Scale should generate Add maintenance for 365 stores only.
		declare @Is365NonScalePlu bit =		case 
												when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key where icfs.SendToScale = 1) then 1
												else 0
											end

		if @Is365NonScalePlu = 1
			begin
				INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
					SELECT 
						inserted.Item_Key, 'A', Inserted.Store_No
					FROM 
						Inserted
						INNER JOIN Deleted ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Store_No = Inserted.Store_No
						INNER JOIN Store S ON Inserted.Store_No = S.Store_No
					WHERE 
						(S.WFM_Store = 0 AND S.Mega_Store = 1)
						AND Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
						AND Inserted.Authorized = 1						-- the item is now authorized
						AND Deleted.ScaleDeAuth = 0						
						AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')
						AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')
			end

		-- Clean up the de-auth flags (#3)
		UPDATE StoreItem SET
				POSDeAuth = 0,
				ScaleDeAuth = 0
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized = 1 -- the item is authorized
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No
	
	
		-- WHEN THE ITEM AUTHORIZATION FOR A STORE CHANGES FROM AUTHORIZED TO DE-AUTHORIZED:
		--		1. Sets StoreItem.POSDeAuth = 1 to communicate the de-auth to the POS 
		--		2. For scale items, sets StoreItem.ScaleDeAuth = 1 to send the de-auth to the scale
		--		3. The StoreItem.ScaleAuth flags is set to 0

		-- Communicate de-authorization to the POS if necessary (#1)
		UPDATE StoreItem SET
				POSDeAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 0					-- the item is now de-authorized
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No

		-- Communicate de-authorization to the scale if necessary (#2)
		UPDATE StoreItem SET
				ScaleDeAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN ItemIdentifier ON
				ItemIdentifier.Item_Key = Inserted.Item_Key
				AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 0					-- the item is now de-authorized
				AND Deleted.ScaleAuth = 0					-- the auth was already communicated to the scale
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No

		-- Communicate de-authorization to the PriceBatchDenorm
		INSERT INTO PriceBatchDenorm (
				Item_Key,
				Store_No,
				Check_Box_20,
				Text_10,
				IsAuthorized)
			SELECT 
			Inserted.Item_Key,
			Inserted.Store_No,
			1,
			'DEAUTH',
			Inserted.Authorized
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 0					-- the item is now de-authorized

		-- Communicate ECommerce to the PriceBatchDenorm
		INSERT INTO PriceBatchDenorm (
				Item_Key,
				Store_No,
				Check_Box_20,
				Text_10,
				ECommerce)
			SELECT 
			Inserted.Item_Key,
			Inserted.Store_No,
			1,
			'ECOM',
			Inserted.ECommerce
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE ISNULL(Inserted.ECommerce,0) <> ISNULL(Deleted.ECommerce,0)	-- the ECommerce changed

		-- Clean up the auth flags (#3)
		UPDATE StoreItem SET
				ScaleAuth = 0
			FROM StoreItem
			INNER JOIN Inserted ON
				StoreItem.Item_Key = Inserted.Item_Key 
				AND StoreItem.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized = 0					-- the item is de-authorized

    SELECT IDENTITY(INT,1,1) as RowNumber, 
		   Inserted.Store_No as Store_No, 
		   Inserted.Item_Key as Item_Key
	INTO #tmpData
	FROM Inserted
		INNER JOIN Deleted ON
		Deleted.Item_Key = Inserted.Item_Key 
		AND Deleted.Store_No = Inserted.Store_No
		WHERE Inserted.Authorized <> Deleted.Authorized	
			  AND Inserted.Authorized = 0		 -- item got Deauthorized
	
	DECLARE @RowCount INT = (SELECT COUNT(*) FROM #tmpData)

	DECLARE @CurrentRowNumber INT = 1
	DECLARE @Item_Key INT
	DECLARE @Store_No INT

	WHILE (@CurrentRowNumber <= @RowCount)
	BEGIN
	    SELECT @Item_Key= Item_Key, @Store_No = Store_No FROM #tmpData WHERE RowNumber = @CurrentRowNumber
	  	EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, @Store_No, 'ItemDeauthorization'

		SET @CurrentRowNumber = @CurrentRowNumber + 1
	END

	DROP Table #tmpData
   

    END TRY
	BEGIN CATCH
		IF @@TRANCOUNT <> 0 ROLLBACK TRAN

		DECLARE 
			@ErrorMessage NVARCHAR(MAX) = 'StoreItemAddUpdate trigger failed with error: ' + ERROR_MESSAGE(),
			@ErrorSeverity INT = ERROR_SEVERITY(),
			@ErrorState INT = ERROR_STATE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
	END CATCH
END

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [ExtractRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [spice_user]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItem] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [MammothRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItem] TO [IRMAPDXExtractRole]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This flag is set to 1 when a user selects the Refresh checkbox from the UI or the refresh function in SLIM.  It is set to 0 when the batching process sends the refresh message.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StoreItem', @level2type = N'COLUMN', @level2name = N'Refresh';

