if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemGroupDelete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[ItemGroupDelete]
GO

PRINT N'CREATE TRIGGER [ItemGroupDelete]'
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

