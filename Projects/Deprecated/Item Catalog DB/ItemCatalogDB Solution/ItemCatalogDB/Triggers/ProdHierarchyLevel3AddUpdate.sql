 
-- ProdHierarchyLevel3
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'ProdHierarchyLevel3AddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[ProdHierarchyLevel3AddUpdate]
   END
GO

CREATE TRIGGER [dbo].[ProdHierarchyLevel3AddUpdate] ON [dbo].[ProdHierarchyLevel3] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ProdHierarchyLevel3
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ProdHierarchyLevel3.ProdHierarchyLevel3_Id = i.ProdHierarchyLevel3_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ProdHierarchyLevel3AddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
