 
-- ProdHierarchyLevel4
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'ProdHierarchyLevel4AddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[ProdHierarchyLevel4AddUpdate]
   END
GO

CREATE TRIGGER [dbo].[ProdHierarchyLevel4AddUpdate] ON [dbo].[ProdHierarchyLevel4] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ProdHierarchyLevel4
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ProdHierarchyLevel4.ProdHierarchyLevel4_id = i.ProdHierarchyLevel4_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ProdHierarchyLevel4AddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO