/*
Index Help:
Missing Index Details - sqlshared2-dev\sqlshared2012d.iCon (WFM\Tom.Lux (58))
The Query Processor estimates that implementing the following index could improve the query cost by 75.1693%.

USE [iCon]
GO
CREATE NONCLUSTERED INDEX [???]
ON [dbo].[ItemTrait] ([itemID])
INCLUDE ([traitID],[traitValue])
GO
*/


CREATE VIEW [app].[vItemBase]
	AS
SELECT
	[ItemID]				= i.itemID
	,[ScanCode]				= scn.scancode
	,[Product Description]	= lng.traitvalue
	,[POS Description]		= sht.traitvalue
	,[Package Unit]			= pu.traitvalue
	,[Food Stamp Eligible]	= 'not yet defined'
	,[Tare]					= 'not yet defined'
	,[Brand]				= hc.hierarchyclassname
FROM
	item i
JOIN scancode scn
	ON i.itemid = scn.itemid
JOIN scancodetype tscn
	ON scn.scancodetypeid = tscn.scancodetypeid
	AND tscn.scancodetypedesc = 'UPC'
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
JOIN itemhierarchyclass ihc
	ON i.itemid = ihc.itemid
JOIN hierarchyclass hc
	ON ihc.hierarchyclassid = hc.hierarchyclassid
JOIN hierarchy h
	ON hc.hierarchyid = h.hierarchyid
	AND h.hierarchyname = 'Brand'