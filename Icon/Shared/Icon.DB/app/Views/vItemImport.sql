CREATE VIEW [app].[vItemImport]
	AS
SELECT
	[ItemID]				= i.itemID
	,[ScanCode]				= scn.scanCode
	,[Product Description]	= lng.traitvalue
	,[POS Description]		= sht.traitvalue
	,[Package Unit]			= pu.traitvalue
	,[Food Stamp Eligible]	= '?'
	,[Tare]					= 'NAN'
FROM
	item i
JOIN scancode scn
	ON i.itemid = scn.itemid
JOIN itemtrait lng
	ON i.itemid = lng.itemid
JOIN trait tlng
	ON lng.traitID = tlng.traitID
	AND tlng.traitdesc = 'Product Description'
JOIN itemtrait sht
	ON i.itemid = sht.itemid
JOIN trait tsht
	ON sht.traitID = tsht.traitID
	AND tsht.traitdesc = 'POS Description'
JOIN itemtrait pu
	ON i.itemid = pu.itemid
JOIN trait tpu
	ON pu.traitID = tpu.traitID
	AND tpu.traitdesc = 'Package Unit'