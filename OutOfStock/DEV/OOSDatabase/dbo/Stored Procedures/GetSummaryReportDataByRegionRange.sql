-- =============================================
-- Author:		Anjana	
-- Create date: 10/25/2011
-- Description:	Summary report data
-- =============================================
CREATE PROCEDURE [dbo].[GetSummaryReportDataByRegionRange] 
	@Region as char(3),
	@DateRange as int = 7
AS

DECLARE @times_scanned as int
DECLARE @Store_OOS_Count as int
declare @Store_UPC_Count as nvarchar
DECLARE @No_of_SKUs as int
declare @DBString varchar(max)
declare @sql nvarchar(max)


BEGIN
	
delete from SummaryData
		

insert into SummaryData(StoreID, StoreName)
Select s.ID, s.STORE_NAME from STORE s join REGION r on r.ID = s.REGION_ID 
									where r.REGION_ABBR = @Region

Update a
set a.StoreOOSCount =
(select COUNT(rd.ID)
				 from REPORT_DETAIL rd join REPORT_HEADER rh on rh.ID = rd.REPORT_HEADER_ID
				 
				 where rd.CREATED_DATE between (convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) and getdate()
				 	 and rh.STORE_ID = a.StoreID )
from SummaryData a	

Update a
set a.NoTimesScanned =
( select count(*) 
						from REPORT_HEADER rh1
						where (rh1.CREATED_DATE
						 between (convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) 
						 and getdate()) and rh1.STORE_ID = a.StoreID)
from SummaryData a	

Select * from SummaryData


END