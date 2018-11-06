 

-- NatItemCat
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'NatItemCatAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[NatItemCatAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[NatItemCatAddUpdate] ON [dbo].[NatItemCat] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update NatItemCat 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where NatItemCat.NatCatId = i.NatCatId

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('NatItemCatAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO