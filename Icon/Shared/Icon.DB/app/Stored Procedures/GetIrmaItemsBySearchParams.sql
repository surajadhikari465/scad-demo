
create PROCEDURE [app].[GetIrmaItemsBySearchParams]
@identifier varchar(15) = null,
@itemDescription varchar(20)= null,
@brandName varchar(30) = null,
@regionCode varchar(5) = null,
@parttialScanCodeSearch bit = 0,
@taxRomanceName varchar(255) = null 

As

BEGIN
declare @taxHierarchyID int;
declare @undefinedTax bit = 0;
declare @taxRomanceTraitID int;

if @identifier = ''
	set @identifier = null
if @regionCode = ''
	set @regionCode = null

if @brandName = ''
	set @brandName  = null
if @itemDescription = ''
	set @itemDescription = null


if @identifier is not null
begin
	if @parttialScanCodeSearch = 1
		set @identifier = '%' + LOWER(@identifier) + '%'
	else
	begin
		set @identifier = LOWER(@identifier) + '%'
		end
end

if @regionCode is not null
	set @regionCode = Lower(@regionCode)
if @itemDescription is not null
	set @itemDescription  = LOWER(@itemDescription)

if @brandName is not null  
	set @brandName  = LOWER(@brandName)	

if @taxRomanceName is not null
begin
	set @taxRomanceName = LOWER(@taxRomanceName)
	if(@taxRomanceName = 'undefined')
		set @undefinedTax = 1;
	else
		set @taxRomanceName = '%'+ @taxRomanceName + '%'
end

SET @taxHierarchyID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax');
SET @taxRomanceTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'TRM');
with
Tax_CTE (hierarchyClassID, hierarchyClassName, hierarchyId, taxRomance)
AS
(
	SELECT hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue as taxRomance
	FROM		
		HierarchyClass hc
		JOIN HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @taxRomanceTraitID
	WHERE
		hc.hierarchyID = @taxHierarchyID
)

select i.* from app.IRMAItem i
LEFT JOIN Tax_CTE	tax		on	i.taxClassID = tax.hierarchyClassID

where  ((i.identifier LIKE @identifier)
             OR @identifier is null)

and ((( CAST(CHARINDEX(@itemDescription , LOWER(i.itemDescription )) AS int)) > 0)
            OR @itemDescription  is null)

and ((LOWER(i.regioncode) = @regionCode)
             OR @regionCode is null)
and ((( CAST(CHARINDEX(@brandName, LOWER(i.brandName )) AS int)) > 0)
            OR @brandName is null)
and ((Lower(tax.taxRomance) like @taxRomanceName and @undefinedTax = 0 )
		OR (i.taxClassID is not null and tax.hierarchyClassID is null and @undefinedTax = 1)
        OR @taxRomanceName is null)

Order By i.insertDate, i.regioncode

end;