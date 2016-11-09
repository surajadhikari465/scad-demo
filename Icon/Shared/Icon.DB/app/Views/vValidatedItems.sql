/*
Title: View of Validated Items with POS Attributes to be sent back to IRMA

Description: This view shows only validated scancodes with the all traits.
			 This view will not show non-validated scancodes and will not show
			 validated scancodes that do not have the following traits defined:
			 ValidationDate, Product Description, POS Description, Package Unit,
			 Food Stamp Eligible, POS Scale Tare and the hierarchies Brand and Tax.

Change History: TFS		Initials	Description		Date
				2625	BJL			View Creation	2014-04-11
*/
CREATE VIEW [app].[vValidatedItems]
	AS
SELECT
	 [ItemID]				= vi.[itemID]
	,[ValidationDate]		= vi.[traitValue]
	,[ScanCode]				= scn.[scancode]
	,[ScanCodeType]			= tscn.[scancodetypedesc]
	,[ProductDescription]	= lng.[traitValue]
	,[POSDescription]		= sht.[traitValue]
	,[PackageUnit]			= pu.[traitValue]
	,[FoodStampEligible]	= fs.[traitValue]
	,[Tare]					= pst.[traitValue]
	,[BrandId]				= hcb.[hierarchyclassID]
	,[Brand]				= hcb.[hierarchyclassname]
	,[Tax]					= hct.[hierarchyclassname]

FROM

-- Validation Date
(SELECT i.[itemId], vd.[traitValue]
FROM [Item] i				
JOIN [ItemTrait] vd		ON i.[itemID] = vd.[itemid]
JOIN [Trait] tvd			ON vd.[traitID] = tvd.[traitID]
							AND tvd.[traitdesc] = 'Validation Date') as vi

-- ScanCode for UPC
JOIN [scancode] scn		ON vi.[itemid] = scn.[itemid]
JOIN [scancodetype] tscn	ON scn.[scancodetypeid] = tscn.[scancodetypeid]

-- Product Description
JOIN [itemtrait] lng		ON vi.[itemid] = lng.[itemid]
JOIN [trait] tlng			ON lng.[traitID] = tlng.[traitID]
						AND tlng.[traitDesc] = 'Product Description'

-- POS Description
JOIN [ItemTrait] sht		ON vi.[itemid] = sht.[itemid]
JOIN [trait] tsht			ON sht.[traitID] = tsht.[traitID]
						AND tsht.[traitdesc] = 'POS Description'

-- Package Unit
JOIN [itemtrait] pu		ON vi.[itemid] = pu.[itemid]
JOIN [trait] tpu			ON pu.[traitID] = tpu.[traitID]
						AND tpu.[traitdesc] = 'Package Unit'

-- Food Stamp Eligible
JOIN [itemtrait] fs		ON vi.[itemid] = fs.[itemid]
JOIN [trait] tfs			ON fs.[traitID] = tfs.[traitID]
						AND tfs.[traitdesc] = 'Food Stamp Eligible'

-- POS Scale Tare
JOIN [itemtrait] pst		ON vi.[itemid] = pst.[itemid]
JOIN [trait] tpst			ON pst.[traitID] = tpst.[traitID]
						AND tpst.[traitdesc] = 'POS Scale Tare'

-- Brand
JOIN [itemhierarchyclass] ihcb	ON vi.[itemid] = ihcb.[itemid]
JOIN [hierarchyclass] hcb			ON ihcb.[hierarchyclassid] = hcb.[hierarchyclassid]
JOIN [hierarchy] hb				ON hcb.[hierarchyid] = hb.[hierarchyid]
								AND hb.[hierarchyname] = 'Brand'

-- Tax
JOIN [itemhierarchyclass] ihct	ON vi.[itemid] = ihct.[itemid]
JOIN [hierarchyclass] hct			ON ihct.[hierarchyclassid] = hct.[hierarchyclassid]
JOIN [hierarchy] ht				ON hct.[hierarchyid] = ht.[hierarchyid]
								AND ht.[hierarchyname] = 'Tax'

GO
