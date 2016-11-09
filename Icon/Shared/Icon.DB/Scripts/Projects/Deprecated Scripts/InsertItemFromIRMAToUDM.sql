--INSERT INTO item
DECLARE @Load TABLE
   (
       itemdesc     VARCHAR( 60 ),
       scancode     VARCHAR( 100 ),
       shortdesc    VARCHAR( 100 ),
       packageunit  VARCHAR( 100 ),
       foodstamp    VARCHAR( 100 ),
       posscaletare VARCHAR( 100 )
   )
DECLARE @Itemtmp TABLE
   (
       itemid       INT,
       localeid     INT,
       scancode     VARCHAR( 100 ),
       shortdesc    VARCHAR( 100 ),
       packageunit  VARCHAR( 100 ),
       foodstamp    VARCHAR( 100 ),
       posscaletare VARCHAR( 100 )
   )

INSERT INTO @Load
SELECT
   i.item_description,
   Cast(ii.identifier AS VARCHAR( 100 )),
   Cast(i.pos_description AS VARCHAR( 100 )),
   Cast(i.package_desc1 AS VARCHAR( 100 )),
   Cast(i.food_stamps AS VARCHAR( 100 )),
   Cast(IsNull(i.scaletare, 0) AS VARCHAR( 100 ))
FROM       [idt-sw].[ItemCatalog_Test].[dbo].item i(nolock)
INNER JOIN [idt-sw].[ItemCatalog_Test].[dbo].itemidentifier ii(nolock) ON i.item_key = ii.item_key
WHERE      i.deleted_item                   = 0
           AND ii.deleted_identifier = 0
           AND i.subteam_no                   = 110

MERGE INTO item
using @Load AS tmp
ON 1 = 0
WHEN NOT matched THEN
   INSERT (itemdesc,
           localeid)
   VALUES (tmp.itemdesc,
           1) --1=global/chain localeid
output inserted.itemid,
       inserted.localeid,
       tmp.scancode,
       tmp.shortdesc,
       tmp.packageunit,
       tmp.foodstamp,
       tmp.posscaletare
INTO @Itemtmp (itemID, localeid, scancode, shortdesc, packageunit, foodstamp, posscaletare);

INSERT INTO itemtrait
(
   itemid,
   localeid,
   traitvalue,
   traitcode
)
SELECT
   i.itemid,
   i.localeid,
   i.scancode,
   t.traitcode
FROM       @Itemtmp i
CROSS JOIN ( SELECT
                traitcode
             FROM   trait
             WHERE  traitdesc = 'ScanCode' ) t
UNION ALL
SELECT
   i.itemid,
   i.localeid,
   i.shortdesc,
   t.traitcode
FROM       @Itemtmp i
CROSS JOIN ( SELECT
                traitcode
             FROM   trait
             WHERE  traitdesc = 'Short Description' ) t
UNION ALL
SELECT
   i.itemid,
   i.localeid,
   i.packageunit,
   t.traitcode
FROM       @Itemtmp i
CROSS JOIN ( SELECT
                traitcode
             FROM   trait
             WHERE  traitdesc = 'Package Unit' ) t
UNION ALL
SELECT
   i.itemid,
   i.localeid,
   i.foodstamp,
   t.traitcode
FROM       @Itemtmp i
CROSS JOIN ( SELECT
                traitcode
             FROM   trait
             WHERE  traitdesc = 'Food Stamp Eligible' ) t
UNION ALL
SELECT
   i.itemid,
   i.localeid,
   i.posscaletare,
   t.traitcode
FROM       @Itemtmp i
CROSS JOIN ( SELECT
                traitcode
             FROM   trait
             WHERE  traitdesc = 'POS Scale Tare' ) t 
