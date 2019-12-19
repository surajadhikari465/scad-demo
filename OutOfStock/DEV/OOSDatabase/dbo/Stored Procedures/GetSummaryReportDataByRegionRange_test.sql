-- =============================================
-- Author:		Anjana	
-- Create date: 10/25/2011
-- Description:	Summary report data
-- =============================================
CREATE PROCEDURE [dbo].[GetSummaryReportDataByRegionRange_test] 
	@Region as char(3),
	@DateRange as int = 7
AS

DECLARE @times_scanned as int
DECLARE @Store_OOS_Count as int
declare @Store_UPC_Count as nvarchar
DECLARE @No_of_SKUs as int
declare @DBString varchar(max)
declare @sql nvarchar(max)
declare @S_ID as int


BEGIN
	
delete from SummaryData
		

insert into SummaryData(StoreID, StoreName)
Select s.ID, s.STORE_NAME from STORE s join REGION r on r.ID = s.REGION_ID 
									where r.REGION_ABBR = @Region and  s.STORE_NAME  not like '%(closed)' 

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



--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
declare @record_counter int
	declare @loop_counter int
	set @loop_counter = isnull((select count(*) from SummaryData),0)
	set @record_counter = 1

while @loop_counter > 0 AND @record_counter <= @loop_counter
		begin
			set @S_ID= (select s.StoreID from SummaryData s where s.S_ID = @record_counter)
			SET @sql = N'SELECT * FROM OPENQUERY(VIMORACLE,
			 ''SELECT count(UPC) from vim.vendor_store_item where PS_BU = '+ cast(@S_ID as nvarchar) +''')'
						  
				--	set @Store_UPC_Count =  select (exec sp_executesql @sql)
						
							exec sp_executesql @Store_UPC_Count = @sql
							
		update SummaryData  set StoreUPCCount = isnull(@Store_UPC_Count,0) where StoreID = @S_ID 
		
		--select @Store_UPC_Count as dfgdfg
		
		SET @record_counter = @record_counter + 1				
			
		END

					

--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

Select * from SummaryData



END