-- Script to Send ALL Regional IRMA items to Icon
-- by sending this, the Interface Controller will send them and Icon will take it from there.

-- First, get all the appropriate Identifiers:
SELECT ii.item_key,
	ii.identifier
INTO #identifier
FROM itemidentifier ii
JOIN item i ON ii.item_key = i.item_key
WHERE i.Deleted_Item = 0
	AND i.Remove_Item = 0
	AND i.Retail_Sale = 1
	AND ii.Deleted_Identifier = 0

-- Out with the old:
TRUNCATE TABLE IconItemChangeQueue

-- In with the new:
INSERT INTO IconItemChangeQueue (
	Item_Key,
	Identifier,
	ItemChgTypeID,
	InsertDate
	)
SELECT 
   ii.Item_Key       AS [Item_Key],
	ii.Identifier     AS [Identifier],
	'1'               AS [ItemChgTypeID], -- 1 = New Item Change Type
	SYSDATETIME()     AS [InsertDate]
FROM #identifier ii

-- Clean up:
DROP TABLE #identifier
