CREATE PROCEDURE [dbo].[DeleteItemIdentifier]
	@Identifier_ID int
AS 

BEGIN
  DECLARE @Error_No int = 0,
          @Item_Key int,
          @Identifier varchar(13);
	
	SELECT 
		@Item_Key = Item_Key,  
		@Identifier = Identifier 
	FROM ItemIdentifier 
	WHERE Identifier_ID = @Identifier_ID

  BEGIN TRAN
	  --Send Mammoth Events first because if the Add_Identifier = 1 then the Identifier will be 
	  --deleted and the no event will be generated
    --18-10-16: Seems like the the query below has been obsoleted but let's keep it here for awhile just in case
		--EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemDelete', @Identifier
			
	    DECLARE @IdentifiersType dbo.IdentifiersType
	    INSERT INTO  @IdentifiersType(Identifier) values(@Identifier)
	
	    EXEC [mammoth].[GenerateEvents] @IdentifiersType, 'ItemDeauthorization'

		  SELECT @Error_No = @@ERROR
    
	IF @Error_No = 0
	BEGIN
		DELETE 
		FROM ItemIdentifier
		WHERE Identifier_ID = @Identifier_ID AND Add_Identifier = 1

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No = 0
    BEGIN
        UPDATE ItemIdentifier
        SET Remove_Identifier = 1
        WHERE Identifier_ID = @Identifier_ID

        SELECT @Error_No = @@ERROR
    END

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
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemIdentifier] TO [IRMAReportsRole]
    AS [dbo];