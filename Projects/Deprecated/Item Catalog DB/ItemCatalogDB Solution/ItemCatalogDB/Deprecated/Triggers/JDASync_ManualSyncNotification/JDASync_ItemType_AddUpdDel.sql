

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemType_AddUpdDel')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemType_AddUpdDel'
		DROP  Trigger JDASync_ItemType_AddUpdDel
	END
GO


PRINT 'Creating Trigger JDASync_ItemType_AddUpdDel'
GO
CREATE Trigger dbo.JDASync_ItemType_AddUpdDel
ON ItemType
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN

		DECLARE @BodyText varchar(255),
			@ForAddUpdBodyText varchar(255), @ForDeleteBodyText varchar(255)

		SELECT @ForAddUpdBodyText = 'Added or Updated IRMA ItemType: ' + CAST(Inserted.ItemType_ID AS varchar(38))
		FROM Inserted

		SELECT @ForDeleteBodyText = 'Deleted IRMA ItemType: ' + CAST(Deleted.ItemType_ID AS varchar(38))
		FROM Deleted

		IF @ForDeleteBodyText IS NOT NULL
			SET @BodyText = @ForDeleteBodyText
		ELSE
			SET @BodyText = @ForAddUpdBodyText

		SET @BodyText = @BodyText + CHAR(10) + 'Depending on the change, you may need to update the "TBLFLD" table in JDA.'

		EXEC dbo.JDASync_Notify
			@EventKey = 'SYNC_MAP_TABLE_UPDATE',
			@AdditionalBodyText = @BodyText

	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_ItemType_AddUpdDel trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
