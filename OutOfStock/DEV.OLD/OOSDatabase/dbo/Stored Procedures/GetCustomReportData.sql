-- =============================================
-- Author:		[dbo].[GetCustomReportData]
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomReportData]
	
	@StartDate as datetime = null,
	@endDate as datetime = null,
	
	@Stores as varchar(2000) = null,
	@team as varchar(2000) = null,
	@Subteam as varchar(2000)= null
	
AS



BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @query AS nVARCHAR(MAX)
DECLARE @date_Diff as int
DECLARE @date_Diff_range as int

set	 @date_Diff = DATEDIFF(day,@StartDate,GETDATE())
set @date_Diff_range = DATEDIFF(day,@StartDate, @endDate)


--select rd.ps_subteam, rd.upc, im.brand, im.LONG_DESCRIPTION, im.ITEM_SIZE, im.ITEM_UOM,
--rd.vendor_key, rd.VIN, rd.MOVEMENT, (rd.EFF_PRICE * rd.movement) sales, dbo.get_times_scanned(rd.upc, rh.STORE_ID, rh.CREATED_DATE, 42) times_scanned,
--rd.notes, 
--cost = case -- this is movement times unit cost 
--    when coalesce(rd.case_size, 0) = 0 then 0 -- don't divide by zero
--    else rd.movement * rd.EFF_COST / rd.case_size
--END,
--margin = case 
--    when coalesce(rd.eff_price, 0) * coalesce(rd.case_size, 0) = 0 then 0 -- don't divide by zero
--    else round(100 * ((rd.EFF_PRICE - rd.eff_cost / rd.case_size) / rd.eff_price), 2) 
--END,
--rd.eff_price, rd.EFF_PRICETYPE, im.CATEGORY_NAME, im.CLASS_NAME
--, 0 as Avg_daily_Units
--, 0 as Avg_Mov_Sales
--, 'sdf' as Product_Status

--from report_header rh, REPORT_DETAIL rd left join ITEM_MASTER im
--on rd.UPC = im.NAT_UPC
--where rh.id = rd.REPORT_HEADER_ID
--and rh.STORE_ID = 669
--and rh.CREATED_DATE between '06/01/2011' and '11/01/2011'

  select rd.ps_subteam, 
			rd.upc,
			 im.brand, 
			 im.LONG_DESCRIPTION, 
			 im.ITEM_SIZE,
			  im.ITEM_UOM,
			rd.vendor_key, 
			rd.VIN, 
			rd.MOVEMENT,
			 (rd.EFF_PRICE * rd.movement) sales,
			  dbo.get_times_scanned(rd.upc, rh.STORE_ID, rh.CREATED_DATE,@date_Diff) times_scanned,
			rd.notes, 
			cost = case -- this is movement times unit cost 
				when coalesce(rd.case_size, 0) = 0 then 0 
				else rd.movement * rd.EFF_COST / rd.case_size
			END,
			margin = case 
				when coalesce(rd.eff_price, 0) * coalesce(rd.case_size, 0) = 0 then 0 
				else round(100 * ((rd.EFF_PRICE - rd.eff_cost / rd.case_size) / rd.eff_price), 2) 
			END,
			rd.eff_price,
			 rd.EFF_PRICETYPE,
			  im.CATEGORY_NAME,
			   im.CLASS_NAME
			   ,((rd.MOVEMENT)/@date_Diff_range) as Avg_daily_Units
			   ,((rd.EFF_PRICE * rd.movement)/@date_Diff_range) as Avg_Mov_Sales
			   ,r.REASON_DESCRIPTION as Product_Status
			   
			   
			from report_header rh, REPORT_DETAIL rd 
			left join ITEM_MASTER im on rd.UPC = im.NAT_UPC
			inner join REASON r on r.ID = rd.REASON_ID
			
			where rh.id = rd.REPORT_HEADER_ID
			and rh.STORE_ID in (select * from DBO.fn_CSVToTable(@Stores))
			
			and rd.PS_TEAM in (select * from DBO.fn_CSVToTable(@team))
			and rd.PS_SUBTEAM in (select * from DBO.fn_CSVToTable(@Subteam))
			
			and rh.CREATED_DATE between @StartDate AND @endDate
		
			
			
			
			






END







