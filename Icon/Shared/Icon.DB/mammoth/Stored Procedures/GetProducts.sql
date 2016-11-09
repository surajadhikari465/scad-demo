CREATE PROCEDURE [mammoth].[GetProducts]
	@Instance int
AS
BEGIN

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
	DECLARE @nationalClassId int;
	DECLARE @financialMapId int;
	DECLARE @validationDate int;
	DECLARE @productAddEventTypeId int;
	DECLARE @productUpdateEventTypeId int;

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
	SET @financialMapId		= (SELECT traitID FROM Trait WHERE traitDesc = 'Merch Fin Mapping');
	SET @validationDate		= (SELECT traitID FROM Trait WHERE traitDesc = 'Validation Date');

	-- Hierarchies
	SET @brandId			= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Brands');
	SET @merchId			= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Merchandise');
	SET @taxId				= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax');
	SET @financialId		= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Financial');
	SET @nationalClassId	= (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'National');

	-- Event Types
	SET @productAddEventTypeId		= (SELECT EventTypeId FROM mammoth.EventType where Name = 'ProductAdd');
	SET @productUpdateEventTypeId	= (SELECT EventTypeId FROM mammoth.EventType where Name = 'ProductUpdate');

	-- ======================================================
	-- Item-Hierarchy Association CTEs
	-- ======================================================
	-- brand-item association
	WITH Brand_CTE (itemID, hierarchyClassID)
	AS
	(
		SELECT
			ihc.itemID,
			hc.hierarchyClassID
		FROM
			ItemHierarchyClass			ihc
			LEFT JOIN HierarchyClass	hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @brandId
	),
	-- tax-item association
	Tax_CTE (itemID, hierarchyClassID)
	AS
	(
		SELECT
			ihc.itemID,
			hc.hierarchyClassID
		FROM 
			ItemHierarchyClass			ihc
			LEFT JOIN HierarchyClass	hc	on	ihc.hierarchyClassID	= hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @taxId
	),
	-- merch/fin-item association
	MerchFin_CTE (itemID, subBrickId, subTeamId)
	AS
	(
		SELECT
			ihc.itemID,
			sb.hierarchyClassID		as subBrickId,
			fin.hierarchyClassID	as subTeamId
		FROM 
			ItemHierarchyClass				ihc
			LEFT JOIN HierarchyClass		sb	on  ihc.hierarchyClassID	= sb.hierarchyClassID
			LEFT JOIN HierarchyClassTrait	mfm	on	sb.hierarchyClassID		= mfm.hierarchyClassID
													AND mfm.traitID			= @financialMapId
			LEFT JOIN HierarchyClass		fin	on	mfm.traitValue			= fin.hierarchyClassName
													AND fin.hierarchyID		= @financialId
		WHERE
			sb.hierarchyID = @merchId
	),
	-- NationalClass-item association
	National_CTE (itemID, hierarchyClassId)
	AS
	(
		SELECT
			ihc.itemID,
			hc.hierarchyClassID
		FROM 
			ItemHierarchyClass			ihc
			LEFT JOIN HierarchyClass	hc	on	ihc.hierarchyClassID = hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @nationalClassId
	)

	-- ======================================================
	-- Main
	-- ======================================================
	SELECT
		eq.QueueId				as QueueId,
		eq.EventTypeId			as EventTypeId,
		i.itemID				as ItemId,
		tp.itemTypeID			as ItemTypeId,
		sc.scanCode				as ScanCode,
		pd.traitValue			as ProductDescription,
		pos.traitValue			as PosDescription,
		pack.traitValue			as PackageUnit,
		size.traitValue			as RetailSize,
		uom.traitValue			as RetailUom,
		fs.traitValue			as FoodStamp,
		tare.traitValue			as PosScaleTare,
		vld.traitValue			as ValidationDate,
		brnd.hierarchyClassID	as BrandId,
		tax.hierarchyClassID	as TaxClassId,
		mfin.subBrickId			as SubBrickId,
		mfin.subTeamId			as SubTeamId,
		nat.hierarchyClassID	as NationalClassId
	FROM
		mammoth.EventQueue		eq
		INNER JOIN Item			i		on	eq.EventReferenceId = i.itemID
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
		LEFT JOIN Brand_CTE		brnd	on	i.itemID			= brnd.itemID
		LEFT JOIN Tax_CTE		tax		on	i.itemID			= tax.itemID
		LEFT JOIN MerchFin_CTE	mfin	on	i.itemID			= mfin.itemID
		LEFT JOIN National_CTE	nat		on	i.itemID			= nat.itemID
	WHERE
		eq.InProcessBy = @Instance
		and (eq.EventTypeId = @productAddEventTypeId 
			or eq.EventTypeId = @productUpdateEventTypeId);

END
