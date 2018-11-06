 
-- ItemBrand
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'ItemBrandAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[ItemBrandAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[ItemBrandAddUpdate] ON [dbo].[ItemBrand] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemBrand 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ItemBrand.Brand_Id = i.Brand_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemBrandAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO