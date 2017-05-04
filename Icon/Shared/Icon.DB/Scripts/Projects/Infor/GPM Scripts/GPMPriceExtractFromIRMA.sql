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
from [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchDetail] pbd
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchHeader] pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[Item] i on i.Item_key = pbd.Item_key
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemIdentifier] ii on i.Item_Key = ii.Item_key 
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[Store] s on s.Store_No = pbd.Store_No
where i.Deleted_Item = 0 and i.Remove_Item = 0
and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0 
and ii.Default_Identifier = 1
and pbh.PriceBatchStatusID = 6
and pbd.Expired = 0 
and pbd.PriceChgTypeId is Not null
and (s.WFM_Store = 1 or s.Mega_Store = 1)
--and s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
and pbd.StartDate <= @today
and (pbd.Sale_End_Date is null or pbd.Sale_End_Date >= @today)
--and pbd.Item_key = 286658
--and pbd.Store_No = 10633
Group by pbd.Item_Key, pbd.Store_No, s.BusinessUnit_ID

--select * from #LatestPBD

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
from [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchDetail] pbd
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchHeader] pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
join #LatestPBD lpbd on lpbd.Item_key = pbd.Item_key and lpbd.Store_No = pbd.Store_No
where pbd.Expired = 0 
and pbh.PriceBatchStatusID = 6
and pbd.PriceChgTypeId is Not null
and pbd.PriceBatchDetailID > lpbd.PriceBatchDetailId
and pbd.StartDate > @today

--select * from #FuturePBD

insert into #LatestPBD
select Item_Key, Store_No, BusinessUnit_ID, PriceBatchDetailId
from #FuturePBD 

--select * from #LatestPBD

select 
	sc.ItemId as 'Item ID', 
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
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[Item] i on l.Item_Key = i.Item_Key 
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemIdentifier] ii on l.Item_Key = ii.Item_Key
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchDetail] pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceChgType] pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUnit] runit on i.Retail_Unit_ID = runit.Unit_ID
join ScanCode sc on sc.ScanCode = ii.Identifier
left join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUomOverride] iuo on iuo.Item_Key = l.Item_Key and iuo.Store_No = l.Store_No
left join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUnit] rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
where pbd.Price is not null
and ii.Default_Identifier = 1
UNION
select 
	sc.ItemId as 'Item ID', 
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
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[Item] i on l.Item_Key = i.Item_Key 
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemIdentifier] ii on l.Item_Key = ii.Item_Key 
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceBatchDetail] pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[PriceChgType] pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUnit] runit on i.Retail_Unit_ID = runit.Unit_ID
join ScanCode sc on sc.ScanCode = ii.Identifier
left join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUomOverride] iuo on iuo.Item_Key = l.Item_Key and iuo.Store_No = l.Store_No
left join [IDQ-RM\RMQ].[ItemCatalog].[dbo].[ItemUnit] rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
where pbd.Sale_Price is not null
and ii.Default_Identifier = 1
