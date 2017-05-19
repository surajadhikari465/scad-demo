if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorNamesbyRegion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetJobStatusList]
GO

CREATE PROCEDURE [dbo].[GetVendorNamesbyRegion] 
	@UserID as int
	, @DBString varchar(250) = null
	
	
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
        --  @DBString varchar(max),
          @Sql nvarchar(max)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		set @RegionID = (SELECT RegionID from users where userID = @UserID)
		
		IF object_id('tempdb..#TEMPMySelect') IS NOT NULL
	DROP TABLE #TEMPMyVendor
	
	create table #TEMPMyVendor
	(
		Vendor_ID int 
		,CompanyName varchar(50)
		
	)
	
	insert into #TEMPMyVendor(Vendor_ID,CompanyName) values (0,'--Select One--')


if @DBString is null
begin
	select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
	 from Regions where RegionID = @RegionID
	 
	
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
end


	
	
	set @sql = 'select Vendor_ID,
      CompanyName from ' + @DBString + '[Vendor]'
	
	
insert into #TEMPMyVendor exec sp_executesql @sql
	
select * from #TEMPMyVendor
	


END

