CREATE PROCEDURE infor.PriceExtractQuery
AS
BEGIN
DECLARE @today DATETIME = CONVERT(DATE, getdate()) 
       --,@ExcludedStoreNo varchar(250) = ''--(SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))

IF Object_id('tempdb..#LatestPBD') IS NOT NULL
		DROP TABLE #LatestPBD

CREATE TABLE #LatestPBD(
		[Item_Key] [int] NOT NULL,
		[Store_No] [int] NOT NULL,
		[BusinessUnit_ID] [int] NOT NULL,
		[PriceBatchDetailId] [int] NULL,
		CONSTRAINT [PK_LatestPBD] PRIMARY KEY CLUSTERED 
		(
			[Item_Key] ASC,
			[Store_No] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

insert into #LatestPBD
select pbd.Item_Key, pbd.Store_No, s.BusinessUnit_ID, max(pbd.PriceBatchDetailId)
from PriceBatchDetail pbd
join PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
join Item i on i.Item_key = pbd.Item_key
join ItemIdentifier ii on i.Item_Key = ii.Item_key 
join Store s on s.Store_No = pbd.Store_No
join StoreItem si on si.Store_No = s.Store_No and si.Item_Key = i.Item_Key
where i.Deleted_Item = 0 and i.Remove_Item = 0
and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0 
and ii.Default_Identifier = 1
and pbh.PriceBatchStatusID = 6
and pbd.Expired = 0 
and pbd.PriceChgTypeId is Not null
and (s.WFM_Store = 1 or s.Mega_Store = 1)
--and s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
and si.Authorized = 1
and pbd.StartDate <= @today
and (pbd.Sale_End_Date is null or pbd.Sale_End_Date >= @today)
Group by pbd.Item_Key, pbd.Store_No, s.BusinessUnit_ID

IF Object_id('tempdb..#FuturePBD') IS NOT NULL
		DROP TABLE #FuturePBD

CREATE TABLE #FuturePBD(
		[Item_Key] [int] NOT NULL,
		[Store_No] [int] NOT NULL,
		[BusinessUnit_ID] [int] NOT NULL,
		[PriceBatchDetailId] [int] NULL,
		CONSTRAINT [PK_FuturePBD] PRIMARY KEY CLUSTERED 
		(
			[Item_Key] ASC,
			[Store_No] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

insert into #FuturePBD
select pbd.Item_Key, pbd.Store_No,lpbd.BusinessUnit_ID, pbd.PriceBatchDetailId
from PriceBatchDetail pbd
join PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
join #LatestPBD lpbd on lpbd.Item_key = pbd.Item_key and lpbd.Store_No = pbd.Store_No
where pbd.Expired = 0 
and pbh.PriceBatchStatusID = 6
and pbd.PriceChgTypeId is Not null
and pbd.PriceBatchDetailID > lpbd.PriceBatchDetailId
and pbd.StartDate > @today

insert into #LatestPBD
select Item_Key, Store_No, BusinessUnit_ID, PriceBatchDetailId
from #FuturePBD 

select 
	vsc.inforItemId as 'Item ID', 
	ii.Identifier as 'Scan Code', 
	pbd.Price as Price, 
	'REG' as 'Price Type', 
	case 
		when pbd.Sale_End_Date is null 
			then pbd.StartDate 
		else Convert(Date,pbd.Sale_End_Date + 1, 102) 
		end as 'Start Date', 
	NULL as 'End Date', 
	pbd.Insert_Date as 'Insert Date', 
	case 
		when ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			then ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation)
		 else 'EA' end as 'Selling UOM', 
	pbd.Multiple, 
	l.BusinessUnit_ID as Location
from #LatestPBD l
join Item i on l.Item_Key = i.Item_Key 
join ItemIdentifier ii on l.Item_Key = ii.Item_Key
join PriceBatchDetail pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
join PriceChgType pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
join ItemUnit runit on i.Retail_Unit_ID = runit.Unit_ID
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
left join ItemUomOverride iuo on iuo.Item_Key = l.Item_Key and iuo.Store_No = l.Store_No
left join ItemUnit rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
where pbd.Price is not null
and ii.Default_Identifier = 1
UNION
select 
	vsc.inforItemId as 'Item ID', 
	ii.Identifier as 'Scan Code', 
	pbd.Sale_Price as Price, 
	pct.PriceChgTypeDesc as 'Price Type', 
	pbd.StartDate as 'Start Date', 
	pbd.Sale_End_Date as 'End Date', 
	pbd.Insert_Date as 'Insert Date', 
	case 
		when ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			then ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation)
		 else 'EA' end as 'Selling UOM',
	pbd.Multiple, 
	l.BusinessUnit_ID as Location
from #LatestPBD l
join Item i on l.Item_Key = i.Item_Key 
join ItemIdentifier ii on l.Item_Key = ii.Item_Key 
join PriceBatchDetail pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
join PriceChgType pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
join ItemUnit runit on i.Retail_Unit_ID = runit.Unit_ID
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
left join ItemUomOverride iuo on iuo.Item_Key = l.Item_Key and iuo.Store_No = l.Store_No
left join ItemUnit rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
where pbd.Sale_Price is not null
and ii.Default_Identifier = 1
End