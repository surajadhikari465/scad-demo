--------------------------------------------------
-- The Fuax ItemIdentifier Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_ItemIdentifierView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS item_key ,
            identifiertype AS identifiertype ,
            identifier AS identifier ,
            CAST(NULL AS TINYINT) AS Deleted_identifier ,
            CAST(1 AS TINYINT) AS Default_Identifier ,
            CAST(NULL AS TINYINT) AS national_identifier ,
            CAST(NULL AS INT) AS numpludigitssenttoscale ,
            CAST(CASE WHEN ItemType_ID = 2 THEN 1
                      ELSE 0
                 END AS BIT) AS scale_identifier
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemIdentifierView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemIdentifierView] TO [IRMAReportsRole]
    AS [dbo];

