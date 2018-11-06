 
-- NatItemFamily
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'NatItemFamilyAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[NatItemFamilyAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[NatItemFamilyAddUpdate] ON [dbo].[NatItemFamily] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update NatItemFamily
		Set LastUpdateTimestamp = GetDate()
	from Inserted I
	where NatItemFamily.NatFamilyId = i.NatFamilyId

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('NatItemFamilyAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
