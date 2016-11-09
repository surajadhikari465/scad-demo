-- be good
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

-- get your ducks in a row
DECLARE @srvr      VARCHAR(6),
        @Link      VARCHAR(100),
        @db        VARCHAR(50) = '[itemcatalog_test]',
        @sql       NVARCHAR(max),
        @GetServer CURSOR

-- go get your linked servers
SET @GetServer = CURSOR
FOR SELECT
       srv_name = srv.name
    FROM   sys.servers srv
    WHERE  is_linked = 1

-- fire up the cursor and loop
OPEN @GetServer

FETCH NEXT FROM @GetServer INTO @srvr

WHILE @@FETCH_STATUS = 0
   BEGIN
      SELECT
         @Link = '[' + @srvr + '].' + @db

      SELECT
         @sql = '
DECLARE @Load TABLE
   (
       localename     NVARCHAR( 255 ), -- store name
       storegroupname NVARCHAR( 255 ), -- jurisdiction
	   bizunitnum	NVARCHAR( 255 )
   )

INSERT INTO @Load
SELECT
   Cast(s.store_name AS NVARCHAR( 255 ))             AS localename,-- localeName
   Cast(sj.storejurisdictiondesc AS NVARCHAR( 255 )) AS storegroupname,
   Cast(s.businessunit_id AS NVARCHAR( 255 )) AS bizunitnum
-- ADDRESS INFORMATION + 
FROM   ' + @Link + '.dbo.store s (nolock)
JOIN   ' + @Link + '.dbo.vendor v (nolock) ON v.store_no = s.store_no
JOIN   ' + @Link + '.dbo.storejurisdiction sj (nolock) ON s.storejurisdictionid = sj.storejurisdictionid

DECLARE @Storetemp TABLE
   (
       storeid        INT,
       localename     NVARCHAR( 255 ),
       storegroupname NVARCHAR( 255 )
   )

MERGE INTO locale
using @Load AS tmp
ON 1 = 0
WHEN NOT matched THEN
   INSERT (ownerorgpartyid,
           localename,
           localetypecode,
           parentlocaleid)
   VALUES (1,
           tmp.localename,
           4,-- store type code!
           ( SELECT
                l.localeid
             FROM   locale l
             JOIN   localetrait lt ON l.localeid = lt.localeid
             JOIN   trait t ON lt.traitcode = t.traitcode
                           AND t.traitdesc = ''Region Abbreviation''
             JOIN   ' + @Link + '.dbo.region r ON r.regioncode = lt.traitvalue )) 
output inserted.localeid,
       tmp.storegroupname
INTO @Storetemp (storeid, storegroupname);

INSERT INTO store
(
   storeid
)
SELECT
   s.storeid
FROM   @Storetemp s

INSERT INTO storegroupmember
(
   storeid,
   storegroupid,
   storegrouptypeid
)
SELECT
   s.storeid,
   sg.storegroupid,
   1
FROM   @Storetemp s
JOIN   storegroup sg ON s.storegroupname = sg.storegroupname


		/*
			Add locale traits:
			Biz Unit Number (from @Load.bizunitnum)
		*/
		declare @buTraitCode int; select @buTraitCode = traitCode from trait where traitDesc = ''PS Business Unit ID''

		insert into localetrait (
			[traitCode]
			,[localeID]
			,[uomID]
			,[traitValue]
		)
		select
			[traitCode] = @buTraitCode
			,[localeID] = lo.localeID
			,[uomID] = null
			,[traitValue] = ld.bizunitnum
		from @load ld
		join locale lo
		on ld.localename = lo.localeName
'

      EXEC sp_executesql
         @sql

		


      FETCH NEXT FROM @GetServer INTO @srvr
   END

-- clean it all up
CLOSE @GetServer

DEALLOCATE @GetServer

-------------------------------------------------------------------------------
-- LET'S CLEAN UP A LITTLE BIT 
-------------------------------------------------------------------------------
--delete StoreGroupMember
--delete store
--delete locale where localeid > 4
-------------------------------------------------------------------------------
-- HERE IS THE WORK TO GET THE LOCALE ID 
-------------------------------------------------------------------------------
--DECLARE @Load TABLE
--   (
--       localename     NVARCHAR( 255 ),
--       storegroupname NVARCHAR( 255 )
--   )

--INSERT INTO @Load
--SELECT
--   Cast(s.store_name AS NVARCHAR( 255 ))             AS localename,-- localeName
--   Cast(sj.storejurisdictiondesc AS NVARCHAR( 255 )) AS storegroupname
---- ADDRESS INFORMATION + 
--FROM   [idd-mw].[ItemCatalog_Test].[dbo].store s (nolock)
--JOIN   [idd-mw].[ItemCatalog_Test].[dbo].vendor v (nolock) ON v.store_no = s.store_no
--JOIN   [idd-mw].[ItemCatalog_Test].[dbo].storejurisdiction sj (nolock) ON s.storejurisdictionid = sj.storejurisdictionid

--DECLARE @Storetemp TABLE
--   (
--       storeid        INT,
--       localename     NVARCHAR( 255 ),
--       storegroupname NVARCHAR( 255 )
--   )

--MERGE INTO locale
--using @Load AS tmp
--ON 1 = 0
--WHEN NOT matched THEN
--   INSERT (ownerorgpartyid,
--           localename,
--           localetypecode,
--           parentlocaleid)
--   VALUES (1,
--           tmp.localename,
--           4,-- store type code!
--           ( SELECT
--                l.localeid
--             FROM   locale l
--             JOIN   localetrait lt ON l.localeid = lt.localeid
--             JOIN   trait t ON lt.traitcode = t.traitcode
--                           AND t.traitdesc = 'Region Abbreviation'
--             JOIN   [idd-mw].[ItemCatalog_Test].[dbo].region r ON r.regioncode = lt.traitvalue )) 
--output inserted.localeid,
--       tmp.storegroupname
--INTO @Storetemp (storeid, storegroupname);

--INSERT INTO store
--(
--   storeid
--)
--SELECT
--   s.storeid
--FROM   @Storetemp s

--INSERT INTO storegroupmember
--(
--   storeid,
--   storegroupid,
--   storegrouptypeid
--)
--SELECT
--   s.storeid,
--   sg.storegroupid,
--   1
--FROM   @Storetemp s
--JOIN   storegroup sg ON s.storegroupname = sg.storegroupname

--go
-------------------------------------------------------------------------------
-- LET'S SEE OUR RESULTS! 
-------------------------------------------------------------------------------
SELECT
   rg.localename as 'Region',
   st.localename as 'Store Name',
   sg.storegroupname as 'Jurisdiction'
FROM   store s
JOIN   locale st ON s.storeid = st.localeid
JOIN   locale rg on st.parentLocaleID = rg.localeID
JOIN   storegroupmember sgm ON s.storeid = sgm.storeid
JOIN   storegroup sg ON sgm.storegroupid = sg.storegroupid
JOIN   storegrouptype sgt ON sgm.storegrouptypeid = sgt.storegrouptypeid
                         AND sgt.storegrouptypedesc = 'IRMA Store Jurisdiction' 
