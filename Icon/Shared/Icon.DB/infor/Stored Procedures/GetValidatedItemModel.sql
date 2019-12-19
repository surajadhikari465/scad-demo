CREATE PROCEDURE [infor].[GetValidatedItemModel]
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
	   declare @trueBit bit;
	   declare @falseBit bit;
              
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

	   -- helper variables
	   set @trueBit = 1;
	   set @falseBit = 0;

       --=======================================================
       -- Setup CTEs
       --=======================================================
       WITH 
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
                     JOIN HierarchyClass			   finhc	on	ihc.hierarchyClassID = finhc.hierarchyClassID 
																		and finhc.hierarchyID = @finId
                     LEFT JOIN HierarchyClassTrait     finPos	on  finhc.hierarchyClassID = finPos.hierarchyClassID
                                                                        and finPos.traitID = @posDept
                     LEFT JOIN HierarchyClassTrait     finevent on  finhc.hierarchyClassID = finevent.hierarchyClassID
                                                                        and finevent.traitID = @nonAlignedSubTeam
                     
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
              JSON_VALUE(i.ItemAttributesJson, N'$.CreatedDateTimeUtc') as ValidationDate,
              sc.scanCode                       as ScanCode,
              sct.scanCodeTypeDesc				as ScanCodeType,
              JSON_VALUE(i.ItemAttributesJson, N'$.ProductDescription')         as ProductDescription,
              JSON_VALUE(i.ItemAttributesJson, N'$.POSDescription')             as PosDescription,
              JSON_VALUE(i.ItemAttributesJson, N'$.ItemPack')	                as PackageUnit,
              JSON_VALUE(i.ItemAttributesJson, N'$.FoodStampEligible')			as FoodStampEligible,
              JSON_VALUE(i.ItemAttributesJson, N'$.POSScaleTare')               as Tare,
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
			  JSON_VALUE(i.ItemAttributesJson, N'$.AnimalWelfareRating')	    as AnimalWelfareRating,
			  CASE
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Biodynamic') = 'Yes' THEN @trueBit
					ELSE @falseBit
              END                                                           	as Biodynamic, 
			  JSON_VALUE(i.ItemAttributesJson, N'$.CheeseAttributeMilkType')	as MilkType,
			  CASE
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Raw') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END                                                           	as CheeseRaw,
			  JSON_VALUE(i.ItemAttributesJson, N'$."Eco-ScaleRating"')		as EcoScaleRating,
			  CASE
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.GlutenFree') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.GlutenFree')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.GlutenFree')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END								as GlutenFree,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Kosher') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.Kosher')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.Kosher')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END								as Kosher,
			  JSON_VALUE(i.ItemAttributesJson, N'$.HealthyEatingRating') as HealthyEatingRating,
			  CASE
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Msc') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END								as Msc,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Organic') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.Organic')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.Organic')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END								as Organic,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.NonGMOClaim') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.NonGMOClaim')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.NonGMOClaim')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END								as NonGmo,
			  CASE
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.PremiumBodyCare') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END								as PremiumBodyCare,
			  JSON_VALUE(i.ItemAttributesJson, N'$.FreshorFrozen')				as FreshOrFrozen,
			  JSON_VALUE(i.ItemAttributesJson, N'$.SeafoodWildOrFarmRaised')		as SeafoodCatchType,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Vegan') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.Vegan')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.Vegan')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END								as Vegan,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.Vegetarian') IS NULL OR trim(JSON_VALUE(i.ItemAttributesJson, N'$.Vegetarian')) = '' THEN NULL
					WHEN trim(JSON_VALUE(i.ItemAttributesJson, N'$.Vegetarian')) = 'No' THEN @falseBit
					ELSE @trueBit
			  END					            as Vegetarian,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.WholeTrade') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END            					as WholeTrade,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.GrassFed') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END            					as GrassFed,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.PastureRaised') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END            					as PastureRaised,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.FreeRange') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END            					as FreeRange,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.DryAged') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END        						as DryAged,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.AirChilled') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END                               as AirChilled,
			  CASE 
					WHEN JSON_VALUE(i.ItemAttributesJson, N'$.MadeInHouse') = 'Yes' THEN @trueBit
					ELSE @falseBit
			  END                               as MadeInHouse,
			  CASE
				WHEN (JSON_VALUE(i.ItemAttributesJson, N'$.AnimalWelfareRating') IS NOT NULL        
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Biodynamic') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.CheeseAttributeMilkType') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Raw') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$."Eco-ScaleRating"') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.GlutenFree') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Kosher') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Msc') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Organic') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.NonGMOClaim') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.PremiumBodyCare') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.FreshorFrozen') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.SeafoodWildOrFarmRaised') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Vegan') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.Vegetarian') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.WholeTrade') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.GrassFed') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.PastureRaised') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.FreeRange') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.DryAged') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.AirChilled') IS NOT NULL
				  OR  JSON_VALUE(i.ItemAttributesJson, N'$.MadeInHouse') IS NOT NULL)
					THEN @trueBit
				ELSE @falseBit
			  END																as HasItemSignAttributes,
			  try_Cast(JSON_VALUE(i.ItemAttributesJson, N'$.RetailSize') as decimal(9,4))	as RetailSize,
			  JSON_VALUE(i.ItemAttributesJson, N'$.UOM')						as RetailUom,
			  0																	as EventTypeId,
			  it.itemTypeCode													as ItemTypeCode,
			  JSON_VALUE(i.ItemAttributesJson, N'$.CustomerFriendlyDescription')	as CustomerFriendlyDescription
       FROM
              @ScanCodes			codes
              JOIN ScanCode			sc          on  codes.ScanCode = sc.scanCode
			  JOIN Item             i           on  i.itemID = sc.itemID
			  JOIN ItemType         it          on  it.itemTypeID = i.itemTypeID
              JOIN ScanCodeType		sct         on	sc.scanCodeTypeID = sct.scanCodeTypeID
              LEFT JOIN Brand_CTE	brand		on  sc.itemID = brand.itemID
              LEFT JOIN Tax_CTE			tax     on  sc.itemID = tax.itemID
			  LEFT JOIN Nat_CTE			nat     on  sc.itemID = nat.itemID
              LEFT JOIN FinSubTeam_CTE  fin		on  sc.itemID = fin.itemID	  
END
