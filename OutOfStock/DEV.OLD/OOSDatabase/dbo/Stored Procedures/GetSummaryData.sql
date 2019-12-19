
-- =============================================
-- Author:		Anjana	
-- Create date: 10/25/2011
-- Description:	Summary report data
-- =============================================
CREATE PROCEDURE [dbo].[GetSummaryData] 
	
AS


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	select * from TempSummary
	
--	IF object_id('tempdb..#SummaryData') IS NOT NULL
	--DROP TABLE #SummaryData
	
	--create table #SummaryData
	--(
	--	StoreName nvarchar(50) null 
	--	, StoreOOSCount int
	--	, StoreOOSPercentage decimal
	--	, NoTimesScanned int
	--	, AVG1 decimal
	--	, AVG2 decimal
	--	, AVG3 decimal
	
	--)


 --  insert into #SummaryData(StoreName 
	--	, StoreOOSCount 
	--	, StoreOOSPercentage 
	--	, NoTimesScanned)
   
 --  select t. [StoreName]
 --          ,t.[StoreOOSCount]
 --          ,t.[StoreOOSPercentage]
 --          ,t.[NoTimesScanned] 
         
   
 --  from [TempSummary] t 
   
  -- --group by t.storename, t.[StoreOOSCount]
  -- --        ,t.[StoreOOSPercentage]
  -- --        ,t.[NoTimesScanned] 
           
           
  ----       insert into #SummaryData(AVG1 
		----, AVG2 
		----, AVG3)
		--insert into #SummaryData(StoreName 
		--, StoreOOSCount 
		--, StoreOOSPercentage 
		--, NoTimesScanned)
		--select 'Averages:' as StoreName,(SUM([StoreOOSCount]) / 10) as AVG1
  --         ,(SUM([StoreOOSCount]) / 200) as AVG2
  --         , (SUM([NoTimesScanned]) / 10) as AVG3
  --           from [TempSummary] t 
   
		
				   
select * from #SummaryData

 
 
END

