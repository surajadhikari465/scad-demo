if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemGroupAddUpdate]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[ItemGroupAddUpdate]
GO

-- TRIGGERS ItemGroup
PRINT N'CREATE TRIGGER [ItemGroupAddUpdate]'
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


