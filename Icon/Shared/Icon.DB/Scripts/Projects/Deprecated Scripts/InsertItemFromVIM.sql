DELETE FROM app.PLUMap
DELETE FROM itemhierarchyclass
DELETE FROM hierarchyclass
DBCC CHECKIDENT('hierarchyclass', RESEED, 0)
DELETE FROM itemtrait
DELETE FROM scancode
DBCC CHECKIDENT('scancode', RESEED, 0)
DELETE FROM item
DBCC CHECKIDENT('item', RESEED, 0)
DELETE FROM hierarchyprototype WHERE hierarchyid = 2

-- let's get the brands into the hierarchy table
INSERT INTO hierarchyprototype
            (hierarchyid,
             hierarchylevel,
             hierarchylevelname,
			 itemsAttached)
VALUES      (2,
             1,
             'Brand',
			 1)
INSERT INTO hierarchyclass
(
   hierarchyid,
   hierarchyLevel,
   hierarchyclassname
)
SELECT
   (select hierarchyid from hierarchy where hierarchyname like 'brand') as [hierarchyid],
   1 as [hiearchylevel],
   vim.brand as [hierarchyclassname]
FROM   [app].zzvimstaging vim
GROUP  BY
   vim.brand

-- now let's create some tables
-- this one will only be UPCs
DECLARE @LoadFromVIM TABLE
   (
       scancode    NVARCHAR( 255 ),
       proddesc    NVARCHAR( 255 ),
       posdesc   NVARCHAR( 255 ),
       packageunit NVARCHAR( 255 )
   )
-- this table will have the outputted item id and will be joined to the Load table so we can stick these into Item Trait n Stuff
DECLARE @ItemTemp TABLE
   (
       itemid      INT,
       scancode    NVARCHAR( 255 ),
       proddesc    NVARCHAR( 255 ),
       posdesc   NVARCHAR( 255 ),
       packageunit NVARCHAR( 255 )
   )
-- iconpop load
IF object_id('dbo.##ICONPop') IS NOT NULL
   DROP TABLE ##ICONPop

CREATE TABLE ##ICONPop (
   [upc] nvarchar( 255 ));

BULK INSERT ##ICONPop
   FROM  '\\irmadevfile\e$\ICONData\Icon IPOP 4-8.txt'
   WITH  ( FirstRow = 2 )

UPDATE ##ICONPop
SET [upc] = SUBSTRING([upc], PATINDEX('%[^0 ]%', [upc] + ' '), LEN([upc]))

-- only upcs
INSERT INTO @LoadFromVIM
(
   scancode,
   proddesc,
   posdesc,
   packageunit
)
SELECT
   vim.scancode,
   vim.productdescription,
   vim.posdescription,
   '1'
FROM   [app].zzvimstaging vim
JOIN  ##ICONPop ip on vim.scancode = ip.upc
--WHERE  vim.scancode NOT LIKE ( '2%00000' )
--       AND len(vim.scancode) > 6
ORDER  BY
   vim.scancode

-- item table only has one field we gotta insert:
-- and here's merge:
MERGE INTO item
using @LoadFromVIM AS tmp
ON 1 = 0
WHEN NOT matched THEN
   INSERT (ItemTypeID)
   VALUES (1)
output inserted.itemid,  -- this is why we used 'merge'
       tmp.scancode,
       tmp.proddesc,
       tmp.posdesc,
       tmp.packageunit
INTO @ItemTemp (itemid, scancode, proddesc, posdesc, packageunit);

-- now that we have our item id, we can fill in the item attributes:
INSERT INTO itemtrait
(
   itemid,
   traitvalue,
   traitID,
   localeID
)
SELECT
   i.itemid,
   i.proddesc,
   (select traitid from trait where traitcode='PRD'),
   1 -- default to localeId = 1
FROM       @ItemTemp i
UNION ALL
SELECT
   i.itemid,
   i.posdesc,
   (select traitid from trait where traitcode='POS'),
   1 -- default to localeId = 1
FROM       @ItemTemp i
UNION ALL
SELECT
   i.itemid,
   i.packageunit,
   (select traitid from trait where traitcode='PKG'),
   1 -- default to localeId = 1
FROM       @ItemTemp i
UNION ALL
SELECT
   i.itemid,
   NULL,
   (select traitid from trait where traitcode='FSE'),
   1 -- default to localeId = 1
FROM       @ItemTemp i
UNION ALL
SELECT
   i.itemid,
   NULL,
   (select traitid from trait where traitcode='SCT'),
   1 -- default to localeId = 1
FROM       @ItemTemp i
UNION ALL
SELECT
   i.itemid,
   '1',-- for initial load, UPCs will have a Version = 1. as they're recycled, this should increment by 1
   (select traitid from trait where traitcode='VER'),
   1 -- default to localeId = 1
FROM       @ItemTemp i

-- put the scancodes into their table
INSERT INTO scancode
(
   itemid,
   scancode,
   scancodetypeid,
   localeID
)
SELECT
   i.itemid,
   i.scancode,
   t.scancodetypeid,
   1
FROM       @ItemTemp i
CROSS JOIN ( SELECT
                scancodetypeid
             FROM   scancodetype
             WHERE  scancodetypedesc = 'UPC' ) t

-- establish Item Brand relationship
INSERT INTO itemhierarchyclass
(
   itemid,
   hierarchyclassid
)
SELECT
   sc.itemid,
   hc.hierarchyclassid
FROM   [app].zzvimstaging vim
JOIN   hierarchyclass hc ON vim.brand = hc.hierarchyclassname
JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                  AND h.hierarchyname = 'Brand'
JOIN   scancode sc ON vim.scancode = sc.scancode
JOIN   scancodetype sct ON sc.scancodetypeid = sct.scancodetypeid
                       AND sct.scancodetypedesc = 'UPC'
/*
2/13/14 results (lux) 1 min 8 sec
	(1045976 row(s) affected) -- @LoadFromVIM
	(1045976 row(s) affected) -- item
	(3137928 row(s) affected) -- itemtrait
	(1045976 row(s) affected) -- scancode
	(1045792 row(s) affected) -- itemhierarchyclass

2/23/14 results (td) 5 min 26 sec
	(896960 row(s) affected)	-- @LoadFromVIM
	(896960 row(s) affected)	-- item
	(3587840 row(s) affected)	-- itemtrait
	(896960 row(s) affected)	-- scancode
	(896825 row(s) affected)	-- itemhierarchyclass


 */
-- ok! now run the insert sythentic plu info, if necessary

drop table ##ICONPop