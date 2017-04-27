if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemGroupMembersAddUpdate]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[ItemGroupMembersAddUpdate]
GO

-- TRIGGERS ItemGroupMembers
PRINT N'CREATE TRIGGER [ItemGroupMembersAddUpdate]'
GO
 
/**
	Track Creates and Updates to ItemGroupMembers
**/
CREATE TRIGGER [ItemGroupMembersAddUpdate] ON [dbo].[ItemGroupMembers]
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Inserted records 
	**/
    INSERT INTO ItemGroupMembersHistory (
			 [ItemGroupMembersHistory].[Group_ID]
			,[ItemGroupMembersHistory].[Item_Key]
			,[ItemGroupMembersHistory].[modifieddate]
			,[ItemGroupMembersHistory].[User_ID]
			,[ItemGroupMembersHistory].[OfferChgTypeID]
			,[ItemGroupMembersHistory].[User_Name]
			,[ItemGroupMembersHistory].[Host_Name]
			,[ItemGroupMembersHistory].[Effective_Date])
    SELECT	 [ItemGroupMembers].[Group_ID]
			,[ItemGroupMembers].[Item_Key]
			,[ItemGroupMembers].[modifieddate]
			,[ItemGroupMembers].[User_ID]
			,[ItemGroupMembers].[OfferChgTypeID]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
    FROM ItemGroupMembers
    INNER JOIN
        Inserted
        ON Inserted.Group_ID = ItemGroupMembers.Group_ID AND Inserted.Item_Key = ItemGroupMembers.Item_Key 

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemGroupMembersAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO


