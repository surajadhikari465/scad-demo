CREATE TABLE [dbo].[ItemGroupMembers] (
    [Group_ID]       INT           NULL,
    [Item_Key]       INT           NULL,
    [modifieddate]   SMALLDATETIME NULL,
    [User_ID]        INT           NULL,
    [OfferChgTypeID] TINYINT       CONSTRAINT [DF_ItemGroupMembers_OfferChgTypeID] DEFAULT ((1)) NULL,
    [Identifier]     VARCHAR (13)  NULL,
    [ItemMemberId]   INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ItemGroupMemberId] PRIMARY KEY CLUSTERED ([ItemMemberId] ASC),
    CONSTRAINT [FK_ItemGroupMembers_ItemGroup] FOREIGN KEY ([Group_ID]) REFERENCES [dbo].[ItemGroup] ([Group_ID]),
    CONSTRAINT [FK_ItemGroupMembers_OfferChgTypeID] FOREIGN KEY ([OfferChgTypeID]) REFERENCES [dbo].[OfferChgType] ([OfferChgTypeID])
);


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
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembers] TO [IRMAReportsRole]
    AS [dbo];

