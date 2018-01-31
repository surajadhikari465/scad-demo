declare @scriptKey varchar(128)

set @scriptKey = 'Align IRMA Brands with Icon Brands'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN	
	declare @testServerName nvarchar(255) = 'CEWD1815\SQLSHARED2012D',
			@qaServerName nvarchar(255) = 'QA-SQLSHARED3\SQLSHARED3Q',
			@prdServerName nvarchar(255) = 'SQLSHARED3-PRD3\SHARED3P',
			@currentServerName nvarchar(255) = (select @@SERVERNAME)

	---------------------------------
	--Declare IRMA instances to update with Icon Brands
	---------------------------------
	declare @irmaInstances table
	(
		IrmaServer nvarchar(255),
		IrmaDb nvarchar(255)

	)

	if(@currentServerName = @testServerName)
	begin
		use [iCon]
	
		insert into @irmaInstances
		values 
			('IDD-FL', 'ItemCatalog_Test'),
			('IDD-MA','ItemCatalog_Test'),
			('IDD-MW','ItemCatalog_Test'),
			('IDD-NA','ItemCatalog_Test'),
			('IDD-RM','ItemCatalog_Test'),
			('IDD-SO','ItemCatalog_Test'),
			('IDT-NC','ItemCatalog_Test'),
			('IDT-NE','ItemCatalog_Test'),
			('IDT-PN','ItemCatalog_Test'),
			('IDT-SP','ItemCatalog_Test'),
			('IDT-SW','ItemCatalog_Test')
	end

	if(@currentServerName = @qaServerName)
	begin
		use [iCon]
	
		insert into @irmaInstances
		values 
			('IDQ-FL\FLQ', 'ItemCatalog'),
			('IDQ-MA\MAQ','ItemCatalog'),
			('IDQ-MW\MWQ','ItemCatalog'),
			('IDQ-NA\NAQ','ItemCatalog'),
			('IDQ-RM\RMQ','ItemCatalog'),
			('IDQ-SO\SOQ','ItemCatalog'),
			('IDQ-NC\NCQ','ItemCatalog'),
			('IDQ-NE\NEQ','ItemCatalog'),
			('IDQ-PN\PNQ','ItemCatalog'),
			('IDQ-SP\SPQ','ItemCatalog'),
			('IDQ-SW\SWQ','ItemCatalog')
	end

	if(@currentServerName = @prdServerName)
	begin
		use [iCon]
	
		insert into @irmaInstances
		values 
			('IDP-FL\FLP', 'ItemCatalog'),
			('IDP-MA\MAP','ItemCatalog'),
			('IDP-MW\MWP','ItemCatalog'),
			('IDP-NA\NAP','ItemCatalog'),
			('IDP-RM\RMP','ItemCatalog'),
			('IDP-SO\SOP','ItemCatalog'),
			('IDP-NC\NCP','ItemCatalog'),
			('IDP-NE\NEP','ItemCatalog'),
			('IDP-PN\PNP','ItemCatalog'),
			('IDP-SP\SPP','ItemCatalog'),
			('IDP-SW\SWP','ItemCatalog')
	end


	if OBJECT_ID('tempdb..#iconBrands', N'U') is not null
	begin
		drop table #iconBrands
	end

	create table #iconBrands 
	(
		IconBrandId int primary key,
		BrandName nvarchar(255)
	)

	insert into #iconBrands(IconBrandId, BrandName)
	select 
		hc.hierarchyClassID, 
		LEFT(hc.hierarchyClassName, 25)
	from HierarchyClass hc
	join Hierarchy h on hc.hierarchyID = h.hierarchyID
	where h.hierarchyName = 'Brands'

	select * from #iconBrands

	------------------------------------------
	----Insert brands into each IRMA instance
	------------------------------------------
	declare regionCursor cursor
		for select * from @irmaInstances

	declare @irmaServer nvarchar(255),
			@irmaDb nvarchar(255)

	open regionCursor
	fetch next from regionCursor
	into @irmaServer, @irmaDb

	while @@FETCH_STATUS = 0
	begin
	
		declare @insertSql nvarchar(max) = N'
			-- Inserting Icon Brands into Irma that dont exist.
			insert into [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemBrand(Brand_Name, User_ID, LastUpdateTimestamp)
			select 
				BrandName,
				null,
				GETDATE()
			from #iconBrands tempBrands
			where not exists 
				(
					select * 
					from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemBrand ib 
					where ib.Brand_Name = tempBrands.BrandName
				)
		
			-- Inserting ValidatedBrand records into Irma
			insert into [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedBrand(IrmaBrandId, IconBrandId)
			select 
				ib.Brand_ID,
				tempBrands.IconBrandId
			from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemBrand ib
			join #iconBrands tempBrands on ib.Brand_Name = tempBrands.BrandName
			where not exists 
				(
					select * 
					from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedBrand vb 
					where vb.IconBrandId = tempBrands.IconBrandId
				)

			-- Associate Icon Brands to Irma Brands
			update vb
			set IrmaBrandId = ib.Brand_ID
			from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedBrand vb
			join #iconBrands tempBrands on vb.IconBrandId = tempBrands.IconBrandId
			join [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemBrand ib on tempBrands.BrandName = ib.Brand_Name
			where vb.IrmaBrandId <> ib.Brand_ID
		
			-- Invalidating Irma Brands that dont exist in Icon
			delete from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedBrand
			where not exists
				(
					select *
					from #iconBrands tempBrands
					where tempBrands.IconBrandId = IconBrandId
				)

			-- Deleting Irma brands that arent validated and not used.
			delete from ib 
			from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemBrand ib
			left join [' + @irmaServer + '].[' + @irmaDb + '].dbo.Item i on ib.Brand_ID = i.Brand_ID
			left join [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedBrand vb on ib.Brand_ID = vb.IrmaBrandId
			where i.Item_Key is null and vb.Id is null'

			print @insertSql

			execute sp_executesql @insertSql

		fetch next from regionCursor
		into @irmaServer, @irmaDb
	end

	close regionCursor
	deallocate regionCursor

	if OBJECT_ID('tempdb..#iconBrands') is not null
		drop table #iconBrands
	
	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO