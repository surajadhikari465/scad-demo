if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemGroupMembersDelete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[ItemGroupMembersDelete]
GO

PRINT N'CREATE TRIGGER [ItemGroupMembersDelete]'
GO
 
/**
	Tracks deletions in ItemGroupMembers
**/
CREATE TRIGGER [ItemGroupMembersDelete] ON [dbo].[ItemGroupMembers] 
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Deleted records, including date of deletion
	**/
    INSERT INTO ItemGroupMembersHistory (
			[ItemGroupMembersHistory].[Group_ID]
			,[ItemGroupMembersHistory].[Item_Key]
			,[ItemGroupMembersHistory].[modifieddate]
			,[ItemGroupMembersHistory].[User_ID]
			,[ItemGroupMembersHistory].[OfferChgTypeID]
			,[ItemGroupMembersHistory].[User_Name]
			,[ItemGroupMembersHistory].[Host_Name]
			,[ItemGroupMembersHistory].[Effective_Date]
			,[ItemGroupMembersHistory].[Deleted])
    SELECT	 [Deleted].[Group_ID]
			,[Deleted].[Item_Key]
			,[Deleted].[modifieddate]
			,[Deleted].[User_ID]
			,[Deleted].[OfferChgTypeID]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
 			,1		-- DELETED
    FROM [Deleted]
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemGroupMembersDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO


