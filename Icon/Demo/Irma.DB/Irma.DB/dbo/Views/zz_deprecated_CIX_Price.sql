
create view [zz_deprecated_CIX_Price] as 
select
	i.Item_key												as Item_key,
	z.store													as Store_No,
	p.holdprice												as Price, 
	p.effprice * PCT.On_Sale								as Sale_Price, 
	p.holdprice												as POSPrice,
	p.effprice * PCT.On_Sale								as POSSale_Price, 
	case when PCT.On_Sale = 1 
		then p.lastdate
		else null
	end														as Sale_Start_Date, 
	case when PCT.On_Sale = 1 
		then dateadd(d, -1, p.salprend)
		else null 
	end														as Sale_End_Date,
	case 
		when p.rescode = '9' then 1
		when p.rescode = '09' then 1
		when p.dept = 510 and p.effprice = 0 then 1
		else 0
	end														as notAuthorizedForSale,
	PCT.PriceChgTypeId										as PriceChgTypeId,
	case 
		--when p.ptype = 'CMP' then 1
		when substring(p3.user_def10,1,2) = 'KV' then 1
		when substring(p3.user_def10,1,2) = 'BW' then 1
		else 0
	end														as compFlag,
	dbo.fn_Conv_isY(p.jrnprint)								as grillprint,
	case when p.rescode = '9' then 9
		when p.rescode = '09' then 9
		when isnumeric(p.agecode) = 1 then p.agecode
		else 0
	end														as agecode,
	dbo.fn_Conv_isY(p.visual)									as visualverify,
	0														as srcitizendiscount,
	p.tare													as postare,
	link.item_key											as LinkedItem,
	0														as PricingMethod_ID,
	p.effpm													as Sale_Multiple,
	case
		when p.msrp > 0 then p.msrp
		else 0
	end														as MSRPPrice,
	1														as MSRPMultiple,
	p.holdpm												as Multiple,
	case p.agecode
		when '2' then 1
		else 0
	end														as Restricted_Hours,
	dbo.fn_Conv_isY(p.si1)								as ibm_discount,
	case when i.SubTeam_No between 180 and 233 then isnull(p.mixmatch,0)
		when i.SubTeam_No between 360 and 371 then isnull(p.mixmatch,0)
	else 0	end												as MixMatch,
	case 
		when substring(p3.user_def10,1,2) = 'KV' then 3
		when substring(p3.user_def10,1,2) = 'BW' then 1
		else NULL
	end														as CompetitivePriceTypeId,
	case
		when substring(p3.user_def10,1,2) = 'BW' then 5
		else NULL end										as BandwidthPercentageHigh,
	case
		when substring(p3.user_def10,1,2) = 'BW' then 5
		else NULL end										as BandwidthPercentageLow,	
	p.lastdate												as Effective_Date
from 	cxspricd	p
inner join Item_Prep i
on i.upcno	= p.upcno
inner join CIX_Zoned z
on 		p.dept	= z.dept
	and	p.pzone	= z.pzone
inner join 	PriceChgType PCT
	on PCT.PriceChgTypeDesc = p.ptype
--on (	PCT.PriceChgTypeDesc = p.ptype
--		and dbo.fn_Conv_IsOnPromotion(p.salprend, p.ptype) = 1)
--	or
--	(	PCT.On_Sale = 0
--		and dbo.fn_Conv_IsOnPromotion(p.salprend, p.ptype) = 0)
left join Item_Prep link
 on link.upcno = (substring('0000000000000', 1, 12 - len(rtrim(cast(p.linkcode as bigint)))) + rtrim(p.linkcode) + '0')
 and link.Item_Key != 0
left outer JOIN cxsprc3d p3
 ON ( p3.upcno = p.upcno and
      p3.pzone = p.pzone and   
      p3.store = p.store )
where
	p.store = (select max(p2.store)
					from cxspricd p2
					where p2.upcno = p.upcno
					  and p2.pzone = p.pzone
					  and p2.store in (z.store, 0))	
	and i.Item_Key != 0