  declare @RegionTbl as table (RegionCode varchar(2))
  declare @BusinessUnitIdTbl as table (BusinessUnitID int)
  
  insert into @RegionTbl
  values ('FL'),('MA'),('MW'),('NA'),('NC'),('NE'),('PN'),('RM'),('SO'),('SP'),('SW'),('UK')

  insert into @BusinessUnitIdTbl
  select BusinessUnitID from Locales_TS

  if OBJECT_ID('tempdb..#DeteleCommands') IS NOT NULL
  begin
		drop table #sqlCommands
  end

  CREATE TABLE #sqlCommands(Command NVARCHAR(max));

	INSERT INTO
		#sqlCommands
	SELECT
		'begin

		    select l.*
			from dbo.Locales_' + r.RegionCode + ' l
			join @BusinessUnitIdTbl b on l.BusinessUnitID = b.BusinessUnitID   ' + 
			 
			'delete l
			from dbo.Locales_' + r.RegionCode + ' l
			join @BusinessUnitIdTbl b on l.BusinessUnitID = b.BusinessUnitID
		 end
		'
	from @RegionTbl r

   	DECLARE @Command nvarchar(max)

	DECLARE CommandsCursor CURSOR
		FOR SELECT * FROM #sqlCommands
	OPEN CommandsCursor

	FETCH NEXT FROM CommandsCursor INTO @Command

	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT ''
		PRINT 'Executing - ' + @Command
		EXECUTE sp_executesql @Command
		FETCH NEXT FROM CommandsCursor INTO @Command
	END

	CLOSE CommandsCursor
	DEALLOCATE CommandsCursor

	DROP TABLE #sqlCommands  