CREATE PROCEDURE [dbo].[GetItemChanges]
	@lastChangeVersion bigint,
	@rowOffset int = 0,
	@maxRows int = 2000000
AS

	-- ======================================================
	-- Declare Variables
	-- ======================================================
	DECLARE @wholeFoods int;
	DECLARE @productDescription int;
	DECLARE @posDescription int;
	DECLARE @packageUnit int;
	DECLARE @retailSize int;
	DECLARE @retailUOM int;
	DECLARE @foodStamp int;
	DECLARE @tare int;
	DECLARE @brandId int;
	DECLARE @merchId int;
	DECLARE @taxId int;
	DECLARE @financialId int;
	DECLARE @validationDate int;
	DECLARE @subBrickCode int;
	DECLARE @posDeptNoId int;
	DECLARE @sentToEsbId int;
	DECLARE @alcoholByVolume int;
	DECLARE @brandHierarchyLevel int;

	-- Whole Foods Locale
	SET @wholeFoods = (SELECT localeID FROM Locale l WHERE l.localeName = 'Whole Foods');

	-- Traits
	SET @productDescription = (SELECT traitID FROM Trait WHERE traitDesc = 'Product Description');
	SET @posDescription		= (SELECT traitID FROM Trait WHERE traitDesc = 'POS Description');
	SET @packageUnit		= (SELECT traitID FROM Trait WHERE traitDesc = 'Package Unit');
	SET @foodStamp			= (SELECT traitID FROM Trait WHERE traitDesc = 'Food Stamp Eligible');
	SET @tare				= (SELECT traitID FROM Trait WHERE traitDesc = 'POS Scale Tare');
	SET @retailSize			= (SELECT traitID FROM Trait WHERE traitDesc = 'Retail Size');
	SET @retailUOM			= (SELECT traitID FROM Trait WHERE traitDesc = 'Retail UOM');
	SET @validationDate		= (SELECT traitID FROM Trait WHERE traitDesc = 'Validation Date');
	SET @subBrickCode		= (SELECT traitID FROM Trait WHERE traitDesc = 'Sub Brick Code');
	SET @posDeptNoId		= (SELECT traitID FROM Trait WHERE traitDesc = 'POS Department Number');
	SET @alcoholByVolume	= (SELECT traitID FROM Trait WHERE traitDesc = 'Alcohol By Volume');

	-- Hierarchies
	SET @brandId			= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Brands');
	SET @merchId			= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Merchandise');
	SET @taxId				= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax');
	SET @financialId		= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Financial');

	--Hierarchy Level
	SET @brandHierarchyLevel = ( SELECT hierarchyLevel 
								 FROM HierarchyPrototype
							     WHERE hierarchyID = @brandId 
								 AND hierarchyLevelName ='Brand'
								);

	-- ======================================================
	-- Gather all itemID changes
	-- Note: only need to check specific traits for ItemTrait and HierarchyClassTrait
	-- ======================================================
	SELECT chg.itemID
	INTO #changes
	FROM ( 
			SELECT ic.itemID
			FROM CHANGETABLE(CHANGES dbo.Item, @lastChangeVersion) ic

			UNION

			SELECT DISTINCT sc.itemID
			FROM 
				CHANGETABLE(CHANGES dbo.ScanCode, @lastChangeVersion) scc
				JOIN ScanCode sc on scc.scanCodeID = sc.scanCodeID

			UNION
			
			SELECT itc.itemID
			FROM CHANGETABLE(CHANGES dbo.ItemTrait, @lastChangeVersion) itc
			WHERE
				itc.localeID = @wholeFoods
				AND itc.traitID IN (@productDescription, @posDescription, @packageUnit, @foodStamp, @tare, @retailSize, @retailUOM, @validationDate, @alcoholByVolume) -- only check specific traits
			
			UNION

			SELECT DISTINCT ihcc.itemID
			FROM 
				CHANGETABLE(CHANGES dbo.ItemHierarchyClass, @lastChangeVersion) ihcc
				JOIN HierarchyClass hc on ihcc.hierarchyClassID = hc.hierarchyClassID
			WHERE 
				(hc.hierarchyID = @brandId
				OR hc.hierarchyID = @taxId
				OR hc.hierarchyID = @merchId
				OR hc.hierarchyID = @financialId)

			UNION

			SELECT DISTINCT ihc.itemID
			FROM
				CHANGETABLE(CHANGES dbo.HierarchyClass, @lastChangeVersion) hcc
				JOIN HierarchyClass hc on hcc.hierarchyClassID = hc.hierarchyClassID
				JOIN ItemHierarchyClass ihc on hcc.hierarchyClassID = ihc.hierarchyClassID
			WHERE
				(hc.hierarchyID = @brandId
				OR hc.hierarchyID = @taxId
				OR hc.hierarchyID = @merchId)

			UNION

			SELECT DISTINCT ia.ItemID as itemID
			FROM
			 CHANGETABLE(CHANGES dbo.ItemSignAttribute, @lastChangeVersion) iacc
			 JOIN ItemSignAttribute ia on iacc.ItemSignAttributeID = ia.ItemSignAttributeID

		) chg;
		
	CREATE CLUSTERED INDEX PK_#changes_itemID on #changes (itemID);

	-- ======================================================
	-- Item-Hierarchy Association temp tables
	-- ======================================================
	-- brand-item association temp table
	SELECT
		c.itemID,
		hc.hierarchyClassName,
		hc.hierarchyClassID,
		hc.hierarchyParentClassID,
		hp.hierarchyLevelName
	INTO #itemBrand
	FROM
		#changes						c
		LEFT JOIN ItemHierarchyClass	ihc on	c.itemID				= ihc.itemID
		LEFT JOIN HierarchyClass		hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID
		JOIN HierarchyPrototype         hp  on  hc.hierarchyID          = hp.hierarchyID 
	WHERE
		hc.hierarchyID = @brandId AND hc.hierarchyLevel = @brandHierarchyLevel;

	CREATE NONCLUSTERED INDEX IX_#itemBrands_itemID on #itemBrand (itemID)
		INCLUDE (hierarchyClassName);

	-- tax-item association temp table
	SELECT
		c.itemID,
		hc.hierarchyClassName,
		hc.hierarchyClassID,
		hc.hierarchyParentClassID
	INTO #itemTax
	FROM 
		#changes						c
		LEFT JOIN ItemHierarchyClass	ihc on	c.itemID				= ihc.itemID
		LEFT JOIN HierarchyClass		hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID
	WHERE
		hc.hierarchyID = @taxId;

	CREATE NONCLUSTERED INDEX IX_#itemTax_itemID on #itemTax (itemID)
		INCLUDE (hierarchyClassName);
	
	-- merch/fin-item association temp table
	SELECT
		c.itemID,
		seg.hierarchyClassID		as segmentId,
		seg.hierarchyClassName		as segment,
		fam.hierarchyClassID		as familyId,
		fam.hierarchyClassName		as family,
		cls.hierarchyClassID		as classId,
		cls.hierarchyClassName		as class,
		brk.hierarchyClassID		as brickId,
		brk.hierarchyClassName		as brick,
		sb.hierarchyClassID			as subBrickId,
		sb.hierarchyClassName		as subBrick,
		brk.hierarchyParentClassID	as finHierarchyParentClassID
		--mfm.traitValue			as subteam,
		--pos.traitValue			as posDeptNo
	INTO #itemMerch
	FROM 
		#changes						c
		LEFT JOIN ItemHierarchyClass	ihc on	c.itemID					= ihc.itemID
		LEFT JOIN HierarchyClass		sb	on  ihc.hierarchyClassID		= sb.hierarchyClassID
		LEFT JOIN HierarchyClass		brk	on  sb.hierarchyParentClassID	= brk.hierarchyClassID
		LEFT JOIN HierarchyClass		cls	on  brk.hierarchyParentClassID	= cls.hierarchyClassID
		LEFT JOIN HierarchyClass		fam	on  cls.hierarchyParentClassID	= fam.hierarchyClassID
		LEFT JOIN HierarchyClass		seg	on  fam.hierarchyParentClassID	= seg.hierarchyClassID 
	WHERE
		sb.hierarchyID = @merchId;

	CREATE NONCLUSTERED INDEX IX_#itemMerch_itemID on #itemMerch (itemID)
		INCLUDE (segmentId, segment, familyId, family, classId, class, brickId, brick, subBrickId, subBrick);

	-- financial-item association temp table
	SELECT
		c.itemID				as itemID,
		hc.hierarchyClassName	as subteam,
		pos.traitValue			as posDeptNo
	INTO #itemFinancial
	FROM 
		#changes						c
		LEFT JOIN ItemHierarchyClass	ihc on	c.itemID				= ihc.itemID
		LEFT JOIN HierarchyClass		hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID
		LEFT JOIN HierarchyClassTrait   pos on	hc.hierarchyClassID		= pos.hierarchyClassID
												AND pos.traitID			= @posDeptNoId
	WHERE
		hc.hierarchyID = @financialId;

	CREATE NONCLUSTERED INDEX IX_#itemFinancial_itemID on #itemFinancial (itemID)
		INCLUDE (subteam);

	SELECT c.itemID,
		awr.Description			as AnimalWelfareDescription,
		mt.Description			as MilkTypeDescription, 
		esr.Description			as EcoScaleRatingDescription,
		her.Description			as HealthyEatingRatingDescription, 
		sf.Description			as SeafoodFreshOrFrozenDescription,
		sct.Description			as SeafoodCatchDescription, 
		isa.Biodynamic			as Biodynamic,
		isa.CheeseRaw			as CheeseRaw,
		isa.PremiumBodyCare		as PremiumBodyCare,
		isa.Vegetarian			as Vegetarian,
		isa.WholeTrade			as WholeTrade,
		isa.GrassFed			as GrassFed,
		isa.PastureRaised		as PastureRaised,
		isa.FreeRange			as FreeRange,
		isa.DryAged				as DryAged,
		isa.AirChilled			as AirChilled,
		isa.MadeinHouse			as MadeinHouse,
		case when ISNULL(GlutenFreeAgencyName, '') = '' then 0
			else 1 
		end						as IsCertifiedGlutenFree, 
		case when ISNULL(KosherAgencyName, '') = '' then 0
			else 1 
		end						as IsCertifiedKosher,   
		case when ISNULL(NonGmoAgencyName, '') = '' then 0
			else 1 
		end						as IsCertifiedNonGmo,   
		case when ISNULL(OrganicAgencyName, '') = '' then 0
			else 1 
		end						as IsCertifiedOrganic,   
		case when ISNULL(VeganAgencyName, '') = '' then 0
			else 1 
		end						as	IsCertifiedVegan
		INTO #itemAttribute

		FROM #changes c
		LEFT JOIN ItemSignAttribute isa on c.itemID = isa.ItemID
		LEFT JOIN AnimalWelfareRating awr on isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
		LEFT JOIN MilkType mt on isa.CheeseMilkTypeId = MilkTypeId
		LEFT JOIN EcoScaleRating esr on isa.EcoScaleRatingId = esr.EcoScaleRatingId
		LEFT JOIN HealthyEatingRating her on isa.HealthyEatingRatingId = her.HealthyEatingRatingId
		LEFT JOIN SeafoodFreshOrFrozen sf on isa.SeafoodFreshOrFrozenId = sf.SeafoodFreshOrFrozenId
		LEFT JOIN SeafoodCatchType sct on isa.SeafoodCatchTypeId = sct.SeafoodCatchTypeId;

	CREATE NONCLUSTERED INDEX IX_##itemAttribute_itemID on #itemAttribute (itemID);
	

	-- ======================================================
	-- Main
	-- ======================================================
	SELECT
		i.itemID								as itemID,
		tp.itemTypeDesc							as itemTypeDesc,
		sc.scanCode								as scanCode,
		brnd.hierarchyClassName					as brandName,
		brnd.hierarchyClassID					as brandHierarchyClassID,
		brnd.hierarchyParentClassID				as brandHierarchyParentClassID,
		brnd.hierarchyLevelName					as brandHierarchyLevelName,
		pd.traitValue							as productDescription,
		pos.traitValue							as posDescription,
		pack.traitValue							as packageUnit,
		size.traitValue							as retailSize,
		uom.traitValue							as retailUom,
		fs.traitValue							as foodStamp,
		tare.traitValue							as posScaleTare,
		vld.traitValue							as validationDate,
		abv.traitValue							as alcoholByVolume,
		mrch.segmentId							as segmentId,
		mrch.segment							as segment,
		mrch.familyId							as familyId,
		mrch.family								as family,
		mrch.classId							as classId,
		mrch.class								as class,
		mrch.brickId							as brickId,
		mrch.brick								as brick,
		mrch.subBrickId							as subBrickId,
		mrch.subBrick							as subBrick,
		mrch.finHierarchyParentClassID			as finHierarchyParentClassID,
		tax.hierarchyClassName					as taxClass,
		tax.hierarchyClassId					as taxHierarchyClassID,
		tax.hierarchyParentClassID				as taxHierarchyParentClassID,
		LEFT(fin.subteam, CHARINDEX('(', fin.subTeam) - 1) as subTeamName,
		REPLACE(REPLACE(SUBSTRING(fin.subTeam, CHARINDEX('(', fin.subteam), LEN(fin.subteam)), '(', ''), ')', '') as psSubTeamNo,
		fin.posDeptNo							as posDeptNo,
		ia.AnimalWelfareDescription				as AnimalWelfareDescription,
		ia.MilkTypeDescription					as MilkTypeDescription, 
		ia.EcoScaleRatingDescription			as EcoScaleRatingDescription,
		ia.HealthyEatingRatingDescription		as HealthyEatingRatingDescription, 
		ia.SeafoodFreshOrFrozenDescription		as SeafoodFreshOrFrozenDescription,
		ia.SeafoodCatchDescription				as SeafoodCatchDescription, 
		ia.Biodynamic							as Biodynamic,
		ia.CheeseRaw							as CheeseRaw,
		ia.PremiumBodyCare						as PremiumBodyCare,
		ia.Vegetarian							as Vegetarian,
		ia.WholeTrade							as WholeTrade,
		ia.GrassFed								as GrassFed,
		ia.PastureRaised						as PastureRaised,
		ia.FreeRange							as FreeRange,
		ia.DryAged								as DryAged,
		ia.AirChilled							as AirChilled,
		ia.MadeinHouse							as MadeinHouse,
		ia.IsCertifiedGlutenFree				as IsCertifiedGlutenFree, 
		ia.IsCertifiedKosher					as IsCertifiedKosher,   
		ia.IsCertifiedNonGmo					as IsCertifiedNonGmo,   
		ia.IsCertifiedOrganic					as IsCertifiedOrganic,   
		ia.IsCertifiedVegan						as IsCertifiedVegan,
		COUNT(*) OVER()							as totalRowCount
	FROM
		#changes				c
		LEFT JOIN Item			i		on	c.itemID			= i.itemID	
		LEFT JOIN ItemType		tp		on	i.itemTypeID		= tp.itemTypeID
		LEFT JOIN ScanCode		sc		on	i.itemID			= sc.itemID
		LEFT JOIN ItemTrait		pd		on	i.itemID			= pd.itemID
											and pd.localeID		= @wholeFoods
											and pd.traitID		= @productDescription
		LEFT JOIN ItemTrait		pos		on	i.itemID			= pos.itemID
											and pos.localeID	= @wholeFoods
											and pos.traitID		= @posDescription
		LEFT JOIN ItemTrait		pack	on	i.itemID			= pack.itemID
											and pack.localeID	= @wholeFoods
											and pack.traitID	= @packageUnit
		LEFT JOIN ItemTrait		fs		on	i.itemID			= fs.itemID
											and fs.localeID		= @wholeFoods
											and fs.traitID		= @foodStamp
		LEFT JOIN ItemTrait		tare	on	i.itemID			= tare.itemID
											and tare.localeID	= @wholeFoods
											and tare.traitID	= @tare
		LEFT JOIN ItemTrait		size	on	i.itemID			= size.itemID
											and size.localeID	= @wholeFoods
											and size.traitID	= @retailSize
		LEFT JOIN ItemTrait		uom		on	i.itemID			= uom.itemID	
											and uom.localeID	= @wholeFoods
											and uom.traitID		= @retailUOM
		LEFT JOIN ItemTrait		vld		on	i.itemID			= vld.itemID
											and vld.localeID	= @wholeFoods
											and vld.traitID		= @validationDate
		LEFT JOIN ItemTrait		abv		on	i.itemID			= abv.itemID
											and abv.localeID	= @wholeFoods
											and abv.traitID		= @alcoholByVolume
		LEFT JOIN #itemBrand	brnd	on	i.itemID			= brnd.itemID
		LEFT JOIN #itemTax		tax		on	i.itemID			= tax.itemID
		LEFT JOIN #itemMerch	mrch	on	i.itemID			= mrch.itemID
		LEFT JOIN #itemFinancial fin	on	i.itemID			= fin.itemID
		LEFT JOIN #itemAttribute ia		on	i.itemID			= ia.itemID
	WHERE
		tp.itemTypeDesc IN ('Retail Sale', 'Deposit')
	ORDER BY
		i.itemID OFFSET @rowOffset ROWS FETCH NEXT @maxRows ROWS ONLY;

	DROP TABLE #changes;
	DROP TABLE #itemBrand;
	DROP TABLE #itemTax;
	DROP TABLE #itemMerch;
	DROP TABLE #itemAttribute;
	DROP TABLE #itemFinancial;
GO