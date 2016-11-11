declare @testServerName nvarchar(255) = 'CEWD1815\SQLSHARED2012D',
		@qaServerName nvarchar(255) = 'QA-SQLSHARED3\SQLSHARED3Q',
		@prdServerName nvarchar(255) = 'SQLSHARED3-PRD3\SHARED3P'
declare @currentServerName nvarchar(255) = (select @@SERVERNAME)

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
		('IDD-FL','ItemCatalog_Test'),
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
		('IDQ-FL\FLQ','ItemCatalog'),
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
		('IDP-FL\FLP','ItemCatalog'),
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


if OBJECT_ID('tempdb..#irmaItemSubscriptions', N'U') is not null
begin
	drop table #irmaItemSubscriptions
end

create table #irmaItemSubscriptions 
(
	regioncode varchar(2),
	identifier varchar(13)
)

insert into #irmaItemSubscriptions(regioncode, identifier)
select 
	iis.regioncode
	,iis.identifier
from app.IRMAItemSubscription iis
where iis.deleteDate is null

declare regionCursor cursor
	for select * from @irmaInstances

declare @irmaServer nvarchar(255),
		@irmaDb nvarchar(255)

open regionCursor
fetch next from regionCursor
into @irmaServer, @irmaDb

while @@FETCH_STATUS = 0
begin
	--delete from ValidatedScanCode (vsc) if the scan code is not used for any non-deleted items
	-- or if the region is not subscribed for the code in ICON
	declare @insertSql nvarchar(max) = N'
				declare @regionCode nvarchar(2) = (select RegionCode from [' + @irmaServer + '].[' + @irmaDb + '].dbo.Region)
				
				delete vsc
				from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedScanCode vsc 
				where vsc.ScanCode not in
				(
					select ii.Identifier 
					from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ItemIdentifier ii
					join [' + @irmaServer + '].[' + @irmaDb + '].dbo.Item i on ii.Item_Key = i.Item_Key
					where ii.Deleted_Identifier = 0
						and i.Deleted_Item = 0
				)
				
				delete vsc
				from [' + @irmaServer + '].[' + @irmaDb + '].dbo.ValidatedScanCode vsc 
				where not exists 
				(
					select 1 from #irmaItemSubscriptions where regioncode=@regionCode and identifier = vsc.ScanCode
				)'
	print @insertSql

	execute sp_executesql @insertSql

	fetch next from regionCursor
	into @irmaServer, @irmaDb
end

close regionCursor
deallocate regionCursor

if OBJECT_ID('tempdb..#irmaItemSubscriptions') is not null
	drop table #irmaItemSubscriptions