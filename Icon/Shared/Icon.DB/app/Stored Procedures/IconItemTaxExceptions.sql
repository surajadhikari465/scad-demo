create PROCEDURE [app].[IconItemTaxExceptions]
	@DbServerName varchar(255),
	@DbName varchar(255),
	@ReportName varchar(255)
As

Begin

--=======================================================
-- Declare Variables
--=======================================================
declare	@taxClassID int,
		@ValidationTraitID int,
	    @wholeFoodsLocale int,
		@productDescriptionTraitID int;

declare @SQLString nvarchar(MAX);
declare @SQLStringDB nvarchar(MAX);
declare @SQLStringItem nvarchar(MAX);
declare @SQLStringWhere nvarchar(MAX);
declare @SQLStringIdentifier nvarchar(MAX);
declare @SQLStringTaxClass nvarchar(MAX);

if(@ReportName <> 'Tax')
	begin
		return;
	end

declare @iconValidatedScanCodes table
(
	ScanCode nvarchar(13) PRIMARY KEY,
	ProductDescription nvarchar(255),
	taxCode nvarchar(7),
	taxClassName nvarchar(255)
)

declare @irmaItems table
(
	ScanCode nvarchar(13) PRIMARY KEY,
	Default_Identifier [bit] NOT NULL,
	taxCode nvarchar(7),
	taxClassDescription nvarchar(50) 
)

SET @DbServerName = '['+ @DbServerName + ']'
SET @DbName       = '[' + @DbName + ']'
set @SQLStringDB  = @DbServerName + '.' + @DbName;

-- Hierarchy
SET @taxClassID					= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'tax')
-- Locale
SET @wholeFoodsLocale			= (SELECT l.localeID FROM Locale l WHERE l.localeName = 'Whole Foods')
-- Traits
SET @ValidationTraitID			= (SELECT t.traitID from Trait t WHERE t.traitCode = 'VAL')
SET @productDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRD');

--=======================================================
-- Setup CTEs
--=======================================================
	WITH 
	ItemTrait_CTE (traitID, traitValue, itemID)
	AS
	(
		SELECT it.traitID, it.traitValue, it.itemID
		FROM ItemTrait it
		WHERE it.localeID = @wholeFoodsLocale
	)

insert into @iconValidatedScanCodes
select sc.scanCode, pd.traitValue as ProductDescription, left(taxhc.hierarchyClassName, 7) as taxCode, taxhc.hierarchyClassName
from 
	ScanCode sc
join 
	ItemTrait_CTE val on sc.itemID = val.itemID
		and val.traitID = @ValidationTraitID
join 
	ItemHierarchyClass itaxhc on sc.itemID = itaxhc.itemID
join 
	HierarchyClass	taxhc	on itaxhc.hierarchyClassID = taxhc.hierarchyClassID
		and taxhc.hierarchyID	= @taxClassID
LEFT JOIN ItemTrait_CTE	pd		on	sc.itemID = pd.itemID
									and pd.traitID = @productDescriptionTraitID
where val.traitValue is not null
order by sc.scanCode

--=============================================================
-- Get IRMA Items
--=============================================================

SET @SQLString = 'select ii.Identifier, ii.Default_Identifier, t.ExternalTaxGroupCode as taxCode, t.TaxClassDesc
	from ';
SET @SQLStringWhere = ' 
	where i.Retail_Sale = 1 
	and i.Deleted_Item = 0 
	and i.Remove_Item = 0
	and ii.Deleted_Identifier = 0 
	and ii.Remove_Identifier = 0
	order by ii.Identifier'
SET @SQLStringItem = @SQLStringDB + '.dbo.Item i 
	JOIN '
SET @SQLStringIdentifier = @SQLStringDB + '.dbo.ItemIdentifier	ii on	i.Item_Key = ii.Item_Key
	join '
SET @SQLStringTaxClass = @SQLStringDB + '.dbo.TaxClass t on	i.TaxClassID = t.TaxClassID '
	
SET @SQLString = @SQLString + @SQLStringItem +   @SQLStringIdentifier + @SQLStringTaxClass  + @SQLStringWhere

Insert into @irmaItems EXECUTE sp_executesql @SQLString

select	icon.ScanCode,
		icon.ProductDescription,
		irma.Default_Identifier as DefaultIdentifier,
		icon.taxCode as 'Icon TaxCode',
		icon.taxClassName as 'Icon TaxClass',
		irma.taxCode as 'IRMA TaxCode',
		irma.taxClassDescription as 'IRMA TaxClass'
from 
	@iconValidatedScanCodes icon
join
	@irmaItems irma on icon.ScanCode = irma.ScanCode
where isnull(icon.taxCode, '') <> isnull(irma.taxCode, '')
order by icon.ScanCode
End