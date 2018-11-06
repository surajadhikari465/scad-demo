CREATE PROCEDURE [dbo].[DeleteItemIdentifier]
	@Identifier_ID int
AS 
BEGIN
  BEGIN TRAN
    set nocount on;
    declare @Error_No int = 0;
    declare @IdentifiersType dbo.IdentifiersType;

    insert into @IdentifiersType(Identifier)
      select Identifier from ItemIdentifier where Identifier_ID = @Identifier_ID;

    exec mammoth.GenerateEvents @IdentifiersType, 'ItemDeauthorization';
    set @Error_No = @@ERROR;

    if(@Error_No = 0)
    begin
	    if(exists(select 1 from ItemIdentifier where Identifier_ID = @Identifier_ID and Add_Identifier = 1)) --Delete if it's a new Identifier
		    delete from ItemIdentifier
        where Identifier_ID = @Identifier_ID;
      else                                                                                                 --Set Remove_Identifier flag 
        UPDATE ItemIdentifier
        SET Remove_Identifier = 1
        WHERE Identifier_ID = @Identifier_ID;

      set @Error_No = @@ERROR;
    end

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint = (SELECT ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16));
        RAISERROR ('DeleteItemIdentifier failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMAClientRole] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMASchedJobsRole] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMAReportsRole] AS [dbo];