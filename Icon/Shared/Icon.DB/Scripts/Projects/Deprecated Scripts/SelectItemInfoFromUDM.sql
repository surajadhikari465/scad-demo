SELECT
   i.itemdesc     AS item_description,
   scn.traitvalue AS identifier,
   sht.traitvalue AS pos_description,
   pu.traitvalue  AS package_desc1,
   fse.traitvalue AS food_stamps,
   pst.traitvalue AS scaletare
FROM   item i
JOIN   itemtrait scn ON i.itemid = scn.itemid
                    AND i.localeid = scn.localeid
JOIN   trait tscn ON scn.traitcode = tscn.traitcode
                 AND tscn.traitdesc = 'ScanCode'
JOIN   itemtrait sht ON i.itemid = sht.itemid
                    AND i.localeid = sht.localeid
JOIN   trait tsht ON sht.traitcode = tsht.traitcode
                 AND tsht.traitdesc = 'Short Description'
JOIN   itemtrait pu ON i.itemid = pu.itemid
                   AND i.localeid = pu.localeid
JOIN   trait tpu ON pu.traitcode = tpu.traitcode
                AND tpu.traitdesc = 'Package Unit'
JOIN   itemtrait fse ON i.itemid = fse.itemid
                    AND i.localeid = fse.localeid
JOIN   trait tfse ON fse.traitcode = tfse.traitcode
                 AND tfse.traitdesc = 'Food Stamp Eligible'
JOIN   itemtrait pst ON i.itemid = pst.itemid
                    AND i.localeid = pst.localeid
JOIN   trait tpst ON pst.traitcode = tpst.traitcode
                 AND tpst.traitdesc = 'POS Scale Tare'
ORDER  BY
   scn.traitvalue

go

SELECT
   i.item_description,
   ii.identifier,
   i.pos_description,
   i.package_desc1,
   i.food_stamps,
   i.scaletare
FROM       [idt-sw].[ItemCatalog_Test].[dbo].item i(nolock)
INNER JOIN [idt-sw].[ItemCatalog_Test].[dbo].itemidentifier ii(nolock) ON i.item_key = ii.item_key
WHERE      i.deleted_item                   = 0
           AND ii.deleted_identifier = 0
           AND i.subteam_no                   = 110
ORDER      BY
   ii.identifier

go 
