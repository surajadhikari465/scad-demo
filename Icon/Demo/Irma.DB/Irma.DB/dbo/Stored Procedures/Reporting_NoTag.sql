
CREATE PROCEDURE [dbo].[Reporting_NoTag]
	@StoreNumber int,
	@BatchDescription varchar(max) = null,	
	@BeginStartDate datetime,
	@EndStartDate datetime = null
AS
BEGIN	
	set nocount on

	set transaction isolation level read uncommitted

	if @StoreNumber = 0
		begin
			set @StoreNumber = NULL
		end

	if @BatchDescription is not null or @BatchDescription = ''
		begin
			set @BatchDescription = '%' + @BatchDescription + '%'
		end

	if @EndStartDate is null or @EndStartDate = '1-1-1'
		begin
			set @EndStartDate = '2100-01-01'
		end
	else
		begin
			set @EndStartDate = DATEADD(DAY, 1, @EndStartDate)
		end
	
	select
		ii.Identifier,
		st.SubTeam_Name as [SubTeam],
		ib.Brand_Name as [Brand],
		i.Item_Description as [Item Description],
		s.Store_Name as [Store],
		sal.Date_Key as [Last Sold],
		ord.SentDate as [Last Ordered],
		ord.DateReceived as [Last Received],
		case sj.StoreJurisdictionDesc 
			when 'US' then CONVERT(VARCHAR, CAST(i.Package_Desc1 AS DECIMAL(20, 2))) 
			else (	case 
						when iod.Package_Desc1 is null then CONVERT(VARCHAR, CAST(i.Package_Desc1 AS DECIMAL(20, 2))) 
						else CONVERT(VARCHAR, CAST(iod.Package_Desc1 AS DECIMAL(20, 2))) 
					end) 
		end + '/' + case sj.StoreJurisdictionDesc 
						when 'US' then CONVERT(VARCHAR, CAST(i.Package_Desc2 AS DECIMAL(20, 2))) 
						else (	case 
									when iod.Package_Desc2 is null 
									then CONVERT(VARCHAR, CAST(i.Package_Desc2 AS DECIMAL(20, 2))) 
									else CONVERT(VARCHAR, CAST(iod.Package_Desc2 AS DECIMAL(20, 2))) 
								end) 
					end as PackSize,
		case sj.StoreJurisdictionDesc 
			when 'US' then CAST(iu.Unit_Name AS VARCHAR(10))
			else (	case 
						when apu.Unit_Name is null then CAST(iu.Unit_Name AS VARCHAR(10)) 
						else CAST(apu.Unit_Name AS VARCHAR(10)) 
					end)
		end as UOM,
		isnull(pbh.BatchDescription, 'Ad hoc') as BatchName,
		pbh.StartDate as BatchStartDate,
		lt.LabelTypeDesc as [Label Type],
		case when pbd.InsertApplication = 'Sale Off' or pbd.CancelAllSales = 1 then 'Y' else 'N' end as [Sale Off]
	from 
		NoTagItemExclusion ntie
		join ItemIdentifier ii on ntie.Identifier = ii.Identifier
		join Item i on ii.Item_Key = i.Item_Key
		left join PriceBatchHeader pbh on ntie.PriceBatchHeaderId = pbh.PriceBatchHeaderID
		left join PriceBatchDetail pbd on pbh.PriceBatchHeaderId = pbd.PriceBatchHeaderID and i.Item_Key = pbd.Item_Key
		join ItemBrand ib on i.Brand_ID = ib.Brand_ID
		join SubTeam st on st.SubTeam_No = i.SubTeam_No
		join Store s on ntie.StoreNumber = s.Store_No
		left join 
		   (select 
				MAX(Date_Key) as Date_Key, 
				Item_Key, 
				Store_No
			from 
				Sales_SumByItem 
			group by 
				Item_Key, 
				store_no) sal on i.Item_Key = sal.Item_Key and ntie.StoreNumber = sal.Store_No
		
		left join 
		   (select 
				MAX(oi.DateReceived) as DateReceived, 
				MAX(oh.SentDate) as SentDate,
				oi.Item_Key,
				v.Store_No as Store_No
			from 
				OrderItem oi
				join OrderHeader oh on oi.OrderHeader_ID = oh.OrderHeader_ID 
				join Vendor v on oh.ReceiveLocation_ID = v.Vendor_ID
			group by 
				oi.Item_Key, 
				v.Store_No) ord on ord.Item_Key = i.Item_Key and ntie.StoreNumber = ord.Store_No
		
		inner join ItemUnit iu on i.Package_Unit_ID = iu.Unit_ID
		inner join StoreJurisdiction sj on s.StoreJurisdictionID = sj.StoreJurisdictionID
		left join ItemOverride iod on i.Item_Key = iod.Item_Key and sj.StoreJurisdictionID = iod.StoreJurisdictionID
		left join ItemUnit apu on iod.Package_Unit_ID = apu.Unit_ID
		left join LabelType lt on i.LabelType_ID = lt.LabelType_ID
	where 
		(@BatchDescription is null or pbh.BatchDescription like @BatchDescription) 
		and ntie.StoreNumber = ISNULL(@StoreNumber, ntie.StoreNumber)  
		and ((pbh.StartDate between @BeginStartDate and @EndStartDate) or (ntie.InsertDate between @BeginStartDate and @EndStartDate))
	order by
		ntie.InsertDate desc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_NoTag] TO [IRMAReportsRole]
    AS [dbo];

