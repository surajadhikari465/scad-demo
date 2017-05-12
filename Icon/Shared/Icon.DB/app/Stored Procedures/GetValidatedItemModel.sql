CREATE PROCEDURE [app].[GetValidatedItemModel]
       @ScanCodes app.ScanCodeListType READONLY
AS
BEGIN
       SET NOCOUNT ON;
              
       --=======================================================
       -- Declare Variables
       --=======================================================
       declare @productDescription int;
       declare @posDescription int;
       declare @packageUnit int;
       declare @foodStamp int;
       declare @tare int;
       declare @brandId int;
       declare @taxId int;
       declare @validationDate int;
       declare @taxAbbrevation int;
       declare @posDept int;
       declare @finId int;
       declare @merchFin int;
       declare @merchId int;
       declare @nonAlignedSubTeam int;
	   declare @natId int;
	   declare @natClassCodeTrait int;
	   declare @retailSize int;
	   declare @retailUom int;
              
       -- traits
       set @productDescription = (select traitID from Trait where traitDesc = 'Product Description');
       set @posDescription = (select traitID from Trait where traitDesc = 'POS Description');
       set @packageUnit = (select traitID from Trait where traitDesc = 'Package Unit');
       set @foodStamp = (select traitID from Trait where traitDesc = 'Food Stamp Eligible');
       set @tare = (select traitID from Trait where traitDesc = 'POS Scale Tare');
       set @validationDate = (select traitID from Trait where traitDesc = 'Validation Date');
       SET @taxAbbrevation = (select traitID from Trait where traitDesc = 'Tax Abbreviation');
       set @posDept = (select traitID from Trait where traitCode = 'PDN');
       set @merchFin = (select traitID from Trait where traitCode = 'MFM');
	   set @nonAlignedSubTeam = (select traitID from Trait where traitCode = 'NAS');
	   set @natClassCodeTrait = (select traitID from Trait where traitCode = 'NCC');
	   set @retailSize = (select traitID from Trait where traitDesc = 'Retail Size');
	   set @retailUom = (select traitID from Trait where traitDesc = 'Retail UOM');

       -- hierarchies
       set @brandId = (select hierarchyID from Hierarchy where hierarchyName = 'Brands');
       set @taxId = (select hierarchyID from Hierarchy where hierarchyName = 'Tax');
       set @finId = (select hierarchyID from Hierarchy where hierarchyName = 'Financial');
       set @merchId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise');
	   set @natId = (select hierarchyID from Hierarchy where hierarchyName = 'National');

       --=======================================================
       -- Setup CTEs
       --=======================================================
       WITH 
       ItemTrait_CTE (traitID, traitValue, itemID)
       AS
       (
              SELECT it.traitID, it.traitValue, it.itemID
              FROM ItemTrait it
              WHERE it.localeID = 1
       ),
       Brand_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
       AS
       (
              SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
              FROM 
                     ItemHierarchyClass   ihc
                     JOIN HierarchyClass hc     on ihc.hierarchyClassID = hc.hierarchyClassID
              WHERE
                     hc.hierarchyID = @brandId
       ),
       Tax_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, taxAbbreviation)
       AS
       (
              SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
              FROM 
                     ItemHierarchyClass                ihc
                     JOIN HierarchyClass               hc     on     ihc.hierarchyClassID = hc.hierarchyClassID
                     JOIN HierarchyClassTrait   hct on hc.hierarchyClassID = hct.hierarchyClassID
                                                                           and hct.traitID = @taxAbbrevation
              WHERE
                     hc.hierarchyID = @taxId
       ),
       FinSubTeam_CTE (itemID, subTeamNo, subTeamName, finHierarchyID, posDeptNo, disableSubTeamEvents)
       AS
       (
              SELECT ihc.itemID, finhc.hierarchyClassID as subTeamNo, finhc.hierarchyClassName as subTeamName, finhc.hierarchyID as finHierarchyID, finPos.traitValue as posDeptNo, finevent.traitValue as disableSubTeamEvents
              FROM 
                     ItemHierarchyClass                ihc
                     JOIN HierarchyClass               hc     on     ihc.hierarchyClassID = hc.hierarchyClassID and hc.hierarchyID = @merchId
                     JOIN HierarchyClassTrait   hct on hc.hierarchyClassID = hct.hierarchyClassID
                                                                           and hct.traitID = @merchFin
                     JOIN HierarchyClass finhc on hct.traitValue = finhc.hierarchyClassName 
                     LEFT JOIN HierarchyClassTrait     finPos on     finhc.hierarchyClassID = finPos.hierarchyClassID
                                                                           and finPos.traitID = @posDept
                     LEFT JOIN HierarchyClassTrait     finevent on   finhc.hierarchyClassID = finevent.hierarchyClassID
                                                                           and finevent.traitID = @nonAlignedSubTeam
              WHERE
              finhc.hierarchyID = @finId
                     
       ),
	   Nat_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, nationalClassCode)
       AS
       (
              SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
              FROM 
                     ItemHierarchyClass                ihc
                     JOIN HierarchyClass               hc     on     ihc.hierarchyClassID = hc.hierarchyClassID
                     JOIN HierarchyClassTrait   hct on hc.hierarchyClassID = hct.hierarchyClassID
                                                                           and hct.traitID = @natClassCodeTrait
              WHERE
                     hc.hierarchyID = @natId
       )




       --=======================================================
       -- Main
       --=======================================================
       SELECT
              sc.itemID                         as ItemId,
              vdat.traitValue                   as ValidationDate,
              sc.scanCode                       as ScanCode,
              sct.scanCodeTypeDesc				as ScanCodeType,
              prd.traitValue                    as ProductDescription,
              pos.traitValue                    as PosDescription,
              pack.traitValue                   as PackageUnit,
              fs.traitValue						as FoodStampEligible,
              tare.traitValue                   as Tare,
              brand.hierarchyClassID			as BrandId,
              CASE 
                    WHEN LEN(brand.hierarchyClassName) > 25 THEN SUBSTRING(brand.hierarchyClassName, 1, 25)
					ELSE brand.hierarchyClassName 
              END                               as BrandName,
              tax.taxAbbreviation				as TaxClassName,
			  nat.nationalClassCode				as NationalClassCode,
              fin.subTeamName					as SubTeamName,
              coalesce(fin.subTeamNo, -1)		as SubTeamNo,
              cast(coalesce(nullif(fin.posDeptNo,''), '-1') AS int)				as DeptNo,
              cast(coalesce(nullif(fin.disableSubTeamEvents,'0'), '0') AS bit)	as SubTeamNotAligned,
			  awr.Description					as AnimalWelfareRating,
			  isa.Biodynamic					as Biodynamic,
			  mt.Description					as CheeseMilkType,
			  isa.CheeseRaw						as CheeseRaw,
			  esr.Description					as EcoScaleRating,
			  CASE 
					WHEN isa.GlutenFreeAgencyId IS NULL THEN NULL
					ELSE CAST(1 AS BIT)
			  END								as GlutenFree,
			  CASE 
					WHEN isa.KosherAgencyId IS NULL THEN NULL
					ELSE CAST(1 AS BIT)
			  END								as Kosher,
			  her.Description as HealthyEatingRating,
			  isa.Msc							as Msc,
			  CASE 
					WHEN isa.OrganicAgencyId IS NULL THEN NULL
					ELSE CAST(1 AS BIT)
			  END								as Organic,
			  CASE 
					WHEN isa.NonGmoAgencyId IS NULL THEN NULL
					ELSE CAST(1 AS BIT)
			  END								as NonGmo,
			  isa.PremiumBodyCare				as PremiumBodyCare,
			  sff.Description					as FreshOrFrozen,
			  sfct.Description					as SeafoodCatchType,
			  CASE 
					WHEN isa.VeganAgencyId IS NULL THEN NULL
					ELSE CAST(1 AS BIT)
			  END								as Vegan,
			  isa.Vegetarian					as Vegetarian,
			  isa.WholeTrade					as WholeTrade,
			  isa.GrassFed						as GrassFed,
			  isa.PastureRaised					as PastureRaised,
			  isa.FreeRange						as FreeRange,
			  isa.DryAged						as DryAged,
			  isa.AirChilled					as AirChilled,
			  isa.MadeInHouse					as MadeInHouse,
			  CASE
				WHEN isa.ItemSignAttributeID IS NULL THEN CAST(0 AS BIT)
				ELSE CAST(1 AS BIT)
			  END								as HasItemSignAttributes,
			  CAST(rsz.TraitValue as decimal(9,4)) as RetailSize,
			  rum.TraitValue					as RetailUom,
			  0									as EventTypeId
       FROM
              @ScanCodes			codes
              JOIN ScanCode			sc          on  codes.ScanCode = sc.scanCode
              JOIN ScanCodeType		sct         on	sc.scanCodeTypeID = sct.scanCodeTypeID
              JOIN ItemTrait_CTE	vdat		on  sc.itemID     = vdat.itemID
                                                        and vdat.traitID = @validationDate
              JOIN ItemTrait_CTE	prd         on  sc.itemID = prd.itemID
                                                        and prd.traitID = @productDescription
              JOIN ItemTrait_CTE	pos         on  sc.itemID = pos.itemID
                                                        and pos.traitID = @posDescription
              JOIN ItemTrait_CTE	pack		on  sc.itemID = pack.itemID
                                                        and pack.traitID = @packageUnit
              JOIN ItemTrait_CTE	fs          on  sc.itemID = fs.itemID
                                                        and fs.traitID = @foodStamp
              JOIN ItemTrait_CTE	tare		on  sc.itemID = tare.itemID
														and tare.traitID = @tare
			  JOIN ItemTrait_CTE	rsz			on  sc.itemID = rsz.itemID
														and rsz.traitID = @retailSize
			  JOIN ItemTrait_CTE	rum			on  sc.itemID = rum.itemID
														and rum.traitID = @retailUom
              LEFT JOIN Brand_CTE	brand		on  sc.itemID = brand.itemID
              LEFT JOIN Tax_CTE			tax     on  sc.itemID = tax.itemID
			  LEFT JOIN Nat_CTE			nat     on  sc.itemID = nat.itemID
              LEFT JOIN FinSubTeam_CTE  fin		on  sc.itemID = fin.itemID
			  LEFT JOIN ItemSignAttribute isa	on	sc.itemID = isa.ItemID
			  LEFT JOIN AnimalWelfareRating awr on	isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
			  LEFT JOIN HealthyEatingRating her on	isa.HealthyEatingRatingId = her.HealthyEatingRatingId
			  LEFT JOIN MilkType	mt			on	isa.CheeseMilkTypeId = mt.MilkTypeId
			  LEFT JOIN EcoScaleRating esr		on	isa.EcoScaleRatingId = esr.EcoScaleRatingId
			  LEFT JOIN SeafoodFreshOrFrozen sff	on	isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId
			  LEFT JOIN SeafoodCatchType sfct	on	isa.SeafoodCatchTypeId = sfct.SeafoodCatchTypeId

END

GO
