-- this is very similar to the InsertItemFromVIM.sql script
DELETE FROM scancode
WHERE  scancodetypeid IN ( 2, 3 )
delete from itemhierarchyclass where hierarchyclassid in 
(select hierarchyClassID from hierarchyclass where hierarchyClassName ='Test Scale PLU Brand')
delete from itemhierarchyclass where hierarchyclassid in 
(select hierarchyClassID from hierarchyclass where hierarchyClassName ='Test POS PLU Brand')
delete from hierarchyclass where hierarchyClassName ='Test Scale PLU Brand'      
delete from hierarchyclass where hierarchyClassName ='Test POS PLU Brand'

IF object_id('dbo.##SynthPLU') IS NOT NULL
   DROP TABLE ##SynthPLU

CREATE TABLE ##SynthPLU (
   [ScanCode] nvarchar( 255 ),
   [ProdDesc] nvarchar( 255 ),
   [POSDesc] nvarchar( 255 )
   );

BULK INSERT ##SynthPLU
   FROM  '\\irmadevfile\e$\ICONData\Synth_PLUs.txt'
   WITH  ( FirstRow = 2 )

INSERT INTO hierarchyclass
( hierarchyid, hierarchyclassname)
SELECT
   ( SELECT
        hierarchyid
     FROM   hierarchy
     WHERE  hierarchyname LIKE 'brand' ) AS 'hierarchyid',
   'Test POS PLU Brand'                  AS hierarchyclassname

INSERT INTO hierarchyclass
( hierarchyid, hierarchyclassname )
SELECT
   ( SELECT hierarchyid
     FROM   hierarchy
     WHERE  hierarchyname LIKE 'brand' ) AS 'hierarchyid',
   'Test Scale PLU Brand'                AS hierarchyclassname

DECLARE @LoadFromVim TABLE
   (
       scancode    NVARCHAR( 255 ),
       proddesc    NVARCHAR( 255 ),
       posdesc     NVARCHAR( 255 ),
       packageunit NVARCHAR( 255 )
   )
DECLARE @Itemtemp TABLE
   (
       itemid      INT,
       scancode    NVARCHAR( 255 ),
       proddesc    NVARCHAR( 255 ),
       posdesc     NVARCHAR( 255 ),
       packageunit NVARCHAR( 255 )
   )

INSERT INTO @LoadFromVim
(
   scancode,
   proddesc,
   posdesc,
   packageunit
)
SELECT
   vim.scancode ,
   vim.ProdDesc,
   vim.POSDesc,
   '1' AS packageunit
FROM   ##SynthPLU vim
ORDER  BY
   vim.scancode

--
select * from @LoadFromVIM
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
( itemid, scancode, scancodetypeid, localeID)
SELECT
   i.itemid,
   i.scancode,
   '2', -- pos plu
   1
FROM       @Itemtemp i
WHERE      i.proddesc LIKE '%POS PLU%'

INSERT INTO scancode
( itemid, scancode, scancodetypeid, localeID )
SELECT
   i.itemid,
   i.scancode,
   '3', -- scale plu
   1
FROM       @Itemtemp i
WHERE      i.proddesc LIKE '%Scale PLU%'

-- establish Item Brand relationship

INSERT INTO itemhierarchyclass
(itemid,hierarchyclassid)
SELECT sc.itemid, hc.hierarchyclassid
FROM   scancode sc
JOIN   hierarchyclass hc ON hc.hierarchyclassname LIKE '%Test%POS%Brand%'
JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                  AND h.hierarchyname = 'Brand'
JOIN   scancodetype sct ON sc.scancodetypeid = sct.scancodetypeid
                       AND sct.scancodetypedesc LIKE 'POS PLU'

INSERT INTO itemhierarchyclass
( itemid, hierarchyclassid)
SELECT sc.itemid, hc.hierarchyclassid
FROM   scancode sc
JOIN   hierarchyclass hc ON hc.hierarchyclassname LIKE '%Test%Scale%Brand%'
JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                  AND h.hierarchyname = 'Brand'
JOIN   scancodetype sct ON sc.scancodetypeid = sct.scancodetypeid
                       AND sct.scancodetypedesc LIKE 'Scale PLU'


SELECT
   *
FROM   scancode
WHERE  scancodetypeid IN ( 2, 3 ) 
Drop table ##SynthPLU
