if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamNamesbyRegion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetJobStatusList]
GO

Create PROCEDURE [dbo].[GetSubTeamNamesbyRegion] 
	@UserID as int
	, @DBString varchar(250) = null
	
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
      --    @DBString varchar(max),
          @Sql nvarchar(max)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		set @RegionID = (SELECT RegionID from users where userID = @UserID)
		
		IF object_id('tempdb..#TEMPMySubTeam') IS NOT NULL
	DROP TABLE #TEMPMySubTeam
	
	create table #TEMPMySubTeam
	(
		SubTeam_No int 
		,SubTeam_Name varchar(50)
		
	)
	
	insert into #TEMPMySubTeam(SubTeam_No,SubTeam_Name) values (0,'--Select One--')


if @DBString is null
begin

	select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
	 from Regions where RegionID = @RegionID
	 

		
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
	
end
	
	set @sql = 'select SubTeam_No,
      SubTeam_Name from ' + @DBString + '[SubTeam]'
	
	
insert into #TEMPMySubTeam exec sp_executesql @sql
	
select * from #TEMPMySubTeam
	
	


END

