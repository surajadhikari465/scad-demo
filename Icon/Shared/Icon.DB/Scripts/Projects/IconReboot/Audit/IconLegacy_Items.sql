set transaction isolation level read uncommitted

declare @selectSql nvarchar(max) = '
	SELECT i.itemId,
	sc.scanCode,
	brands.hierarchyClassID AS BrandHierarchyClassID,
	brands.hierarchyClassName AS BrandHierarchyClassName,
	merchandise.hierarchyClassID AS MerchandiseHierarchyClassID,
	merchandise.hierarchyClassName AS MerchandiseHierarchyClassName,
	tax.hierarchyClassID AS TaxHierarchyClassID,
	tax.hierarchyClassName AS TaxHierarchyClassName,
	nat.hierarchyClassID AS NationalHierarchyClassID,
	nat.hierarchyClassName AS NationalHierarchyClassName,
	financial.hierarchyClassID AS FinancialHierarchyClassID,
	financial.hierarchyClassName AS FinancialHierarchyClassName,
	isa.AnimalWelfareRating,
	isa.Biodynamic,
	isa.MilkType,
	isa.CheeseRaw,
	isa.EcoScaleRating,
	isa.GlutenFreeAgencyName,
	isa.KosherAgencyName,
	isa.Msc,
	isa.NonGmoAgencyName,
	isa.OrganicAgencyName,
	isa.PremiumBodyCare,
	isa.FreshOrFrozen,
	isa.SeafoodCatchType,
	isa.VeganAgencyName,
	isa.Vegetarian,
	isa.WholeTrade,
	isa.GrassFed,
	isa.PastureRaised,
	isa.FreeRange,
	isa.DryAged,
	isa.AirChilled,
	isa.MadeInHouse,
	isa.CustomerFriendlyDescription, '
declare @fromSql nvarchar(max) = ' 
	FROM item i
JOIN ScanCode sc ON i.itemID = sc.itemID
JOIN (
	SELECT ihc.itemID
		,hc.hierarchyClassID
		,hc.hierarchyClassName
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
	JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
	WHERE h.hierarchyName = ''Brands''
	) brands ON i.itemID = brands.itemID
JOIN (
	SELECT ihc.itemID
		,hc.hierarchyClassID
		,hc.hierarchyClassName
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
	JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
	WHERE h.hierarchyName = ''Merchandise''
	) merchandise ON i.itemID = merchandise.itemID
JOIN (
	SELECT ihc.itemID
		,hc.hierarchyClassID
		,hc.hierarchyClassName
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
	JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
	WHERE h.hierarchyName = ''Tax''
	) tax ON i.itemID = tax.itemID
JOIN (
	SELECT ihc.itemID
		,hc.hierarchyClassID
		,hc.hierarchyClassName
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
	JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
	WHERE h.hierarchyName = ''National''
	) nat ON i.itemID = nat.itemID
JOIN (
	SELECT ihc.itemID
		,hc.hierarchyClassID
		,hc.hierarchyClassName
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
	JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
	WHERE h.hierarchyName = ''Financial''
	) financial ON i.itemID = financial.itemID
	LEFT JOIN ItemSignAttribute isa ON i.ItemID = isa.ItemId'
	
select @selectSql += (' ' + LOWER(t.traitCode) + '.traitValue AS [' + t.traitDesc + '],'),
	@fromSql += (' left join dbo.ItemTrait ' + LOWER(t.traitCode) + ' on i.itemID = ' + LOWER(t.traitCode) + '.itemID and ' + LOWER(t.traitCode) + '.traitID = ' + cast(t.traitID as nvarchar(10)) + ' ')
from Trait t
where exists (
		select top 1 1 from ItemTrait it
		where it.traitID = t.traitID
	)

declare @sql nvarchar(max) = SUBSTRING(@selectSql, 1, LEN(@selectSql) - 1) + @fromSql

select @sql
--exec sp_executesql  @sql