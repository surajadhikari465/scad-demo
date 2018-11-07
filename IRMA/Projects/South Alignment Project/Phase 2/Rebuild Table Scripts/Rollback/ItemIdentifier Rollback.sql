/******************************************************************************
		SO [dbo].[ItemIdentifier]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[ItemIdentifier] ROLLBACK  --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[ItemIdentifier]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemIdentifier] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF__ItemIdent__Defau__47C20D6C]
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF__ItemIdent__Delet__48B631A5]
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF__ItemIdent__Add_I__49AA55DE]
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF__ItemIdent__Remov__4A9E7A17]
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF_ItemIdentifier_National_Identifier]
GO
ALTER TABLE [ItemIdentifier] DROP CONSTRAINT [DF_ItemIdentifier_Scale_Identifier]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifierUpdate]'))
DROP TRIGGER [dbo].[ItemIdentifierUpdate]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifierAdd]'))
DROP TRIGGER [dbo].[ItemIdentifierAdd]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ItemIdent__Item___46CDE933]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]'))
ALTER TABLE [dbo].[ItemIdentifier] DROP CONSTRAINT [FK__ItemIdent__Item___46CDE933]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierRemove_Identifier')
DROP INDEX [idxIdentifierRemove_Identifier] ON [dbo].[ItemIdentifier]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierItemKey')
DROP INDEX [idxIdentifierItemKey] ON [dbo].[ItemIdentifier]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierIdentifierDefault')
DROP INDEX [idxIdentifierIdentifierDefault] ON [dbo].[ItemIdentifier]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierDefaultIdentifier')
DROP INDEX [idxIdentifierDefaultIdentifier] ON [dbo].[ItemIdentifier]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierAdd_Identifier')
DROP INDEX [idxIdentifierAdd_Identifier] ON [dbo].[ItemIdentifier]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxDefaultIdentifier')
DROP INDEX [idxDefaultIdentifier] ON [dbo].[ItemIdentifier]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'PK_ItemIdentifier_Identifier_ID')
EXECUTE sp_rename N'[dbo].[PK_ItemIdentifier_Identifier_ID]', N'PK_ItemIdentifier_Identifier_ID_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier_Unaligned]') AND name = N'PK_ItemIdentifier_Identifier_ID_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_ItemIdentifier_Identifier_ID_Unaligned]', N'Identifier_ID';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemIdentifier]', N'ItemIdentifier_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[ItemIdentifier_Unaligned]', N'ItemIdentifier';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF__ItemIdent__Defau__47C20D6C] DEFAULT (0) FOR [Default_Identifier]
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF__ItemIdent__Delet__48B631A5] DEFAULT (0) FOR [Deleted_Identifier]
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF__ItemIdent__Add_I__49AA55DE] DEFAULT (0) FOR [Add_Identifier]
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF__ItemIdent__Remov__4A9E7A17] DEFAULT (0) FOR [Remove_Identifier]
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF_ItemIdentifier_National_Identifier] DEFAULT (0) FOR [National_Identifier]
GO
ALTER TABLE [ItemIdentifier] WITH NOCHECK ADD CONSTRAINT [DF_ItemIdentifier_Scale_Identifier] DEFAULT ((0)) FOR [Scale_Identifier]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[ItemIdentifier] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [BizTalk] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [ExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IConInterface] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ItemIdentifier] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IConInterface] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [iCONReportingRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMA_Teradata] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAAdminRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAAVCIRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAClientRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[ItemIdentifier] TO [IRMAExcelRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ItemIdentifier] TO [IRMAExcelRole] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[ItemIdentifier] TO [IRMAExcelRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAExcelRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ItemIdentifier] TO [IRMAExcelRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAPromoRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[ItemIdentifier] TO [IRMASLIMRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[ItemIdentifier] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMASLIMRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ItemIdentifier] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [NutriChefDataWriter] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [sobluesky] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [SODataControl] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [SOInventory] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[ItemIdentifier] TO [spice_user] AS [dbo]
GO
GRANT DELETE ON [dbo].[ItemIdentifier] TO [SQLExcel] AS [dbo]
GO
GRANT INSERT ON [dbo].[ItemIdentifier] TO [SQLExcel] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[ItemIdentifier] TO [SQLExcel] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [SQLExcel] AS [dbo]
GO
GRANT UPDATE ON [dbo].[ItemIdentifier] TO [SQLExcel] AS [dbo]
GO
GRANT SELECT ON [dbo].[ItemIdentifier] TO [TibcoDataWriter] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierItemKey')
CREATE CLUSTERED INDEX [idxIdentifierItemKey] ON [dbo].[ItemIdentifier]
(
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'_dta_IX_ItemIdentifier_Identifier_IdentifierID_ItemKey')
CREATE NONCLUSTERED INDEX [_dta_IX_ItemIdentifier_Identifier_IdentifierID_ItemKey] ON [dbo].[ItemIdentifier]
(
	[Identifier] ASC,
	[Identifier_ID] ASC,
	[Item_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierAdd_Identifier')
CREATE NONCLUSTERED INDEX [idxIdentifierAdd_Identifier] ON [dbo].[ItemIdentifier]
(
	[Add_Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierDefaultDeleted')
CREATE NONCLUSTERED INDEX [idxIdentifierDefaultDeleted] ON [dbo].[ItemIdentifier]
(
	[Default_Identifier] ASC,
	[Deleted_Identifier] ASC
)
INCLUDE ( 	[Item_Key],
	[Identifier]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierDefaultIdentifier')
CREATE NONCLUSTERED INDEX [idxIdentifierDefaultIdentifier] ON [dbo].[ItemIdentifier]
(
	[Identifier] ASC,
	[Deleted_Identifier] ASC,
	[Default_Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxIdentifierRemove_Identifier')
CREATE NONCLUSTERED INDEX [idxIdentifierRemove_Identifier] ON [dbo].[ItemIdentifier]
(
	[Remove_Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxItemIdentifier_GetPriceBatchSent')
CREATE NONCLUSTERED INDEX [idxItemIdentifier_GetPriceBatchSent] ON [dbo].[ItemIdentifier]
(
	[Item_Key] ASC,
	[Identifier] ASC
)
INCLUDE ( 	[National_Identifier],
	[Scale_Identifier]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxItemIdentifierAdjustedCost')
CREATE NONCLUSTERED INDEX [idxItemIdentifierAdjustedCost] ON [dbo].[ItemIdentifier]
(
	[Default_Identifier] ASC,
	[Item_Key] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'idxItemIdentifierAlloc')
CREATE NONCLUSTERED INDEX [idxItemIdentifierAlloc] ON [dbo].[ItemIdentifier]
(
	[Item_Key] ASC,
	[Default_Identifier] ASC,
	[Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]') AND name = N'IX_ItemIdentifierScale_Identifier')
CREATE NONCLUSTERED INDEX [IX_ItemIdentifierScale_Identifier] ON [dbo].[ItemIdentifier]
(
	[Item_Key] ASC,
	[Scale_Identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ItemIdent__Item___46CDE933]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]'))
ALTER TABLE [dbo].[ItemIdentifier]  WITH NOCHECK ADD  CONSTRAINT [FK__ItemIdent__Item___46CDE933] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ItemIdent__Item___46CDE933]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemIdentifier]'))
ALTER TABLE [dbo].[ItemIdentifier] CHECK CONSTRAINT [FK__ItemIdent__Item___46CDE933]
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifierAdd]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[ItemIdentifierAdd]
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
			Inserted.Item_Key, ''A'', s.Store_No
		FROM 
			Inserted
			CROSS JOIN Store s
			JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE 
			Inserted.Scale_Identifier = 1 
			AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
			AND si.Authorized = 1 
			AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = ''A'') 
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = ''A'')
		
		if @Is365NonScalePlu = 1
			begin
				INSERT INTO 
					PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
				SELECT 
					Inserted.Item_Key, ''A'', s.Store_No
				FROM 
					Inserted
					CROSS JOIN Store s
					INNER JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE 
					(s.WFM_Store = 0 AND s.Mega_Store = 1)
					AND si.Authorized = 1 
					AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = ''A'') 
					AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = ''A'')
			end
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT <> 0 ROLLBACK TRAN
		DECLARE 
			@ErrorMessage NVARCHAR(MAX) = ''ItemIdentifierAdd trigger failed with error: '' + ERROR_MESSAGE(),
			@ErrorSeverity INT = ERROR_SEVERITY(),
			@ErrorState INT = ERROR_STATE()
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
	END CATCH
END
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemIdentifierUpdate]'))
EXEC dbo.sp_executesql @statement = N'CREATE Trigger [dbo].[ItemIdentifierUpdate] 
ON [dbo].[ItemIdentifier]
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int
	DECLARE @Identifier varchar(13) 
	
	SELECT @Identifier = Identifier FROM Inserted
	DECLARE @newItemChgTypeID tinyint
	SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like ''new''
	DECLARE @EnableUPCIRMAToIConFlow bit
	SELECT  @EnableUPCIRMAToIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = ''IRMA Client'' AND
			ack.Name = ''EnableUPCIRMAToIConFlow'' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
	
	DECLARE @EnablePLUIRMAIConFlow bit
	SELECT @EnablePLUIRMAIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = ''IRMA Client'' AND
			ack.Name = ''EnablePLUIRMAIConFlow'' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = ''IRMA CLIENT''),1,1)
    
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
                    ON ZS.Supplier_Store_No = Q.Store_No AND Inserted.Item_Key = Q.Item_Key AND Q.ChangeType = ''A''
                WHERE Inserted.Default_Identifier <> Deleted.Default_Identifier
                    AND Item.EXEDistributed = 1
                    AND Q.WarehouseItemChangeID IS NULL)
    BEGIN
        ROLLBACK TRAN
        RAISERROR(''Default Identifier cannot be changed for an EXE Distributed Item'', 16, 1)
        RETURN
    END
    -- Queue for Price Modeling if necessary
    INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID, Status)
    SELECT ''Product'', Inserted.Item_Key, Inserted.Identifier, Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Item.SubTeam_No) + ''1''), ISNULL(Category_Name, ''NO CATEGORY''), 
           ''CHANGE'', CASE WHEN dbo.fn_GetDiscontinueStatus(Inserted.Item_Key, NULL, NULL) = 1 THEN ''DISCONTINUED'' ELSE ''ACTIVE'' END
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
        SELECT Store_No, Inserted.Item_Key, 2, ''ItemIdentifierUpdate trigger''
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
        SELECT Inserted.Item_Key, ''D'', s.Store_No
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
	          NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''D'') AND
			  NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''D'')
    
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
        SELECT Inserted.Item_Key, ''A'', s.Store_No
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
			NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = ''A'') AND
			NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = ''A'')
    
        SELECT @Error_No = @@ERROR
    END
	IF @Error_No = 0
    BEGIN
		--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE ADDED (FOR NON-TYPE-2 IDENTIFIERS)
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, ''ItemIdentifierUpdate trigger''
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        WHERE 
            (Inserted.Scale_Identifier = 1  AND Deleted.Scale_Identifier = 0) AND
			(SUBSTRING(Inserted.Identifier,1,1) <> ''2'')
		
        
        SELECT @Error_No = @@ERROR
    END
	IF @Error_No = 0
    BEGIN
		--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE DELETED (FOR NON-TYPE-2 IDENTIFIERS)
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 3, ''ItemIdentifierUpdate trigger''
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        WHERE 
            (Inserted.Deleted_Identifier = 1  AND Deleted.Deleted_Identifier = 0) AND
			(SUBSTRING(Inserted.Identifier,1,1) <> ''2'')
		
        
        SELECT @Error_No = @@ERROR
    END
	-- WHEN AN ALTERNATE IDENTIFIER IS PROMOTED FROM ALTERNATE TO DEFAULT, THE ITEM WILL BE SENT TO ICON 
	-- AS A NEW ITEM
	IF (@Error_No = 0)
		BEGIN
			IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
				(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE ''2%00000'')) OR
				(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE ''2%00000''))
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
        SELECT Inserted.Item_Key, ''C'', s.Store_No
        FROM 
			Inserted
        INNER JOIN
            Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
			Store s
		JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
        WHERE Inserted.Remove_Identifier = 0 AND Inserted.Deleted_Identifier = 0 
            AND ISNULL(Inserted.NumPluDigitsSentToScale, '''') <> ISNULL(Deleted.NumPluDigitsSentToScale, '''')
            AND Inserted.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			AND s.WFM_Store = 1 AND si.Authorized = 1
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''C'')
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = ''C'')
            
        SELECT @error_no = @@ERROR
    END
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''ItemIdentifierUpdate trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END
' 
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ItemIdentifier', N'COLUMN',N'IdentifierType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'B=Barcode;P=PLU;S=SKU;O=Other' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ItemIdentifier', @level2type=N'COLUMN',@level2name=N'IdentifierType'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[ItemIdentifier] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[ItemIdentifier] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
