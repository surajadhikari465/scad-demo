CREATE TABLE [dbo].[Scale_StorageData](
	[Scale_StorageData_ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[StorageData] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_ScaleStorage] PRIMARY KEY CLUSTERED 
(
	[Scale_StorageData_ID] ASC
)
);
GO
ALTER TABLE [dbo].[Scale_StorageData] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO

CREATE Trigger [dbo].[Scale_StorageData_Insert]
ON [dbo].[Scale_Storagedata]
FOR INSERT
AS

BEGIN
    DECLARE @Error_No int 
    SELECT @Error_No = 0

	-- Queue for PlumCorpChgQueue
	BEGIN
		INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        isc.Item_Key, 'A', s.Store_No
            FROM 
		        Inserted
			INNER JOIN ItemScale isc ON isc.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
            CROSS JOIN 
				Store s
            JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			WHERE 
				(s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
				AND NOT EXISTS (SELECT icfs.Item_Key from ItemCustomerFacingScale icfs (NOLOCK) WHERE icfs.Item_Key = si.Item_Key)
			UNION
			-- for 365 stores --keeping it for future
			SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
			INNER JOIN ItemScale isc ON isc.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
            CROSS JOIN 
				Store s
            INNER JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			INNER JOIN
				ItemCustomerFacingScale icfs on icfs.Item_Key = si.Item_Key and icfs.SendToScale = 1
			WHERE
				s.Mega_Store = 1
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
			SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_Storagedata_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

CREATE Trigger [dbo].[Scale_StorageData_Update]
ON [dbo].[Scale_Storagedata]
FOR UPDATE
AS

BEGIN
    DECLARE @Error_No int 
    SELECT @Error_No = 0

	-- Queue for PlumCorpChgQueue
	BEGIN
		INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
			INNER JOIN  Deleted ON Deleted.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
			INNER JOIN ItemScale isc ON isc.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
            CROSS JOIN 
				Store s
            JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			WHERE 
				(s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
				AND NOT EXISTS (SELECT icfs.Item_Key from ItemCustomerFacingScale icfs (NOLOCK) WHERE icfs.Item_Key = si.Item_Key)
			UNION
			-- for 365 stores --keeping it for future
			SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
			INNER JOIN ItemScale isc ON isc.Scale_StorageData_ID = Inserted.Scale_StorageData_ID
            CROSS JOIN 
				Store s
            INNER JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			INNER JOIN
				ItemCustomerFacingScale icfs on icfs.Item_Key = si.Item_Key and icfs.SendToScale = 1
			WHERE
				s.Mega_Store = 1
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
			SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_Storagedata_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
