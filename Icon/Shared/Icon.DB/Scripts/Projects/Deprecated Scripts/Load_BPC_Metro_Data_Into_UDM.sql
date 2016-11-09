/*

IF object_id('dbo.##StoreTemp') IS NOT NULL
   DROP TABLE ##StoreTemp

CREATE TABLE ##StoreTemp (
   [region] nvarchar( 255 ),
   [metro]  nvarchar( 255 ),
   [bu]     nvarchar( 255 ),
   [desc]   nvarchar( 255 ),
   [status] nvarchar( 255 ));

BULK INSERT ##StoreTemp
   FROM '\\irmadevfile\e$\ICONData\BPC_Store_List.txt'
   WITH
      (
         FirstRow = 2
      )
DECLARE @LocaleMetroTemp TABLE (
   localename     varchar( 255 ),
   localeopendate datetime,
   parentlocaleid int)
DECLARE @LocaleStoreTemp TABLE (
   localename     varchar( 255 ),
   localeopendate datetime,
   parentlocaleid int)
DECLARE @MetroTemp TABLE (
   localeid   int,
   localename varchar( 255 ))
DECLARE @StoreTemp TABLE (
   localeid       int,
   localename     varchar( 255 ),
   parentlocaleid int)

INSERT INTO @LocaleMetroTemp
SELECT
   localename,
   localeopendate,
   parentlocaleid
FROM
   ( SELECT
        zzl.metro    AS localeName,
        '09/22/1980' AS localeOpenDate,
        NULL         AS localeCloseDate,
        (select localetypeid from localetype where localetypecode ='MT')         AS localeTypeID,
        lt.localeid  AS parentlocaleid
     FROM
        locale l
     INNER JOIN localetrait lt ON l.localeid = lt.localeid
     INNER JOIN ( SELECT
                     DISTINCT
                     region,
                     metro
                  FROM
                     ##StoreTemp ) zzl ON lt.traitvalue = zzl.region
     WHERE
      lt.traitID = (select traitid from trait where traitcode='ABB') ) localemetrotemp -- Region Abbreviation
-- Populate the Locale table with Metro data
MERGE INTO locale
USING @LocaleMetroTemp AS lt
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( ownerorgpartyid,
            localename,
            localeopendate,
            localeTypeID,
            parentlocaleid)
   VALUES ( 1,
            lt.localename,
            '09/22/1980',
            (select localetypeid from localetype where localetypecode ='MT'),
            lt.parentlocaleid)
OUTPUT inserted.localeid,
       lt.localename
INTO @MetroTemp (localeID, localeName);

--Setting up @LocaleStoreTemp table 
INSERT INTO @LocaleStoreTemp
SELECT
   localename,
   localeopendate,
   parentlocaleid
FROM
   ( SELECT
        DISTINCT
        zzl.[desc]   AS localeName,
        CASE WHEN zzl.status = 'Open' THEN '09/23/1980'
        ELSE NULL
        END          AS localeOpenDate,
        lmt.localeid AS parentLocaleID
     FROM
        ##StoreTemp zzl
     INNER JOIN @MetroTemp lmt ON zzl.metro = lmt.localename ) localestoretemp

--SELECT * FROM @LocaleStoreTemp ORDER BY localeName, parentLocaleID
-- Populate Locale table with Store data
MERGE INTO locale
USING @LocaleStoreTemp AS lst
ON 1 = 0
WHEN NOT MATCHED THEN
   INSERT ( ownerorgpartyid,
            localename,
            localeopendate,
            localeTypeID,
            parentlocaleid)
   VALUES ( 1,
            lst.localename,
            lst.localeopendate,
            (select localetypeid from localetype where localetypecode ='ST'),
            lst.parentlocaleid)
OUTPUT inserted.localeid,
       lst.localename,
       inserted.parentlocaleid
INTO @StoreTemp (localeID, localeName, parentLocaleID);

-- Populate Store table with localeID 
INSERT INTO store
            (storeid)
SELECT
   s.localeid AS storeID
FROM
   @Storetemp s

-- Populate LocaleTrait table with Business Unit values
INSERT INTO localetrait
            (traitID,
             localeid,
             traitvalue)
SELECT
   DISTINCT
   (select traitid from trait where traitcode='BU'),
   foo.localeid,
   bar.bu
FROM
   @Storetemp foo
INNER JOIN locale l ON foo.parentlocaleid = l.localeid
INNER JOIN ##StoreTemp bar ON foo.localename = bar.[desc]
                              AND l.localename = bar.[metro]

DROP TABLE ##StoreTemp 

*/