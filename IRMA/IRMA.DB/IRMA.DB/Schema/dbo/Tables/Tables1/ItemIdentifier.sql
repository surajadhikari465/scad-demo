CREATE TABLE [dbo].[ItemIdentifier] (
    [Identifier_ID]           INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Key]                INT          NOT NULL,
    [Identifier]              VARCHAR (13) NOT NULL,
    [Default_Identifier]      TINYINT      CONSTRAINT [DF__ItemIdent__Defau__47C20D6C] DEFAULT ((0)) NOT NULL,
    [Deleted_Identifier]      TINYINT      CONSTRAINT [DF__ItemIdent__Delet__48B631A5] DEFAULT ((0)) NOT NULL,
    [Add_Identifier]          TINYINT      CONSTRAINT [DF__ItemIdent__Add_I__49AA55DE] DEFAULT ((0)) NOT NULL,
    [Remove_Identifier]       TINYINT      CONSTRAINT [DF__ItemIdent__Remov__4A9E7A17] DEFAULT ((0)) NOT NULL,
    [National_Identifier]     TINYINT      CONSTRAINT [DF_ItemIdentifier_National_Identifier] DEFAULT ((0)) NOT NULL,
    [CheckDigit]              CHAR (1)     NULL,
    [IdentifierType]          CHAR (1)     NULL,
    [NumPluDigitsSentToScale] INT          NULL,
    [Scale_Identifier]        BIT          CONSTRAINT [DF_ItemIdentifier_Scale_Identifier] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ItemIdentifier_Identifier_ID] PRIMARY KEY CLUSTERED ([Identifier_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__ItemIdent__Item___46CDE933] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
ALTER TABLE [dbo].[ItemIdentifier] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [idxIdentifierItemKey]
    ON [dbo].[ItemIdentifier]([Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxIdentifierDefaultIdentifier]
    ON [dbo].[ItemIdentifier]([Deleted_Identifier] ASC, [Default_Identifier] ASC, [Identifier] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxIdentifierIdentifierDefault]
    ON [dbo].[ItemIdentifier]([Deleted_Identifier] ASC, [Identifier] ASC, [Default_Identifier] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxIdentifierAdd_Identifier]
    ON [dbo].[ItemIdentifier]([Add_Identifier] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxIdentifierRemove_Identifier]
    ON [dbo].[ItemIdentifier]([Remove_Identifier] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxDefaultIdentifier]
    ON [dbo].[ItemIdentifier]([Default_Identifier] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_ItemIdentifier_Identifier_IdentifierID_ItemKey]
    ON [dbo].[ItemIdentifier]([Identifier] ASC, [Identifier_ID] ASC, [Item_Key] ASC);


GO
CREATE STATISTICS [_dta_stat_ItemIdentifier_001]
    ON [dbo].[ItemIdentifier]([Item_Key], [Identifier_ID]);


GO
CREATE STATISTICS [_dta_stat_ItemIdentifier_002]
    ON [dbo].[ItemIdentifier]([Identifier_ID], [Identifier], [Item_Key]);


GO

CREATE TRIGGER ItemIdentifierAdd
ON [dbo].[ItemIdentifier]
FOR INSERT 
AS 
BEGIN
    BEGIN TRY
		-- 365 non-scale PLUs which are marked Send to Scale should generate Add maintenance for 365 stores only.
		declare @Is365NonScalePlu bit =		case 
												when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key where icfs.SendToScale = 1) then 1
												else 0
											end

		DELETE 
			PLUMCorpChgQueue
		FROM 
			PLUMCorpChgQueue
			INNER JOIN Inserted ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
		WHERE 
			Inserted.Scale_Identifier = 1
			OR @Is365NonScalePlu = 1

		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
			CROSS JOIN Store s
			JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE 
			Inserted.Scale_Identifier = 1 
			AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
			AND si.Authorized = 1 
			AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
		
		if @Is365NonScalePlu = 1
			begin
				INSERT INTO 
					PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
				SELECT 
					Inserted.Item_Key, 'A', s.Store_No
				FROM 
					Inserted
					CROSS JOIN Store s
					INNER JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE 
					(s.WFM_Store = 0 AND s.Mega_Store = 1)
					AND si.Authorized = 1 
					AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
					AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
			end
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT <> 0 ROLLBACK TRAN

		DECLARE 
			@ErrorMessage NVARCHAR(MAX) = 'ItemIdentifierAdd trigger failed with error: ' + ERROR_MESSAGE(),
			@ErrorSeverity INT = ERROR_SEVERITY(),
			@ErrorState INT = ERROR_STATE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
	END CATCH
END

GO
CREATE Trigger ItemIdentifierUpdate 
ON [dbo].[ItemIdentifier]
FOR UPDATE
AS
BEGIN
	IF 0 = (SELECT COUNT(*) FROM dbo.SupportRestoreDeletedItemsItemKeys WHERE Item_Key IN (SELECT Item_Key FROM INSERTED))
	BEGIN
		DECLARE @Error_No int

		DECLARE @Identifier varchar(13) 
	
		SELECT @Identifier = Identifier FROM Inserted

		DECLARE @newItemChgTypeID tinyint
		SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'

		DECLARE @EnableUPCIRMAToIConFlow bit
		SELECT  @EnableUPCIRMAToIConFlow = acv.Value
				FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
				ON acv.EnvironmentID = ace.EnvironmentID 
				INNER JOIN AppConfigApp aca
				ON acv.ApplicationID = aca.ApplicationID 
				INNER JOIN AppConfigKey ack
				ON acv.KeyID = ack.KeyID 
				WHERE aca.Name = 'IRMA Client' AND
				ack.Name = 'EnableUPCIRMAToIConFlow' and
				SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
		DECLARE @EnablePLUIRMAIConFlow bit
		SELECT @EnablePLUIRMAIConFlow = acv.Value
				FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
				ON acv.EnvironmentID = ace.EnvironmentID 
				INNER JOIN AppConfigApp aca
				ON acv.ApplicationID = aca.ApplicationID 
				INNER JOIN AppConfigKey ack
				ON acv.KeyID = ack.KeyID 
				WHERE aca.Name = 'IRMA Client' AND
				ack.Name = 'EnablePLUIRMAIConFlow' and
				SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
    

		SELECT @Error_No = 0

		IF EXISTS (SELECT *
					FROM Inserted
					INNER JOIN
						Item ON Item.Item_Key = Inserted.Item_Key
					INNER JOIN
						Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
					INNER JOIN
						(SELECT Supplier_Store_No, SubTeam_No
						 FROM ZoneSubTeam Z (nolock)
						 INNER JOIN Store (nolock) ON Store.Store_No = Z.Supplier_Store_No
						 WHERE EXEWarehouse IS NOT NULL
						 GROUP BY Supplier_Store_No, SubTeam_No) ZS
						ON ZS.SubTeam_No = Item.SubTeam_No
					LEFT JOIN
						WarehouseItemChange Q
						ON ZS.Supplier_Store_No = Q.Store_No AND Inserted.Item_Key = Q.Item_Key AND Q.ChangeType = 'A'
					WHERE Inserted.Default_Identifier <> Deleted.Default_Identifier
						AND Item.EXEDistributed = 1
						AND Q.WarehouseItemChangeID IS NULL)
		BEGIN
			ROLLBACK TRAN
			RAISERROR('Default Identifier cannot be changed for an EXE Distributed Item', 16, 1)
			RETURN
		END

		-- Queue for Price Modeling if necessary
		INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID, Status)
		SELECT 'Product', Inserted.Item_Key, Inserted.Identifier, Item_Description, 
			   ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Item.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
			   'CHANGE', CASE WHEN dbo.fn_GetDiscontinueStatus(Inserted.Item_Key, NULL, NULL) = 1 THEN 'DISCONTINUED' ELSE 'ACTIVE' END
		FROM Inserted
		INNER JOIN
			Deleted
			ON Deleted.Identifier_ID = Inserted.Identifier_ID
		INNER JOIN
			Item
			ON Inserted.Item_Key = Item.Item_Key
		INNER JOIN 
			PMSubTeamInclude SI 
			ON SI.SubTeam_No = Item.SubTeam_No
		LEFT JOIN
			ItemCategory
			ON Item.Category_ID = ItemCategory.Category_ID
		WHERE Retail_Sale = 1
			  AND NOT EXISTS (SELECT * FROM PMExcludedItem WHERE Item_Key = Item.Item_Key)
			  AND Inserted.Default_Identifier = 1 AND Deleted.Default_Identifier = 0

		SELECT @Error_No = @@ERROR

		IF @Error_No = 0
		BEGIN
			--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS THAT ARE DELETED
			INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
			SELECT Store_No, Inserted.Item_Key, 2, 'ItemIdentifierUpdate trigger'
			FROM Inserted
			INNER JOIN
				Deleted
				ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN
				(SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
			WHERE 
				(Inserted.Default_Identifier = 1 AND Deleted.Default_Identifier = 0)
			AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Store.Store_No) = 0
        
			SELECT @Error_No = @@ERROR
		END

		IF @Error_No = 0
		BEGIN
			--UPDATE SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
			DELETE PLUMCorpChgQueue
			FROM PLUMCorpChgQueue
			INNER JOIN 
				Inserted
				ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
			INNER JOIN
				Deleted
				ON Deleted.Identifier_ID = Inserted.Identifier_ID
			WHERE Deleted.Scale_Identifier = 1 AND Deleted.Deleted_Identifier = 0
				AND (Inserted.Deleted_Identifier = 1 OR Inserted.Scale_Identifier = 0)
    
			SELECT @Error_No = @@ERROR
		END

		IF @Error_No = 0
		BEGIN
			--INSERT SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
			INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT Inserted.Item_Key, 'D', s.Store_No
			FROM 
				Inserted
			INNER JOIN
				Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN 
				Store s
			JOIN 
				StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
			WHERE Deleted.Scale_Identifier = 1 AND Deleted.Deleted_Identifier = 0
				  AND (Inserted.Deleted_Identifier = 1 OR Inserted.Scale_Identifier = 0)
				  AND s.WFM_Store = 1 AND si.Authorized = 1 AND
				  NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'D') AND
				  NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'D')

    
			SELECT @Error_No = @@ERROR
		END
    
		IF @Error_No = 0
		BEGIN
			--UPDATE SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE UNDELETED OR MARKED AS SCALE
			DELETE PLUMCorpChgQueue
			FROM PLUMCorpChgQueue
			INNER JOIN 
				Inserted ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
			INNER JOIN
				Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
			WHERE Inserted.Scale_Identifier = 1 AND Inserted.Deleted_Identifier = 0
				AND (Deleted.Deleted_Identifier = 1 OR Deleted.Scale_Identifier = 0)
    
			SELECT @Error_No = @@ERROR
		END

		IF @Error_No = 0
		BEGIN
			--INSERT SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
			INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT Inserted.Item_Key, 'A', s.Store_No
			FROM Inserted
			INNER JOIN
				Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN
				Store s
			JOIN 
				StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
			WHERE Inserted.Scale_Identifier = 1 AND Inserted.Deleted_Identifier = 0
				AND (Deleted.Deleted_Identifier = 1 OR Deleted.Scale_Identifier = 0)
				AND s.WFM_Store = 1 AND si.Authorized = 1 AND
				NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') AND
				NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')

    
			SELECT @Error_No = @@ERROR
		END

		IF @Error_No = 0
		BEGIN
			--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE ADDED (FOR NON-TYPE-2 IDENTIFIERS)
			INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
			SELECT Store_No, Inserted.Item_Key, 2, 'ItemIdentifierUpdate trigger'
			FROM Inserted
			INNER JOIN
				Deleted
				ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN
				(SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
			WHERE 
				(Inserted.Scale_Identifier = 1  AND Deleted.Scale_Identifier = 0) AND
				(SUBSTRING(Inserted.Identifier,1,1) <> '2')
		
        
			SELECT @Error_No = @@ERROR
		END

		IF @Error_No = 0
		BEGIN
			--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE DELETED (FOR NON-TYPE-2 IDENTIFIERS)
			INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
			SELECT Store_No, Inserted.Item_Key, 3, 'ItemIdentifierUpdate trigger'
			FROM Inserted
			INNER JOIN
				Deleted
				ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN
				(SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
			WHERE 
				(Inserted.Deleted_Identifier = 1  AND Deleted.Deleted_Identifier = 0) AND
				(SUBSTRING(Inserted.Identifier,1,1) <> '2')
		
        
			SELECT @Error_No = @@ERROR
		END

		-- WHEN AN ALTERNATE IDENTIFIER IS PROMOTED FROM ALTERNATE TO DEFAULT, THE ITEM WILL BE SENT TO ICON 
		-- AS A NEW ITEM
		IF (@Error_No = 0)
			BEGIN

				IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
					(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000')) OR
					(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000'))
				BEGIN
			

					insert into iConItemChangeQueue (
						Item_Key, 
						Identifier, 
						ItemChgTypeID
						)
						SELECT DISTINCT
							Item_Key = Inserted.Item_Key,
							Identifier = Inserted.Identifier,
							ItemChgTypeID = @newItemChgTypeID
						FROM Inserted INNER JOIN Deleted
						ON Inserted.Identifier_ID = Deleted.Identifier_ID
						WHERE Inserted.Default_Identifier <> Deleted.Default_Identifier
						AND Inserted.Default_Identifier = 1 

				END
				SELECT @Error_No = @@ERROR
			END

		IF @error_no = 0
		BEGIN
			--INSERT A SCALE "CHANGE" (C) IF THE ItemIdentifier.NumPluDigitsSentToScale CHANGES
			--CURRENTLY NO OTHER FIELDS IN ItemIdentifier TRIGGERS A SCALE/POS BATCH
			INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
			SELECT Inserted.Item_Key, 'C', s.Store_No
			FROM 
				Inserted
			INNER JOIN
				Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
			CROSS JOIN
				Store s
			JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
			WHERE Inserted.Remove_Identifier = 0 AND Inserted.Deleted_Identifier = 0 
				AND ISNULL(Inserted.NumPluDigitsSentToScale, '') <> ISNULL(Deleted.NumPluDigitsSentToScale, '')
				AND Inserted.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
				AND s.WFM_Store = 1 AND si.Authorized = 1
				AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')
				AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')

            
			SELECT @error_no = @@ERROR
		END

		IF @Error_No <> 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('ItemIdentifierUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
	END
END

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [ExtractRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [BizTalk]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemIdentifier] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [spice_user]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemIdentifier] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemIdentifier] TO [TibcoDataWriter]
    AS [dbo];

