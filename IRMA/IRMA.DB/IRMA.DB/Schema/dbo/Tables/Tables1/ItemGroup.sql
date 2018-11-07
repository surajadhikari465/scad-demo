CREATE TABLE [dbo].[ItemGroup] (
    [Group_ID]     INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]    VARCHAR (50)  NULL,
    [GroupLogic]   BIT           NULL,
    [createdate]   SMALLDATETIME NULL,
    [modifieddate] SMALLDATETIME NULL,
    [User_ID]      INT           NULL,
    [IsEdited]     INT           NULL,
    CONSTRAINT [PK_ItemGroup] PRIMARY KEY CLUSTERED ([Group_ID] ASC)
);


GO
/**
	Tracks deletions in ItemGroup
**/
CREATE TRIGGER [ItemGroupDelete] ON [dbo].[ItemGroup] 
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Deleted records, including date of deletion
	**/
    INSERT INTO ItemGroupHistory (
			 [ItemGroupHistory].[Group_ID]
			,[ItemGroupHistory].[GroupName]
			,[ItemGroupHistory].[GroupLogic]
			,[ItemGroupHistory].[createdate]
			,[ItemGroupHistory].[modifieddate]
			,[ItemGroupHistory].[User_ID]
			,[ItemGroupHistory].[User_Name]
			,[ItemGroupHistory].[Host_Name]
			,[ItemGroupHistory].[Effective_Date]
			,[ItemGroupHistory].[Deleted])
    SELECT	 [Deleted].[Group_ID]
			,[Deleted].[GroupName]
			,[Deleted].[GroupLogic]
			,[Deleted].[createdate]
			,[Deleted].[modifieddate]
			,[Deleted].[User_ID]
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
        RAISERROR ('ItemGroupDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
/**
	Track Creates and Updates to ItemGroup
**/
CREATE TRIGGER [ItemGroupAddUpdate] ON [dbo].[ItemGroup] 
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Inserted records 
	**/
    INSERT INTO ItemGroupHistory (
			 [ItemGroupHistory].[Group_ID]
			,[ItemGroupHistory].[GroupName]
			,[ItemGroupHistory].[GroupLogic]
			,[ItemGroupHistory].[createdate]
			,[ItemGroupHistory].[modifieddate]
			,[ItemGroupHistory].[User_ID]
			,[ItemGroupHistory].[User_Name]
			,[ItemGroupHistory].[Host_Name]
			,[ItemGroupHistory].[Effective_Date])
    SELECT	 [ItemGroup].[Group_ID]
			,[ItemGroup].[GroupName]
			,[ItemGroup].[GroupLogic]
			,[ItemGroup].[createdate]
			,[ItemGroup].[modifieddate]
			,[ItemGroup].[User_ID]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
    FROM ItemGroup
    INNER JOIN
        Inserted
        ON Inserted.Group_ID = ItemGroup.Group_ID 

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemGroupAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroup] TO [IRMAReportsRole]
    AS [dbo];

