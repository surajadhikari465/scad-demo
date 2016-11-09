
CREATE PROCEDURE dbo.Scale_InsertUpdateExtraText
	@ID int,
	@Description varchar(50),
	@Scale_LabelType_ID int,
	@ExtraText varchar(4200),
	@NEW_ID int OUTPUT
AS 
BEGIN 
    DECLARE @ItemKey int, @Error_No int, @StoreJurisdictionID int
	DECLARE @IdentifiersTable dbo.IdentifiersType

	SET @ExtraText = REPLACE(@ExtraText, CHAR(9),  '')
    SET @ExtraText = REPLACE(@ExtraText, CHAR(10), '')
    SET @ExtraText = REPLACE(@ExtraText, CHAR(13), '')

	SELECT TOP 1 @StoreJurisdictionID = StoreJurisdictionID FROM StoreJurisdiction WHERE StoreJurisdictionID > 1

	IF @ID > 0 
		BEGIN
			BEGIN
				UPDATE 
					Scale_ExtraText 
				SET 
					Description = @Description,
					Scale_LabelType_ID = @Scale_LabelType_ID,
					ExtraText = @ExtraText
				WHERE 
					Scale_ExtraText_ID = @ID
			END
	  		
			SELECT @Error_No = @@ERROR

			IF @Error_No = 0
				BEGIN
					INSERT INTO @IdentifiersTable
					SELECT ii.Identifier
					FROM 
						dbo.Item i   
					  JOIN ItemIdentifier        ii  on i.Item_Key = ii.Item_key       
				 LEFT JOIN dbo.ItemScale	     isc on i.Item_Key = isc.Item_Key
					  JOIN Scale_ExtraText       et  on et.Scale_ExtraText_ID = isc.Scale_ExtraText_ID
				WHERE et.Scale_ExtraText_ID = @ID
				  AND i.Deleted_Item = 0
				  AND i.Remove_Item = 0
				  AND ii.Deleted_Identifier = 0
				  AND ii.Remove_Identifier = 0
					  
					IF EXISTS (SELECT * FROM @IdentifiersTable)
						EXEC mammoth.IconGenerateMammothEvents @IdentifiersTable, NULL
				END

			SELECT @Error_No = @@ERROR

			IF @Error_No = 0
				DELETE @IdentifiersTable
			
			SELECT @Error_No = @@ERROR

			IF @Error_No = 0
				BEGIN
					INSERT INTO @IdentifiersTable
					SELECT ii.Identifier
					FROM 
						dbo.Item i   
					  JOIN ItemIdentifier        ii  on i.Item_Key = ii.Item_key         
				 LEFT JOIN dbo.ItemScaleOverride iso on i.Item_Key = iso.Item_Key
					  JOIN Scale_ExtraText       et  on et.Scale_ExtraText_ID = iso.Scale_ExtraText_ID 
					WHERE 
						et.Scale_ExtraText_ID = @ID
				  AND i.Deleted_Item = 0
				  AND i.Remove_Item = 0
				  AND ii.Deleted_Identifier = 0
				  AND ii.Remove_Identifier = 0

					IF EXISTS (SELECT * FROM @IdentifiersTable)
						EXEC mammoth.IconGenerateMammothEvents @IdentifiersTable, @StoreJurisdictionID
				END
		END
	ELSE
		BEGIN
			INSERT INTO Scale_ExtraText
				(Description, Scale_LabelType_ID, ExtraText)
			VALUES 
				(@Description, @Scale_LabelType_ID, @ExtraText)
				
			SELECT @NEW_ID = SCOPE_IDENTITY()
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateExtraText] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateExtraText] TO [IRMASLIMRole]
    AS [dbo];

