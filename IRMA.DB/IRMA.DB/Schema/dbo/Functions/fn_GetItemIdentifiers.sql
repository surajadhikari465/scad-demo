
CREATE FUNCTION [dbo].[fn_GetItemIdentifiers] ()
RETURNS @ItemIdentifier TABLE
	(
	Identifier_ID			INT,
	Item_Key				INT,
	Identifier				VARCHAR(13),
	Default_Identifier		TINYINT,
	Deleted_Identifier		TINYINT,
	Add_Identifier			TINYINT,
	Remove_Identifier		TINYINT,
	National_Identifier		TINYINT,
	CheckDigit				CHAR(1),
	IdentifierType			CHAR(1),
	NumPluDigitsSentToScale	INT,
	Scale_Identifier		BIT
	)
AS 
BEGIN

/*
History
Denis Ng & Tom Lux, 2015.05.29, TFS 9554 & 16180: Replaced all "II.*" references selecting from ItemIdentifier to explicitly select each column to fix schema difference in South region.
*/

	DECLARE @Status SMALLINT
	
	SET @Status = dbo.fn_ReceiveUPCPLUUpdateFromIcon()
	
	IF @Status = 0 -- Validated UPC & PLU flags have not been turned on for the region.
	BEGIN
		INSERT INTO @ItemIdentifier
		SELECT
			II.Identifier_ID,
			II.Item_Key,
			II.Identifier,
			II.Default_Identifier,
			II.Deleted_Identifier,
			II.Add_Identifier,
			II.Remove_Identifier,
			II.National_Identifier,
			II.CheckDigit,
			II.IdentifierType,
			II.NumPluDigitsSentToScale,
			II.Scale_Identifier
			FROM ItemIdentifier II (NOLOCK)
			
	END
ELSE
	IF @Status = 1 -- Only validated UPCs are passing from Icon to IRMA
		BEGIN
			INSERT INTO @ItemIdentifier		
			SELECT
			II.Identifier_ID,
			II.Item_Key,
			II.Identifier,
			II.Default_Identifier,
			II.Deleted_Identifier,
			II.Add_Identifier,
			II.Remove_Identifier,
			II.National_Identifier,
			II.CheckDigit,
			II.IdentifierType,
			II.NumPluDigitsSentToScale,
			II.Scale_Identifier
			FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
			ON II.Identifier = VSC.ScanCode
			UNION
			SELECT
			II.Identifier_ID,
			II.Item_Key,
			II.Identifier,
			II.Default_Identifier,
			II.Deleted_Identifier,
			II.Add_Identifier,
			II.Remove_Identifier,
			II.National_Identifier,
			II.CheckDigit,
			II.IdentifierType,
			II.NumPluDigitsSentToScale,
			II.Scale_Identifier
			FROM ItemIdentifier II (NOLOCK)
			WHERE (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
			UNION
			SELECT
			II.Identifier_ID,
			II.Item_Key,
			II.Identifier,
			II.Default_Identifier,
			II.Deleted_Identifier,
			II.Add_Identifier,
			II.Remove_Identifier,
			II.National_Identifier,
			II.CheckDigit,
			II.IdentifierType,
			II.NumPluDigitsSentToScale,
			II.Scale_Identifier
			FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
			ON I.Item_Key = II.Item_Key
			WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1

				
		END
	ELSE
		IF @Status = 2 -- Only validated PLUs are passing from Icon to IRMA
				BEGIN
					INSERT INTO @ItemIdentifier			
					SELECT
					II.Identifier_ID,
					II.Item_Key,
					II.Identifier,
					II.Default_Identifier,
					II.Deleted_Identifier,
					II.Add_Identifier,
					II.Remove_Identifier,
					II.National_Identifier,
					II.CheckDigit,
					II.IdentifierType,
					II.NumPluDigitsSentToScale,
					II.Scale_Identifier
					FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
					ON II.Identifier = VSC.ScanCode
					WHERE (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
					UNION
					SELECT
					II.Identifier_ID,
					II.Item_Key,
					II.Identifier,
					II.Default_Identifier,
					II.Deleted_Identifier,
					II.Add_Identifier,
					II.Remove_Identifier,
					II.National_Identifier,
					II.CheckDigit,
					II.IdentifierType,
					II.NumPluDigitsSentToScale,
					II.Scale_Identifier
					FROM ItemIdentifier II (NOLOCK)
					WHERE NOT (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')
					UNION
					SELECT
					II.Identifier_ID,
					II.Item_Key,
					II.Identifier,
					II.Default_Identifier,
					II.Deleted_Identifier,
					II.Add_Identifier,
					II.Remove_Identifier,
					II.National_Identifier,
					II.CheckDigit,
					II.IdentifierType,
					II.NumPluDigitsSentToScale,
					II.Scale_Identifier
					FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
					ON I.Item_Key = II.Item_Key
					WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
				
				END
			ELSE 
				IF @Status = 3 -- Both Validated UPC & PLU are passing from Icon to IRMA
					BEGIN				
						INSERT INTO @ItemIdentifier				
						SELECT
						II.Identifier_ID,
						II.Item_Key,
						II.Identifier,
						II.Default_Identifier,
						II.Deleted_Identifier,
						II.Add_Identifier,
						II.Remove_Identifier,
						II.National_Identifier,
						II.CheckDigit,
						II.IdentifierType,
						II.NumPluDigitsSentToScale,
						II.Scale_Identifier
						FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK)
						ON II.Identifier = VSC.ScanCode 
						UNION
						SELECT
						II.Identifier_ID,
						II.Item_Key,
						II.Identifier,
						II.Default_Identifier,
						II.Deleted_Identifier,
						II.Add_Identifier,
						II.Remove_Identifier,
						II.National_Identifier,
						II.CheckDigit,
						II.IdentifierType,
						II.NumPluDigitsSentToScale,
						II.Scale_Identifier
						FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK)
						ON I.Item_Key = II.Item_Key
						WHERE I.Remove_Item = 1 OR II.Remove_Identifier = 1
					END
		
		RETURN
		
END

GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetItemIdentifiers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetItemIdentifiers] TO [IRSUser]
    AS [dbo];

