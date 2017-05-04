SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemIdentifier]
GO


CREATE PROCEDURE [dbo].[DeleteItemIdentifier]
@Identifier_ID int
AS 

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
	DECLARE @Item_Key int
	DECLARE @Identifier varchar(50)
	SELECT @Item_Key = Item_Key,  @Identifier = Identifier from ItemIdentifier where Identifier_ID = @Identifier_ID

    BEGIN TRAN
    
    DELETE 
    FROM ItemIdentifier
    WHERE Identifier_ID = @Identifier_ID AND Add_Identifier = 1

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        UPDATE ItemIdentifier
        SET Remove_Identifier = 1
        WHERE Identifier_ID = @Identifier_ID

        SELECT @Error_No = @@ERROR
    END

	IF @Error_No = 0
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemDelete', @Identifier
	END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('DeleteItemIdentifier failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

