 

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_ItemUnitMapping_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_ItemUnitMapping_Update'
		DROP  Trigger JDASync_ItemUnitMapping_Update
	END
GO


PRINT 'Creating Trigger JDASync_ItemUnitMapping_Update'
GO
CREATE Trigger dbo.JDASync_ItemUnitMapping_Update 
ON JDA_ItemUnitMapping
FOR INSERT,UPDATE
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
	
		-- update any null mapped values in unsynced data
		EXEC dbo.JDASync_UpdateMappedValues
	
		SELECT @Error_No = @@ERROR

		IF @Error_No <> 0
		BEGIN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('JDASync_ItemUnitMapping_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
	    
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
