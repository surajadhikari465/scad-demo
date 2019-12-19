-- =============================================
-- Author:		Anjana	
-- Create date: 10/25/2011
-- Description:	Summary report data
-- =============================================
CREATE PROCEDURE [dbo].[GetSummaryReportData] 
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
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
	
	
	
		IF object_id('tempdb..#MyStores') IS NOT NULL
	DROP TABLE #MyStores
	
	create table #MyStores
	(
		S_ID int identity (1,1) not null ,
		Store_ID int)
		
		
	--IF object_id('tempdb..SummaryData') IS NOT NULL
	--DROP TABLE SummaryData
	
	delete from SummaryData
	
	--create table SummaryData
	--(
	--	StoreName nvarchar(50) null 
	--	, StoreOOSCount int
	--	, StoreUPCCount int null
	--	, NoTimesScanned int, StoreID int
	--)
		
		
		
	insert into #MyStores(Store_ID) 
	select s.ID from STORE s join REGION r on r.ID = s.REGION_ID 
									where r.REGION_ABBR = @Region

	declare @record_counter int
	declare @loop_counter int
	set @loop_counter = isnull((select count(*) from #MyStores),0)
	set @record_counter = 1
	Declare @S_ID int
	DECLARE @Store_NAME as varchar(20)

	while @loop_counter > 0 AND @record_counter <= @loop_counter
		begin
			set @S_ID= (select Store_ID from #MyStores where S_ID = @record_counter)
			
			SET @Store_NAME = (select STORE_NAME from STORE where ID = @S_ID)
			
			set @Store_OOS_Count = (select COUNT(rd.ID)
				 from REPORT_DETAIL rd join REPORT_HEADER rh on rh.ID = rd.REPORT_HEADER_ID
				 
				 where rd.CREATED_DATE between (convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) and getdate()
				 	 and rh.STORE_ID = @S_ID)
			
					
					
					
			
					
				SET @times_scanned =( select count(*) 
						from REPORT_HEADER rh1
						where (rh1.CREATED_DATE
						 between (convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) 
						 and getdate()) and rh1.STORE_ID = @S_ID )		
						 
						 
				--SET @sql = N'SELECT * FROM OPENQUERY(VIMORACLE,
 --''SELECT count(UPC) from vim.vendor_store_item where PS_BU = '+ cast(@S_ID as nvarchar) +''')'
			  
		--set @Store_UPC_Count =  select (exec sp_executesql @sql)
			
--				exec sp_executesql @Store_UPC_Count = @sql
				
				
						   
						   
		insert into SummaryData(StoreName 
		, StoreOOSCount 
		, NoTimesScanned, StoreID ) values (@Store_NAME,@Store_OOS_Count,@times_scanned, @S_ID)
		
		
	
							
			SET @record_counter = @record_counter + 1
		end


--CREATE TABLE #DirResults (DirCount int, DirStore int)


--SET @sql = N'SELECT * FROM OPENQUERY(VIMORACLE,
-- ''SELECT count(UPC), PS_BU from vim.vendor_store_item group by PS_BU'')'
			  
--		--set @Store_UPC_Count =  select (exec sp_executesql @sql)
--			insert #DirResults 
--				exec sp_executesql @sql
				

--Update a
--Set StoreUPCCount = isnull((select DirCount from #DirResults Where DirStore = StoreID),0)
--from
--SummaryData a


	select * from SummaryData
							
	END
	--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
	--set @Store_OOS_Count = (select COUNT(*) from REPORT_DETAIL where CREATED_DATE between 
	
	--						(convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) and getdate()
	
	
	--select @sql = N'SELECT count(*)
	--						  FROM
	--									(SELECT vsi.ps_bu, s.store_abbr, vsi.upc
	--										FROM vim.vendor_store_item vsi,
	--												  vim.store s
	--									  WHERE vsi.ps_bu = s.ps_bu
	--										  AND vsi.region = @Region
	--								  GROUP BY vsi.ps_bu, s.store_abbr, vsi.upc)
	--					   GROUP BY ps_bu, store_abbr'


	
	--SET @times_scanned =( select count(*) 
	--					from REPORT_HEADER rh1
	--					where (rh1.CREATED_DATE
	--					 between (convert(datetime, convert (varchar(10), getdate(), 101)) - @DateRange) 
	--					 and getdate())) and rh1.STORE_ID = )


--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&








--	IF object_id('tempdb..#SummaryData') IS NOT NULL
--	DROP TABLE #SummaryData
	
--	create table #SummaryData
--	(
--		StoreName nvarchar(50) null 
--		, StoreOOSCount int
--		, StoreUPCCount decimal
--		, NoTimesScanned int
--		, AVG1 decimal
--		, AVG2 decimal
--		, AVG3 decimal
	
--	)


--   insert into #SummaryData(StoreName 
--		, StoreOOSCount 
--		, StoreUPCCount 
--		, NoTimesScanned)
   
--   select t. [StoreName]
--           ,t.[StoreOOSCount]
--           ,t.[StoreUPCCount]
--           ,t.[NoTimesScanned] 
         
   
--   from [TempSummary] t 
   
 
   
		
				   
--select * from #SummaryData

--SELECT count(vsi.upc)
--		FROM vim.vendor_store_item vsi,
--				  vim.store s
--	  WHERE vsi.ps_bu = s.ps_bu
--		  AND vsi.region = 4-- make it abbreaviation if thats what field is...
--		  AND s.ps_bu = 10062
--  GROUP BY vsi.upc)
  
--  select * from ITEM_MASTER
						   

 
 

