SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateItemIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateItemIdentifier]
GO


CREATE PROCEDURE [dbo].[UpdateItemIdentifier] 
@Item_Key int,
@Identifier_ID int,
@DefaultID bit,
@NatID bit,
@NumPluDigitsSentToScale int,
@Scale_Identifier bit
AS


	DECLARE @NumPluDigitsSentToScaleUpdated bit = 0
	DECLARE @Scale_IdentifierUpdated bit
	DECLARE @Identifier varchar(50)

	SELECT	@NumPluDigitsSentToScaleUpdated = CASE WHEN @NumPluDigitsSentToScale IS NOT NULL AND NumPluDigitsSentToScale IS NOT NULL AND NumPluDigitsSentToScale <> @NumPluDigitsSentToScaleUpdated
														 THEN 1
													WHEN (@NumPluDigitsSentToScale IS NULL AND NumPluDigitsSentToScale IS NOT NULL) OR (@NumPluDigitsSentToScale IS NOT NULL AND NumPluDigitsSentToScale IS NULL)
															THEN 1
													ELSE 0
												END,
			 @Scale_IdentifierUpdated = CASE WHEN @Scale_Identifier IS NOT NULL AND  @Scale_Identifier IS NOT NULL AND Scale_Identifier <> @Scale_Identifier THEN 1
											WHEN (@Scale_Identifier IS NULL AND  @Scale_Identifier IS NOT NULL) OR (@Scale_Identifier IS NOT NULL AND  @Scale_Identifier IS NULL) THEN 1
											ELSE 0
										END,
			 @Identifier = Identifier
	FROM ItemIdentifier
	WHERE
		Item_Key = @Item_Key and Identifier_ID = @Identifier_ID

    UPDATE ItemIdentifier
    SET Default_Identifier = (case when @DefaultID = 1
                                    then CASE WHEN Identifier_ID = @Identifier_ID 
                                               THEN 1 
                                               ELSE 0 
                                             END
                                    else Default_Identifier
                                   end 
                             ),
      National_Identifier = (CASE WHEN Identifier_ID = @Identifier_ID THEN @NatID
                             ELSE National_Identifier
                             END),
      NumPluDigitsSentToScale = (CASE WHEN @NumPluDigitsSentToScale IS NOT NULL AND Identifier_ID = @Identifier_ID THEN @NumPluDigitsSentToScale
								 ELSE NumPluDigitsSentToScale
								 END),
	  Scale_Identifier = (CASE WHEN Identifier_ID = @Identifier_ID THEN @Scale_Identifier
                             ELSE Scale_Identifier
                             END),
	  Add_Identifier = (CASE WHEN Identifier_ID = @Identifier_ID AND @Scale_Identifier = 1 AND Add_Identifier = 0 THEN 1
							WHEN Identifier_ID = @Identifier_ID AND @Scale_Identifier = 0 AND Add_Identifier = 1 THEN 0
							ELSE Add_Identifier
							END)
    WHERE Item_Key = @Item_Key


	--
	IF @NumPluDigitsSentToScaleUpdated  = 1 or @Scale_IdentifierUpdated = 1
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', @Identifier, NULL
	END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



