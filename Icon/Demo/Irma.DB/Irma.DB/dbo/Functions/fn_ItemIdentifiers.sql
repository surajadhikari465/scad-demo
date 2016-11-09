CREATE FUNCTION [dbo].[fn_ItemIdentifiers] ()
RETURNS @TblList TABLE 
	(Item_Key          INT,
	Identifier         VARCHAR(13),
	Default_Identifier TINYINT,
	IdentifierType     CHAR(1))
AS
BEGIN
    DECLARE @FlagValue INT

    SET @FlagValue = (SELECT FlagValue 
                      FROM   InstanceDataFlags
                      WHERE  FlagKey = 'IncludeAllItemIdentifiersInShelfTagPush')

    IF @FlagValue = 0
        BEGIN
            INSERT INTO @TblList
            SELECT 
	              Item_Key,
	              Identifier,
	              Default_Identifier,
	              IdentifierType
            FROM  ItemIdentifier II
            WHERE II.Default_Identifier = 1 AND II.IdentifierType <> 'S'
        END
    ELSE
        BEGIN
            INSERT INTO @TblList
            SELECT 
	              Item_Key,
	              Identifier,
	              Default_Identifier,
	              IdentifierType
            FROM  ItemIdentifier II
            WHERE ((II.Default_Identifier = 0) OR
                   (II.Default_Identifier = 1  AND II.IdentifierType <> 'S'))
        END
    
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ItemIdentifiers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ItemIdentifiers] TO [IRSUser]
    AS [dbo];

