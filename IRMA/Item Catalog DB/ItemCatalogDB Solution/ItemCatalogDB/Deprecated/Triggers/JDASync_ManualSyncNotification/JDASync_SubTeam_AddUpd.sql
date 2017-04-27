

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_SubTeam_AddUpd')
	BEGIN
		PRINT 'Dropping Trigger JDASync_SubTeam_AddUpd'
		DROP  Trigger JDASync_SubTeam_AddUpd
	END
GO


PRINT 'Creating Trigger JDASync_SubTeam_AddUpd'
GO
CREATE Trigger dbo.JDASync_SubTeam_AddUpd
ON SubTeam
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN

		DECLARE @BodyText varchar(255)

		SELECT @BodyText = 'Added or Updated IRMA SubTeam: ' + CAST(Inserted.SubTeam_No AS varchar(38))
		FROM Inserted

		SET @BodyText = @BodyText + CHAR(10) + 'Depending on the change, you may need to update the "JDA_HierarchyMapping" table in IRMA and the "INVDPT" table in JDA.'

		EXEC dbo.JDASync_Notify
			@EventKey = 'SYNC_MAP_TABLE_UPDATE',
			@AdditionalBodyText = @BodyText

	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_SubTeam_AddUpd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
