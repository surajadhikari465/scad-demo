CREATE PROCEDURE [app].[PDX_ItemHierarchyFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

;WITH ITEMS
AS (
select sc.itemID
from ScanCode sc
join app.IRMAItemSubscription iis on sc.ScanCode = iis.identifier
join ItemTrait it on sc.itemID = it.itemID 
join Trait t on it.traitID = t.traitID 
where iis.regioncode = 'RM'
  and t.traitDesc = 'Validation Date'
)
,
DESCR
AS (
       SELECT pdit.itemID, pdit.traitValue
         FROM itemTrait pdit
         JOIN Trait pdt on pdit.traitID = pdt.traitID
		 --JOIN ITEMS i on i.itemID = pdit.itemID
       WHERE pdt.traitDesc = 'Product Description'
       )
,
ITEM_UOM
AS (
       SELECT uomit.itemID, uomit.traitValue
         FROM itemTrait uomit
         JOIN Trait uomt on uomt.traitID = uomit.traitID
		 --JOIN ITEMS i on i.itemID = uomit.itemID
       WHERE uomt.traitDesc = 'Retail UOM'
       )
,
BRAND
AS (
       SELECT brandihc.itemID, brandhc.hierarchyClassName
         FROM ItemHierarchyClass brandihc 
		 --JOIN ITEMS i on i.itemID = brandihc.itemID
 LEFT JOIN HierarchyClass brandhc on brandhc.HierarchyClassId = brandihc.hierarchyClassID
LEFT JOIN Hierarchy brandh on brandh.hierarchyID = brandhc.hierarchyID
     WHERE brandh.hierarchyName = 'Brands'
       )
,
ItemSubbrick
AS (
       SELECT merchihc.itemID, merchhc.HierarchyClassId
         FROM ItemHierarchyClass merchihc
		 --JOIN ITEMS i on i.itemID = merchihc.itemID
 LEFT JOIN HierarchyClass merchhc on merchhc.HierarchyClassId = merchihc.hierarchyClassID
LEFT JOIN Hierarchy merchh on merchh.hierarchyID = merchhc.hierarchyID
     WHERE merchh.hierarchyName = 'Merchandise'
   )
,
Subteam
AS (
       SELECT ihc.itemID, hct.traitValue
         FROM ItemHierarchyClass ihc
		 --JOIN ITEMS i on i.itemID = ihc.itemID 
 LEFT JOIN HierarchyClassTrait hct on hct.hierarchyClassID = ihc.hierarchyClassID
LEFT JOIN Trait t on t.traitID = hct.traitID
     WHERE t.traitDesc = 'Merch Fin Mapping'
   )
SELECT RIGHT('0000000000000'+ISNULL(sc.scanCode,''),13) as UPC, dbo.fn_RemoveSpecialChars(d.traitValue) as UPC_LONG_LABEL, iu.traitValue as ITEM_UOM, dbo.fn_RemoveSpecialChars(b.hierarchyClassName) as BRAND, isb.HierarchyClassId as ITEM_SUBBRICK_ID, 
          SUBSTRING(s.traitValue, CHARINDEX('(', s.traitValue) + 1, CHARINDEX(')', s.traitValue) - CHARINDEX('(', s.traitValue) - 1) as PROD_SUBTEAM, LEFT(s.traitValue, CHARINDEX('(', s.traitValue) - 1 ) as SUBTEAM_NAME
from ScanCode sc
join ITEMS i on i.itemID = sc.itemID
left join DESCR d on sc.itemID = d.itemID
left join ITEM_UOM iu on sc.itemID = iu.itemID
left join BRAND b on sc.itemID = b.itemID 
left join ItemSubbrick isb on sc.itemID = isb.itemID
left join Subteam s on sc.itemID = s.itemID
order by sc.scanCode
END