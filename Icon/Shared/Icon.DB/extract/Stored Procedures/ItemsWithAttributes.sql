CREATE PROCEDURE [extract].[ItemsWithAttributes]
AS
BEGIN
	DECLARE @cols AS NVARCHAR(MAX),
    @colsWithIsNull AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@BrandAbbreviationTraitCodeid int;

		
 
	select @BrandAbbreviationTraitCodeid = traitid from Trait t where t.traitCode = 'BA'

	select itemid into #tempItemHierarchies from item	
		
	alter table #tempItemHierarchies
	add BrandHierarchyClassId int, BrandHierarchyName varchar(255), BrandAbbreviation varchar(10),
	TaxHierarchyClassId int, TaxHierarchyName varchar(255), 
	MerchHierarchyClassId int, MerchHierarchyName varchar(255), 
	FinancialHierarchyClassId int, FinancialHierarchyName varchar(255), 
	NationalHierarchyClassId int, NationalHierarchyName varchar(255),
	ManufacturerHierarchyClassId int null, ManufacturerHierarchyName varchar(255) null
		
	create clustered index ix_tempItemHierarchies_itemid on #tempItemHierarchies (itemid)

	update tih
	set tih.BrandHierarchyClassId = ihc.hierarchyClassID, 
	tih.BrandHierarchyName = bhv.HierarchyClassName, 
	BrandAbbreviation = hct.traitValue
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join BrandHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId
	inner join HierarchyClassTrait hct on ihc.hierarchyClassID = hct.hierarchyClassID 
											and traitid = @BrandAbbreviationTraitCodeid
			
			
	update tih
	set tih.TaxHierarchyClassId = ihc.hierarchyClassID, 
	tih.TaxHierarchyName = bhv.HierarchyClassName
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join TaxHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId


	update tih
	set tih.MerchHierarchyClassId = ihc.hierarchyClassID, 
	tih.MerchHierarchyName = bhv.HierarchyClassName
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join MerchandiseHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId

	update tih
	set tih.FinancialHierarchyClassId = ihc.hierarchyClassID, 
	tih.FinancialHierarchyName = bhv.HierarchyClassName
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join FinancialHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId

	update tih
	set tih.ManufacturerHierarchyClassId = ihc.hierarchyClassID, 
	tih.ManufacturerHierarchyName = bhv.HierarchyClassName
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join ManufacturerHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId

	update tih
	set tih.NationalHierarchyClassId = ihc.hierarchyClassID, 
	tih.NationalHierarchyName = bhv.HierarchyClassName
	from #tempItemHierarchies tih 
	inner join ItemHierarchyClass ihc on tih.ItemId = ihc.ItemId
	inner join NationalClassHierarchyView bhv on ihc.hierarchyClassID = bhv.HierarchyClassId

			
			
	SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(a.[AttributeName]) 
				from Attributes   a
				where a.attributegroupid = 1
				order by ',' + QUOTENAME(a.[AttributeName])
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')
        

	SET @colsWithIsNull = STUFF((SELECT distinct ', isnull(' + QUOTENAME(a.[AttributeName])  + ','''') as ' + QUOTENAME(a.[AttributeName])
				from Attributes   a
				where a.attributegroupid = 1
				order by ', isnull(' + QUOTENAME(a.[AttributeName])  + ','''') as ' + QUOTENAME(a.[AttributeName])
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')        
	set @query = 'SELECT  ItemId,
	scancode,
	ItemTypeId, 
	ItemTypeCode,
	BrandHierarchyClassId, 
	BrandHierarchyName,  
	BrandAbbreviation,	
	TaxHierarchyClassId , 
	TaxHierarchyName, 
	MerchHierarchyClassId , 
	MerchHierarchyName , 
	FinancialHierarchyClassId , 
	FinancialHierarchyName, 
	NationalHierarchyClassId , 
	NationalHierarchyName,  
	isnull(cast(ManufacturerHierarchyClassId as varchar(20)), '''') ManufacturerHierarchyClassId, 
	isnull(ManufacturerHierarchyName, '''') ManufacturerHierarchyName,   
	' + @colsWithIsNull + '
	from 
		(
			select i.ItemId,
			s.scancode,
			it.ItemTypeId,
			it.ItemTypeCode, 
			tih.BrandHierarchyClassId,
			tih.BrandHierarchyName,
			tih.BrandAbbreviation,
			tih.TaxHierarchyClassId,
			tih.TaxHierarchyName,
			tih.MerchHierarchyClassId,
			tih.MerchHierarchyName,
			tih.FinancialHierarchyClassId, 
			tih.FinancialHierarchyName, 
			tih.NationalHierarchyClassId, 
			tih.NationalHierarchyName, 
			tih.ManufacturerHierarchyClassId, 
			tih.ManufacturerHierarchyName,
			[Key] as Attribute, [Value]
			from dbo.Item i 
			CROSS APPLY OPENJSON(i.ItemAttributesJson)
			inner join dbo.ItemType it on i.itemtypeid = it.itemtypeid 
			inner join Scancode s on i.itemid=s.itemid
			inner join #tempItemHierarchies tih on i.itemid = tih.itemid
		) x
		pivot 
		(
			max([Value])
			for Attribute in (' + @cols + ')
		) p '
	
	execute(@query)

	drop table #tempItemHierarchies
END
GO
