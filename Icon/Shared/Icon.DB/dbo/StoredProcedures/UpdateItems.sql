CREATE PROCEDURE [dbo].[UpdateItems]
	@Items dbo.UpdateItemsType readonly
AS
BEGIN
	DECLARE @merchandiseHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Merchandise'),
            @brandsHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Brands'),
            @taxHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Tax'),
            @financialHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Financial'),
            @nationalHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'National'),
		    @manufacturerHierarchyId INT = (SELECT hierarchyId FROM Hierarchy WHERE hierarchyName = 'Manufacturer')
	
	create table #HierarchyClassInfo ( ItemId int, HierarchyClassId int, HierarchyId int)
	create table #ManufacturerHierarchyClassInfo ( ItemId int, HierarchyClassId int, HierarchyId int, ExistingValue int)

	SELECT
		ItemId, 
		BrandsHierarchyClassId ,
		FinancialHierarchyClassId ,
		MerchandiseHierarchyClassId ,
		NationalHierarchyClassId,
		TaxHierarchyClassId,
		ManufacturerHierarchyClassId,
		ItemAttributesJson,
		ItemTypeId
	INTO #items 
	FROM  @Items i

	insert into #HierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @brandsHierarchyId, i.BrandsHierarchyClassId from #items i
	where i.BrandsHierarchyClassId is not null

	insert into #HierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @merchandiseHierarchyId, i.MerchandiseHierarchyClassId from #items i
	where i.MerchandiseHierarchyClassId is not null

	insert into #HierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @taxHierarchyId, i.TaxHierarchyClassId from #items i
	where i.TaxHierarchyClassId is not null

	insert into #HierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @financialHierarchyId, i.FinancialHierarchyClassId from #items i
	where i.FinancialHierarchyClassId is not null

	insert into #HierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @nationalHierarchyId, i.NationalHierarchyClassId from #items i
	where i.NationalHierarchyClassId is not null

	insert into #ManufacturerHierarchyClassInfo (ItemId, HierarchyId, HierarchyClassId)
	select Itemid, @manufacturerHierarchyId, i.manufacturerHierarchyClassId 
	from #items i
	where i.manufacturerHierarchyClassId is not null

	-- update all Hierarchy references that were passed in. Excluding Manufacturer
	update ihc 
		set hierarchyClassID = temp.HierarchyClassId
	FROM ItemHierarchyClass ihc
	JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
	JOIN Hierarchy h ON hc.hierarchyId = h.hierarchyID
	JOIN #HierarchyClassInfo temp ON h.hierarchyId = temp.hierarchyId and ihc.ItemId = temp.ItemId
	WHERE ihc.itemId = temp.ItemId
	AND ihc.hierarchyClassId <> temp.HierarchyClassId
	and temp.HierarchyId <> @manufacturerHierarchyId -- exclude manufacturer. handled below.

	-- delete manufacturer reference from IHC if passed into ManufacturerId = 0
	delete ihc 
	from ItemHierarchyClass ihc
	inner join HierarchyClass hc  on ihc.hierarchyclassid =  hc.hierarchyClassID
	inner join Hierarchy h on hc.hierarchyID= h.hierarchyID
	inner join #ManufacturerHierarchyClassInfo temp on ihc.itemID = temp.ItemId 
	where temp.HierarchyClassId = 0 
	and temp.HierarchyId = @manufacturerHierarchyId 
	and hc.hierarchyID = @manufacturerHierarchyId

		--get existing values for items that need manuf. updates
	update  mhci
		set ExistingValue = ihc.hierarchyClassID		
	from ItemHierarchyClass ihc 
	inner join HierarchyClass hc on ihc.hierarchyclassid = hc.hierarchyclassid
	inner join #ManufacturerHierarchyClassInfo mhci on ihc.itemID = mhci.ItemId
	where hc.hierarchyid = @manufacturerHierarchyId and ihc.itemID = mhci.ItemId

		--update itemhierarchyclass for the ones that have values already, but are different.
	update ihc
	set hierarchyClassID = mhci.HierarchyClassId
	from ItemHierarchyClass ihc 
	inner join HierarchyClass hc on ihc.hierarchyclassid = hc.hierarchyclassid
	inner join #ManufacturerHierarchyClassInfo mhci on ihc.itemID = mhci.ItemId
	where hc.hierarchyid = @manufacturerHierarchyId
		and mhci.hierarchyClassID <> mhci.ExistingValue

		--create manufacturer refrences for items that dont already have existing values.
	insert into ItemHierarchyClass (itemID, hierarchyClassID, localeID)
	select mhci.ItemId, mhci.HierarchyClassId, 1 
	from #ManufacturerHierarchyClassInfo mhci 
	where mhci.ExistingValue is null and  mhci.HierarchyClassId <> 0

		--update item attributes
	update i
	set ItemAttributesJson = temp.ItemAttributesJson
	from Item i inner join #items temp on i.ItemId = temp.itemid
	where temp.ItemAttributesJson is not null

	--update ItemTypeId
	update i
	set itemTypeId = temp.ItemTypeId
	from Item i inner join #items temp on i.ItemId = temp.ItemId
	where temp.ItemTypeId is not null

	drop table #HierarchyClassInfo
	drop table #ManufacturerHierarchyClassInfo
END
