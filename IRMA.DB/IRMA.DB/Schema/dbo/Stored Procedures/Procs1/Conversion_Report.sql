
/****** Object:  StoredProcedure [dbo].[Conversion_Report]    Script Date: 05/11/2007 14:52:59 ******/

CREATE PROCEDURE [dbo].[Conversion_Report] 
AS
---- =================================================================================================
---- Author:		Dave Stacey
---- Create date: 2007.01.24
---- Description:	This script is intended to provide a health check for the data conversion process.
---- It is an automated data check which, when run, issues summary data regarding the results.
---- =================================================================================================
BEGIN

SET NOCOUNT ON

DECLARE @Data_Conversion_Report TABLE ([ConversionID] [int] IDENTITY(1,1) NOT NULL,
	[ConversionDesc] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CIXCount] [int] NULL,
	[IRMACount] [int] NULL,
	[CreateDate] [datetime] NOT NULL  DEFAULT (getdate()))

DECLARE @CIX INT, @IRMA INT
SET @CIX = 0
SET @IRMA = 0

select @CIX = Count(upcno) from (select upcno from  
     [dbo].cxbupcmr
group by upcno) AS CIX

select @IRMA  = Count(identifier) from (select identifier from dbo.ItemIdentifier
group by identifier) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'DISTINCT UPCs ', @CIX , @IRMA

select @CIX = Count(longdesc) from (select longdesc from 
[dbo].cxbupcmr group by longdesc) AS CIX

select @IRMA  = Count(item_description) from (select item_description from dbo.Item
group by item_description) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'DISTINCT Item Desc ', @CIX , @IRMA

select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX

select @CIX = Count(longdesc) from (select longdesc from 
[dbo].cxbupcmr group by longdesc) AS CIX

select @IRMA  = Count(sign_description) from (select sign_description from dbo.Item
group by sign_description) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'DISTINCT POS Desc ', @CIX , @IRMA



select @CIX = Count(upcno) from cxbupcmr where upcno in (select upcno 
from [dbo].cxspricd WHERE Cast(linkcode as bigint) > 0 )  


select @IRMA  = Count(item_key) from (select item_key 
from dbo.Price WHERE LinkedItem > 0 group by item_key) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'Gross ITEMs with LinkCodes ', @CIX , @IRMA

select @CIX = Count(linkcode) from (select linkcode 
from [dbo].cxspricd WHERE LEN(LTRIM(RTRIM(linkcode))) > 0 group by linkcode)  AS CIX

select @IRMA  = Count(LinkedItem) from (select LinkedItem 
from dbo.Price WHERE LinkedItem > 0 group by LinkedItem) AS IRMA


INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'Unique ITEM LinkCodes ', @CIX , @IRMA
select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX


select @CIX = count(chain_code)
from [dbo].cxbupcmr where LEN(LTRIM(RTRIM(chain_code))) > 0

--select @IRMA  = Count(chaincode) from (select chaincode 
--from dbo.Price where chaincode > 0 group by chaincode) AS IRMA

select @IRMA  = 0

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'ITEM ChainCodes ', @CIX , @IRMA

select @CIX = COUNT(BID.miscalpha1) from 
    (select distinct miscalpha1 from [dbo].cxbupcmr
		WHERE LEN(LTRIM(RTRIM(miscalpha1))) > 0) as BID

select @IRMA = count(Brand_ID) from dbo.itembrand

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'DISTINCT BRANDS ', @CIX , @IRMA

select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX

select @IRMA  = Count(subteam_no) from (select subteam_no from dbo.Item
group by subteam_no) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'DISTINCT ITEM SUBTEAMS ', @CIX , @IRMA

select @CIX = Count(itemUOM) from (select itemUOM 
from [dbo].cxbupcmr group by itemUOM) AS CIX

select @IRMA  = Count(Package_Unit_ID) from (select Package_Unit_ID 
from dbo.Item group by Package_Unit_ID) AS IRMA

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'ITEM UOM ', @CIX , @IRMA

select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX

select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX


select @CIX = count(*) from dbo.vendor

select @IRMA = count(*) from dbo.cxbvendr 


INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'Vendors ', @CIX , @IRMA
select @CIX = Count(dept) from (select dept from [dbo].cxbupcmr 
group by dept) AS CIX

select @CIX = count(upcno) from 
 dbo.cxbupcmr
where upcno not in (
select cast(upcno as bigInt) from 
[dbo].[upc_vendr]
)

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - NO VENDOR', @CIX , 0

select @CIX = COUNT(u.upcno) from [dbo].cxbupcmr u GROUP BY u.upcno HAVING count(u.upcno) > 1

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - Duplicate UPCs', @CIX , 0


select @CIX = count(a.upcno) 
from 
(select u.upcno 
from
     [dbo].cxbupcmr u
LEFT OUTER JOIN [dbo].cmmclasr cl 
           on u.class = cl.class AND u.family = cl.family and u.category = cl.category
where isnumeric(CL.category) = 0
GROUP BY u.upcno ) a

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - No Matching Class', @CIX , 0


select @CIX = Count(linkcode) from (select linkcode 
from [dbo].cxspricd WHERE LEN(LTRIM(RTRIM(linkcode))) > 0 and linkcode not in 
(select upcno from [dbo].cxbupcmr)
group by linkcode)  AS CIX


INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - Invalid LinkCodes', @CIX , 0

select @CIX = count(*) from dbo.cxbupcmr where class = 0 or class is null


INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - 0 Class ID', @CIX , 0

select @CIX = count(upcno) from (select upcno from cost_vendr cst where cst.casesize = 0 group by upcno) a

INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - Case Size of 0', @CIX , 0

select upcno, longdesc from 
cxbupcmr 
where upcno in (select p.upcno
from cxspricd p
where p.effprice = 0
)
and 
 upcno not in (select p.upcno
from cxspricd p
where p.effprice > 0)  

select @CIX = count(upcno) from 
cxbupcmr 
where upcno in (select p.upcno
from cxspricd p
where p.effprice = 0
)
and 
 upcno not in (select p.upcno
from cxspricd p
where p.effprice > 0)  

select @IRMA = count(item_key) from 
dbo.price 
where item_key in (select p.item_key
from dbo.price p
where p.price = 0
)
and 
 item_key not in (select p.item_key
from price p
where p.price > 0) 
 
select item_key, count(item_key) from 
dbo.price 
where item_key in (select p.item_key
from dbo.price p
where p.price = 0
)
and 
 item_key not in (select p.item_key
from price p
where p.price > 0) 
and price = 0
group by item_key



INSERT @Data_Conversion_Report (ConversionDesc, CIXCount, IRMACount)
SELECT 'CIX Items - No Price', @CIX , 0



Select 'Conversion Description' = ConversionDesc,
CIXCount, IRMACount from @Data_Conversion_Report
END
