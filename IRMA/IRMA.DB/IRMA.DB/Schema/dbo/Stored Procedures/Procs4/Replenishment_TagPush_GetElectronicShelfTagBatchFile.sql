CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile]
AS
 --**************************************************************************************
-- Procedure: Replenishment_TagPush_GetElectronicShelfTagBatchFile
--	  Author: n/a
--      Date: n/a
--
-- Description: This stored proc is called by the TagWriterDAO.vb in IRMA Client
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/07/2013	BS		8755	Updated Item.Discontinue_Item with SIV.DiscontinueItem.
-- 2013-04-26	KM		8755	Same as above, but for the second part of the union.
-- 2013-05-15	DN		12232	Updated the logic in the UNION SELECT query to include:
--								1. Sending UOM
--								2. Select only the Primary Vendor
--								3. Include alternate jurisdiction for non-US items
-- 2013-12-20	DN		14592	Updated the logic to push only batches with 
--								Send Date <= Current Date
-- 2013-12-20	DN		14597	Added 3 addtional fields. They are:
--								Product_Code, Sale_Start_Date, and Sale_End_Date
-- 2014-09-11	DN		15418	Added function fn_GetItemIdentifiers to get validated identifiers
-- 2014-12-12	BJL		15596	Added existing field [SubTeam].[Dept_No] and new field [SubTeam].[POSDept]
-- 2015-03-13	DN		15879	Added logic to use the app config key RegionalESTStoredProcedure to 
--								re-direct the execution of the regional customized stored procedure
-- 2015-05-18	DN		16152	Deprecate POSDept column in IRMA
-- 2016-07-20	Jamali PBI17033	Use Temporary tables to store the output from the table valued functions 
-- 2016-07-20	Jamali PBI16993	Created a new temp table #ItemIdentifier to hold the data from the function fn_GetItemIdentifiers and then join the Temp Table to the second select statement in the Union Query.
--								Added the filter criteria ii.Add_Identifier = 1 OR ii.Remove_Identifier = 1 at the time of adding the data into the #ItemIdentifier table
--								Changed the order of the the columns in the inner join for the table(s) Price, StoreItemVendor
--								Removed the filter SIV.PrimaryVendor from the inner join of the StoreItemVendor and moved it to the where clause
--								Removed the needless Order By clause and the SubTeam_No from the Group By Clause in the sub query for the StoreSubTeam
--								Removed the Top 1 from the sub query for the StoreSub Team as could not find a duplicate row 
-- 2016-11-31  MZ  21454::18806 Updated the logic to interprete the IncludeAllItemIdentifiersInShelfTagPush instance flag value correctly. The value can be
--                              turned on on the regional level or off on the regional level, but have individual store overrides. 
--**************************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL SNAPSHOT	
	
  DECLARE @RegionCode varchar(2),
           @PLUDigits int,
           @OmitShelfTagTypeFields bit,
           @FourLevelHierarchy bit,
           @RegionalEST as varchar(100),
           @CurrentProcedure as varchar(100)
  
  IF OBJECT_ID('tempdb..#tempESL') IS NOT NULL DROP TABLE #tempESL;
  IF(OBJECT_ID('tempdb..#tempDetailSAL') IS NOT NULL) DROP TABLE #tempDetailSAL;
	IF OBJECT_ID('tempdb..#ItemIdentifier') IS NOT NULL DROP TABLE #ItemIdentifier;
	IF OBJECT_ID('tempdb..#StoresToIncludeAllIdentifiers') IS NOT NULL DROP TABLE #StoresToIncludeAllIdentifiers;

  SELECT @RegionCode = PrimaryRegionCode,
		     @PLUDigits = CASE ISNUMERIC(RIGHT(PluDigitsSentToScale, 1))
				              WHEN 1 THEN CONVERT(int, RIGHT(PluDigitsSentToScale, 1))
				              ELSE NULL
			                END
	FROM dbo.InstanceData 

	SELECT 
		@OmitShelfTagTypeFields = dbo.fn_InstanceDataValue('OmitShelfTagTypeFields', NULL),
		@FourLevelHierarchy = dbo.fn_InstanceDataValue('FourLevelHierarchy', NULL)--,
	
	SELECT  @RegionalEST = acv.Value
		FROM AppConfigValue acv  
		INNER JOIN AppConfigEnv ace ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'POS PUSH JOB' AND
			ack.Name = 'RegionalESTStoredProcedure' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
	--#ItemIdentifier: Contains the data for the non-batchable changes
  SELECT Identifier_ID, Item_Key, Identifier, Default_Identifier, Deleted_Identifier, Add_Identifier, Remove_Identifier,
         National_Identifier, CheckDigit, IdentifierType, NumPluDigitsSentToScale, Scale_Identifier
  INTO #ItemIdentifier
	FROM dbo.fn_GetItemIdentifiers()

	--#StoresToIncludeAllIdentifiers: Stores that require both default and alternate identifiers.
  CREATE TABLE #StoresToIncludeAllIdentifiers(StoreNumber INT PRIMARY KEY);
  INSERT INTO #StoresToIncludeAllIdentifiers(StoreNumber)
    SELECT DISTINCT Store_No
	  FROM Store
	  WHERE dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', Store_No) = 1
    ORDER BY Store_No;
	
	SET @CurrentProcedure = object_name(@@PRocid)
	IF (@CurrentProcedure = ltrim(rtrim(@RegionalEST))) or (len(@RegionalEST) = 0)

	BEGIN
    SELECT * INTO #tempESL FROM
    (SELECT 
			[RecordType] = CASE WHEN PBH.ItemChgTypeID = 3 THEN 'D' ELSE 'R' END,
			[Identifier] =	II.Identifier,
			[CurrentPrice] =																	
								CASE
									WHEN PCT.On_Sale = 1 THEN PBD.POSSale_Price
									ELSE PBD.POSPrice
								END,
			[CurrentMultiple] =																
								CASE
									WHEN PCT.On_Sale = 1 THEN PBD.Sale_Multiple
									ELSE PBD.Multiple
								END,
			[Price] = PBD.POSPrice,
			[Multiple] = PBD.Multiple,
			[POS_Desc] =  RTRIM(ISNULL(PBD.POS_Description,  I.POS_Description)),
			[Item_Size] = CONVERT(real, PBD.Package_Desc2),
			[Sign_Desc] = RTRIM(ISNULL(PBD.Sign_Description, I.Sign_Description)),
			[Brand_Name] = LTRIM(RTRIM(PBD.Brand_Name)),
			[Item_Desc] = RTRIM(ISNULL(PBD.Item_Description, I.Item_Description)),
			[CaseSize] = (SELECT TOP 1 CONVERT(int, Package_Desc1)
						FROM dbo.VendorCostHistory  
						WHERE StoreItemVendorID = SIV.StoreItemVendorID
						ORDER BY VendorCostHistoryID DESC),
			[SubTeam_No] = ST.SubTeam_No,
			[Dept_No] = ST.[Dept_No],
			[RetailUnit] = COALESCE(iuoviu.Unit_Abbreviation, PBD.Retail_Unit_Abbr),
			[PackageUnit] = PBD.Package_Unit,
			[Vendor_Item_ID] = RTRIM(IV.Item_ID),
			[Discontinued] = SIV.DiscontinueItem,
			[WarehouseNumber] = RTRIM(IV.Item_ID),
			[NationalCategoryID] = CAT.NatCatID,
			[Taxable] = ISNULL(TAX.HasFlagsSet, 0),
			[PriceChangeType] = PCT.PriceChgTypeDesc,
			[PriceType] = PCT.PriceChgTypeDesc,
			[UnitPriceCategory] = I.Unit_Price_Category,
			[PackageSize] = CONVERT(real, PBD.Package_Desc1),
			[LabelTypeID] = LBL.LabelType_ID,
			[LabelTypeDesc] = LBL.LabelTypeDesc,
			[LinkCode] = P.POSLinkCode,
			[EffectiveDiscoDate] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
			[LastItemChange] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
			[CurrentDate] = CONVERT(VARCHAR(10), GETDATE(), 101),
			[ScaleItem] = CASE WHEN II.Scale_Identifier = 1 THEN 'Y' ELSE 'N' END,
			[Vendor_ID] = RTRIM(V.Vendor_ID),		
			[Vendor_Key] = RTRIM(V.Vendor_Key),
			IA.Check_Box_1, 
			IA.Check_Box_2, 
			IA.Check_Box_3, 
			IA.Check_Box_4, 
			IA.Check_Box_5, 
			IA.Check_Box_6, 
			IA.Check_Box_7, 
			IA.Check_Box_8, 
			IA.Check_Box_9, 
			IA.Check_Box_10, 
			IA.Check_Box_11, 
			IA.Check_Box_12, 
			IA.Check_Box_13, 
			IA.Check_Box_14, 
			IA.Check_Box_15, 
			IA.Check_Box_16, 
			IA.Check_Box_17, 
			IA.Check_Box_18,
			IA.Check_Box_19, 
			SIE.OrderedByInfor as Ordered_By_Infor, 
			IA.Text_1, 
			IA.Text_2,
			IA.Text_3, 
			IA.Text_4, 
			IA.Text_5, 
			IA.Text_6, 
			IA.Text_7, 
			IA.Text_8, 
			IA.Text_9, 
			IA.Text_10, 
			IA.Date_Time_1, 
			IA.Date_Time_2, 
			IA.Date_Time_3, 
			IA.Date_Time_4, 
			IA.Date_Time_5, 
			IA.Date_Time_6, 
			IA.Date_Time_7, 
			IA.Date_Time_8, 
			IA.Date_Time_9, 
			IA.Date_Time_10,
			
			-- the following fields are required by the VB file writer code
			[Store_Name] = RTRIM(S.Store_Name),
			[PriceBatchDetailID] = 0,
			[DRLUOM] = NULL,
			[tagExt] = 'tag',
			[ShelfTagTypeID] = 1,
			[ExemptTagTypeID] = NULL,
			[ExemptTagExt] = NULL,
			[ExemptTagDesc] = NULL,
			[Item_Key] = I.Item_Key,
			[Store_No] = S.Store_No,
			[ElectronicShelfTag] = P.ElectronicShelfTag,
			[Product_Code] = PBD.Product_Code,
			[Sale_Start_Date] = CONVERT(VARCHAR(10), PBD.StartDate, 101),
			[Sale_End_Date] = CONVERT(VARCHAR(10), PBD.Sale_End_Date, 101),
			ISA.Locality,
			ISA.SignRomanceTextLong,
			ISA.SignRomanceTextShort,
			ISA.AnimalWelfareRating,
			ISA.Biodynamic,
			ISA.CheeseMilkType,
			ISA.CheeseRaw,
			ISA.EcoScaleRating,
			ISA.GlutenFree,
			ISA.HealthyEatingRating,
			ISA.Kosher,
			ISA.NonGmo,
			ISA.Organic,
			ISA.PremiumBodyCare,
			ISA.ProductionClaims,
			ISA.FreshOrFrozen,
			ISA.SeafoodCatchType,
			ISA.Vegan,
			ISA.Vegetarian,
			ISA.WholeTrade,
			ISA.UomRegulationChicagoBaby,
			ISA.UomRegulationTagUom,
			ISA.Msc,
			ISA.GrassFed,
			ISA.PastureRaised,
			ISA.FreeRange,
			ISA.DryAged,
			ISA.AirChilled,
			ISA.MadeInHouse,
			ISA.Exclusive,
			ISA.ColorAdded,
			COALESCE(NonScaleNutrifacts.Calories,			ScaleNutrifacts.Calories)			as Calories,
			COALESCE(NonScaleNutrifacts.ServingsPerPortion, ScaleNutrifacts.ServingsPerPortion)	as ServingsPerPortion,
			COALESCE(NonScaleNutrifacts.ServingSizeDesc,	ScaleNutrifacts.ServingSizeDesc)	as ServingSizeDesc,
			COALESCE(NonScaleNutrifacts.SodiumWeight,		ScaleNutrifacts.SodiumWeight)		as SodiumWeight,
			COALESCE(NonScaleNutrifacts.Sugar,				ScaleNutrifacts.Sugar)				as Sugar,
			COALESCE(NonScaleNutrifacts.TotalFatWeight,		ScaleNutrifacts.TotalFatWeight)		as TotalFatWeight,
			COALESCE(NonScaleIngredients.Ingredients,		ScaleIngredients.Ingredients)		as Ingredients,
			COALESCE(NonScaleAllergen.Allergens,			ScaleAllergen.Allergens)			as Allergens,
			I.Retail_Sale
		FROM 
			dbo.PriceBatchDetail PBD --
			INNER JOIN dbo.PriceBatchHeader PBH  ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID --
			INNER JOIN dbo.Store S  ON S.Store_No = PBD.Store_No --
			INNER JOIN dbo.StoreElectronicShelfTagConfig EST ON EST.Store_No = S.Store_No
			INNER JOIN dbo.Item I ON I.Item_Key = PBD.Item_Key -- 
			INNER JOIN dbo.StoreItem SI  ON SI.Store_No = S.Store_No AND SI.Item_Key = I.Item_Key AND SI.Authorized = 1
			LEFT  JOIN dbo.StoreItemExtended SIE ON SIE.Store_No = SI.Store_No AND SIE.Item_Key = SI.Item_Key
			INNER JOIN dbo.SubTeam ST  ON ST.SubTeam_No = PBD.SubTeam_No
			INNER JOIN dbo.ItemUnit IU  ON IU.Unit_ID = I.Package_Unit_ID
			CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
				--Jamali: Removed the order by, this is not needed and causes the execution plan to have a step for order by
					--Removed the SubTeam_No in the group by as it is not being used
				(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
				FROM dbo.StoreSubTeam 
				WHERE SubTeam_No = ST.SubTeam_No AND PS_SubTeam_No IS NOT NULL
				GROUP BY PS_SubTeam_No --SubTeam_No, 
				--ORDER BY SubTeam_No, [StoreCount] DESC
				) SST
			INNER JOIN dbo.ItemIdentifier II  ON II.Item_Key = I.Item_Key
			INNER JOIN dbo.Price P  ON P.Store_No = S.Store_No AND P.Item_Key = I.Item_Key
			INNER JOIN dbo.PricingMethod PM  ON P.PricingMethod_ID = PM.PricingMethod_ID
			INNER JOIN dbo.Vendor V  ON V.Vendor_ID = PBD.Vendor_ID
			INNER JOIN dbo.ItemVendor IV  ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = V.Vendor_ID
			INNER JOIN dbo.StoreItemVendor SIV  ON SIV.Store_No = S.Store_No AND SIV.Item_Key = I.Item_Key AND SIV.Vendor_ID = V.Vendor_ID
			LEFT JOIN 
				(SELECT 
					[Store_No] = SI1.Store_No, 
					[Item_Key] = SI1.Item_Key, 
					[HasFlagsSet] = MAX(CONVERT(tinyint, COALESCE(TOV.TaxFlagValue, TF.TaxFlagValue, 0)))
				FROM dbo.StoreItem SI1 
					INNER JOIN dbo.Store S1  ON S1.Store_No = SI1.Store_No
					INNER JOIN dbo.Item I1  ON I1.Item_Key = SI1.Item_Key
					LEFT JOIN dbo.TaxFlag TF  ON TF.TaxClassID = I1.TaxClassID AND TF.TaxJurisdictionID = S1.TaxJurisdictionID
					LEFT JOIN dbo.TaxOverride TOV  ON TOV.Store_No = S1.Store_No AND TOV.Item_Key = I1.Item_Key AND TOV.TaxFlagKey = TF.TaxFlagKey
				GROUP BY SI1.Store_No, SI1.Item_Key) TAX ON TAX.Store_No = S.Store_No AND TAX.Item_Key = I.Item_Key
			LEFT JOIN dbo.PriceChgType		PCT					 ON PCT.PriceChgTypeID		= PBD.PriceChgTypeID
			LEFT JOIN dbo.ItemAttribute		IA					 ON IA.Item_Key				= I.Item_Key
			LEFT JOIN dbo.NatItemClass		CLS					 ON CLS.ClassID				= I.ClassID
			LEFT JOIN dbo.NatItemCat		CAT					 ON CAT.NatCatID			= CLS.NatCatID
			LEFT JOIN dbo.LabelType			LBL					 ON LBL.LabelType_ID		= I.LabelType_ID
			LEFT JOIN dbo.ItemUomOverride	iuov				 ON PBD.Item_Key			= iuov.Item_Key 
																			AND PBD.Store_No		= iuov.Store_No
			LEFT JOIN dbo.ItemUnit			iuoviu				 ON iuov.Retail_Unit_ID		= iuoviu.Unit_ID
			LEFT JOIN dbo.ItemSignAttribute ISA					 ON I.Item_Key				= ISA.Item_Key 
			LEFT JOIN dbo.ItemNutrition		ITN					 ON I.Item_Key				= ITN.ItemKey		
			LEFT JOIN dbo.Nutrifacts		NonScaleNutrifacts	 ON ITN.NutrifactsID		= NonScaleNutrifacts.NutriFactsID 
			LEFT JOIN dbo.Scale_Ingredient	NonScaleIngredients  ON ITN.Scale_Ingredient_ID = NonScaleIngredients.Scale_Ingredient_ID
			LEFT JOIN dbo.Scale_Allergen	NonScaleAllergen	 ON ITN.Scale_Allergen_ID	= NonScaleAllergen.Scale_Allergen_ID
			LEFT JOIN dbo.ItemScale			ISC					 ON I.Item_Key				= ISC.Item_Key
			LEFT JOIN dbo.Nutrifacts		ScaleNutrifacts		 ON ISC.Nutrifact_ID		= ScaleNutrifacts.NutriFactsID
			LEFT JOIN dbo.Scale_Ingredient	ScaleIngredients	 ON ISC.Scale_Ingredient_ID = ScaleIngredients.Scale_Ingredient_ID
			LEFT JOIN dbo.Scale_Allergen	ScaleAllergen		 ON ISC.Scale_Allergen_ID	= ScaleAllergen.Scale_Allergen_ID
		WHERE 
			PBH.PriceBatchStatusID = 5
			-- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
			AND (@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))
			AND SI.Authorized = 1
			AND CONVERT(DATE, GETDATE()) >= CONVERT(DATE, PBH.StartDate)
			-- Alternate identifiers should only be included for stores overriden in the instance data flag IncludeAllItemIdentifiersInShelfTagPush.
			AND 
			(EXISTS (select StoreNumber from #StoresToIncludeAllIdentifiers where StoreNumber = S.Store_No)
				OR ii.Default_Identifier = 1
			)
      
		UNION
		 
		SELECT
			[RecordType] = CASE WHEN ii.Add_Identifier = 1 THEN 'A' 
								WHEN ii.Remove_Identifier = 1 THEN 'D' 
								WHEN si.POSDeAuth = 1 THEN 'D' 
								WHEN si.Refresh = 1 THEN 'R' 
								ELSE ' ' 
							END ,
			[Identifier] =	ii.Identifier,
			[CurrentPrice] = CASE
								WHEN PCT.On_Sale = 1 THEN p.POSSale_Price
								ELSE p.POSPrice
							END,
			[CurrentMultiple] =	CASE
									WHEN PCT.On_Sale = 1 THEN p.Sale_Multiple
									ELSE p.Multiple
								END,
			[Price] = p.POSPrice,
			[Multiple] = P.Multiple,
			[POS_Desc] =  CASE WHEN sj.StoreJurisdictionDesc <> 'US' THEN ISNULL(ior.POS_Description,RTRIM(I.POS_Description)) ELSE RTRIM(I.POS_Description) END,
			[Item_Size] = CASE WHEN sj.StoreJurisdictionDesc <> 'US' THEN CONVERT(real, ISNULL(ior.Package_Desc2, i.Package_Desc2)) ELSE i.Package_Desc2 END,
			[Sign_Desc] = ISNULL(ior.Sign_Description,RTRIM(I.Sign_Description)),
			[Brand_Name] = LTRIM(RTRIM(ib.Brand_Name)),
			[Item_Desc] = RTRIM(I.Item_Description),
			[CaseSize] = (SELECT TOP 1 CONVERT(int, Package_Desc1)
						FROM dbo.VendorCostHistory 
						WHERE StoreItemVendorID = SIV.StoreItemVendorID
						ORDER BY VendorCostHistoryID DESC),
			[SubTeam_No] = ST.SubTeam_No,
			[Dept_No] = ST.[Dept_No],
			[RetailUnit] = CASE WHEN iuov.Retail_Unit_ID IS NOT NULL THEN (SELECT iur.Unit_Abbreviation FROM ItemUnit iur WHERE iur.Unit_ID = iuov.Retail_Unit_ID)
								WHEN ior.Retail_Unit_ID IS NOT NULL AND sj.StoreJurisdictionDesc <> 'US' THEN (SELECT iur.Unit_Abbreviation FROM ItemUnit iur WHERE iur.Unit_ID = ior.Retail_Unit_ID)
								ELSE (SELECT ir.Unit_Abbreviation FROM ItemUnit ir  WHERE ir.Unit_ID = i.Retail_Unit_ID)
								END,
			[PackageUnit] = CASE WHEN ior.Package_Unit_ID IS NOT NULL AND sj.StoreJurisdictionDesc <> 'US' THEN (SELECT iur.Unit_Abbreviation FROM ItemUnit iur WHERE iur.Unit_ID = ior.Package_Unit_ID)
								ELSE (SELECT ir.Unit_Abbreviation FROM ItemUnit ir  WHERE ir.Unit_ID = i.Package_Unit_ID)
								END,
			[Vendor_Item_ID] = RTRIM(IV.Item_ID),
			[Discontinued] = SIV.DiscontinueItem,
			[WarehouseNumber] = RTRIM(IV.Item_ID),
			[NationalCategoryID] = CAT.NatCatID,
			[Taxable] = ISNULL(TAX.HasFlagsSet, 0),
			[PriceChangeType] = 'PRI',
			[PriceType] = PCT.PriceChgTypeDesc,
			[UnitPriceCategory] = I.Unit_Price_Category,
			[PackageSize] = CONVERT(real, i.Package_Desc1),
			[LabelTypeID] = LBL.LabelType_ID,
			[LabelTypeDesc] = LBL.LabelTypeDesc,
			[LinkCode] = P.POSLinkCode,
			[EffectiveDiscoDate] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
			[LastItemChange] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
			[CurrentDate] = CONVERT(VARCHAR(10), GETDATE(), 101),
			[ScaleItem] = CASE WHEN II.Scale_Identifier = 1 THEN 'Y' ELSE 'N' END,
			[Vendor_ID] = RTRIM(V.Vendor_ID),		
			[Vendor_Key] = RTRIM(V.Vendor_Key),
			IA.Check_Box_1, 
			IA.Check_Box_2, 
			IA.Check_Box_3, 
			IA.Check_Box_4, 
			IA.Check_Box_5, 
			IA.Check_Box_6, 
			IA.Check_Box_7, 
			IA.Check_Box_8, 
			IA.Check_Box_9, 
			IA.Check_Box_10, 
			IA.Check_Box_11, 
			IA.Check_Box_12, 
			IA.Check_Box_13, 
			IA.Check_Box_14, 
			IA.Check_Box_15, 
			IA.Check_Box_16, 
			IA.Check_Box_17, 
			IA.Check_Box_18,
			IA.Check_Box_19, 
			SIE.OrderedByInfor as Ordered_By_Infor,
			IA.Text_1, 
			IA.Text_2,
			IA.Text_3, 
			IA.Text_4, 
			IA.Text_5, 
			IA.Text_6, 
			IA.Text_7, 
			IA.Text_8, 
			IA.Text_9, 
			IA.Text_10, 
			IA.Date_Time_1, 
			IA.Date_Time_2, 
			IA.Date_Time_3, 
			IA.Date_Time_4, 
			IA.Date_Time_5, 
			IA.Date_Time_6, 
			IA.Date_Time_7, 
			IA.Date_Time_8, 
			IA.Date_Time_9, 
			IA.Date_Time_10,
			
	--		 the following fields are required by the VB file writer code
			[Store_Name] = RTRIM(S.Store_Name),
			[PriceBatchDetailID] = 0,
			[DRLUOM] = NULL,
			[tagExt] = 'tag',
			[ShelfTagTypeID] = 1,
			[ExemptTagTypeID] = NULL,
			[ExemptTagExt] = NULL,
			[ExemptTagDesc] = NULL,
			[Item_Key] = I.Item_Key,
			[Store_No] = S.Store_No,
			[ElectronicShelfTag] = P.ElectronicShelfTag,
			[Product_Code] = I.Product_Code,
			[Sale_Start_Date] = CONVERT(VARCHAR(10), P.Sale_Start_Date, 101),
			[Sale_End_Date] = CONVERT(VARCHAR(10), P.Sale_End_Date, 101),
			ISA.Locality,
			ISA.SignRomanceTextLong,
			ISA.SignRomanceTextShort,
			ISA.AnimalWelfareRating,
			ISA.Biodynamic,
			ISA.CheeseMilkType,
			ISA.CheeseRaw,
			ISA.EcoScaleRating,
			ISA.GlutenFree,
			ISA.HealthyEatingRating,
			ISA.Kosher,
			ISA.NonGmo,
			ISA.Organic,
			ISA.PremiumBodyCare,
			ISA.ProductionClaims,
			ISA.FreshOrFrozen,
			ISA.SeafoodCatchType,
			ISA.Vegan,
			ISA.Vegetarian,
			ISA.WholeTrade,
			ISA.UomRegulationChicagoBaby,
			ISA.UomRegulationTagUom,
			ISA.Msc,
			ISA.GrassFed,
			ISA.PastureRaised,
			ISA.FreeRange,
			ISA.DryAged,
			ISA.AirChilled,
			ISA.MadeInHouse,
			ISA.Exclusive,
			ISA.ColorAdded,
			COALESCE(NonScaleNutrifacts.Calories,			ScaleNutrifacts.Calories)			as Calories,
			COALESCE(NonScaleNutrifacts.ServingsPerPortion, ScaleNutrifacts.ServingsPerPortion)	as ServingsPerPortion,
			COALESCE(NonScaleNutrifacts.ServingSizeDesc,	ScaleNutrifacts.ServingSizeDesc)	as ServingSizeDesc,
			COALESCE(NonScaleNutrifacts.SodiumWeight,		ScaleNutrifacts.SodiumWeight)		as SodiumWeight,
			COALESCE(NonScaleNutrifacts.Sugar,				ScaleNutrifacts.Sugar)				as Sugar,
			COALESCE(NonScaleNutrifacts.TotalFatWeight,		ScaleNutrifacts.TotalFatWeight)		as TotalFatWeight,
			COALESCE(NonScaleIngredients.Ingredients,		ScaleIngredients.Ingredients)		as Ingredients,
			COALESCE(NonScaleAllergen.Allergens,			ScaleAllergen.Allergens)			as Allergens,
			I.Retail_Sale
		FROM 
			--jamali: made theh temp table as the main table instead of the Item table
			#ItemIdentifier ii
			INNER JOIN dbo.Item i 				 ON	i.Item_Key					= ii.Item_Key		
			INNER JOIN dbo.StoreItem si ON	si.Item_Key					= ii.Item_Key AND (si.Authorized = 1 OR (si.Authorized = 0 AND si.POSDeAuth = 1))
			LEFT  JOIN dbo.StoreItemExtended SIE ON SIE.Store_No = SI.Store_No AND SIE.Item_Key = SI.Item_Key
			INNER JOIN dbo.Store 							s 	 ON	si.Store_No					= s.Store_No
			INNER JOIN dbo.StoreJurisdiction				sj 	 ON	s.StoreJurisdictionID		= sj.StoreJurisdictionID
			INNER JOIN dbo.StoreElectronicShelfTagConfig 	est   ON	s.Store_No					= est.Store_No
			INNER JOIN dbo.SubTeam 							st 	 ON	i.SubTeam_No				= st.SubTeam_No
			INNER JOIN dbo.ItemUnit							iu 	 ON	i.Package_Unit_ID			= iu.Unit_ID
			INNER JOIN dbo.ItemBrand						ib   ON	i.Brand_ID					= ib.Brand_ID
			CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
				--Jamali: Removed the order by, this is not needed and causes the execution plan to have a step for order by
					--Removed the SubTeam_No in the group by as it is not being used
				(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
				FROM dbo.StoreSubTeam  
				WHERE SubTeam_No = ST.SubTeam_No AND PS_SubTeam_No IS NOT NULL
				GROUP BY PS_SubTeam_No --SubTeam_No, 
				--ORDER BY SubTeam_No, [StoreCount] DESC
				) SST
			--added the hint to use the right index, for some reason, SQL was ignoring this index and using another one that just had the Store_NO
			INNER JOIN dbo.Price p WITH (INDEX(PK__Price) NOLOCK)	  ON P.Item_Key = II.Item_Key AND P.Store_No			= S.Store_No
			INNER JOIN dbo.PricingMethod PM 	 ON PM.PricingMethod_ID	= P.PricingMethod_ID
			INNER JOIN dbo.ItemVendor IV   ON IV.Item_Key = I.Item_Key 
			INNER JOIN dbo.Vendor V	ON V.Vendor_ID = IV.Vendor_ID
			INNER JOIN dbo.StoreItemVendor SIV    ON SIV.Store_No = S.Store_No AND SIV.Item_Key = I.Item_Key AND SIV.Vendor_ID = V.Vendor_ID 
			LEFT JOIN 
				(SELECT 
					[Store_No] = SI1.Store_No, 
					[Item_Key] = SI1.Item_Key, 
					[HasFlagsSet] = MAX(CONVERT(tinyint, COALESCE(TOV.TaxFlagValue, TF.TaxFlagValue, 0)))
				FROM dbo.StoreItem SI1 
					INNER JOIN dbo.Store S1  ON S1.Store_No = SI1.Store_No
					INNER JOIN dbo.Item I1  ON I1.Item_Key = SI1.Item_Key
					LEFT JOIN dbo.TaxFlag TF  ON TF.TaxClassID = I1.TaxClassID AND TF.TaxJurisdictionID = S1.TaxJurisdictionID
					LEFT JOIN dbo.TaxOverride TOV  ON TOV.Store_No = S1.Store_No AND TOV.Item_Key = I1.Item_Key AND TOV.TaxFlagKey = TF.TaxFlagKey
					GROUP BY SI1.Store_No, SI1.Item_Key
				) TAX ON TAX.Store_No = S.Store_No AND TAX.Item_Key = I.Item_Key
			LEFT JOIN dbo.PriceChgType		PCT					 ON P.PriceChgTypeID		= PCT.PriceChgTypeID
			LEFT JOIN dbo.ItemAttribute		IA					 ON IA.Item_Key				= I.Item_Key
			LEFT JOIN dbo.NatItemClass		CLS					 ON CLS.ClassID				= I.ClassID
			LEFT JOIN dbo.NatItemCat		CAT					 ON CAT.NatCatID			= CLS.NatCatID
			LEFT JOIN dbo.LabelType			LBL					 ON LBL.LabelType_ID		= I.LabelType_ID
			LEFT JOIN dbo.ItemOverride		ior					 ON	i.Item_Key				= ior.Item_Key
			LEFT JOIN dbo.ItemUOMOverride	iuov				 ON i.Item_Key				= iuov.Item_Key AND s.Store_No			= iuov.Store_No
			LEFT JOIN dbo.ItemSignAttribute ISA					 ON I.Item_Key				= ISA.Item_Key 
			LEFT JOIN dbo.ItemNutrition		ITN					 ON I.Item_Key				= ITN.ItemKey		
			LEFT JOIN dbo.Nutrifacts		NonScaleNutrifacts	 ON ITN.NutrifactsID		= NonScaleNutrifacts.NutriFactsID 
			LEFT JOIN dbo.Scale_Ingredient	NonScaleIngredients  ON ITN.Scale_Ingredient_ID = NonScaleIngredients.Scale_Ingredient_ID
			LEFT JOIN dbo.Scale_Allergen	NonScaleAllergen	 ON ITN.Scale_Allergen_ID	= NonScaleAllergen.Scale_Allergen_ID
			LEFT JOIN dbo.ItemScale			ISC					 ON I.Item_Key				= ISC.Item_Key
			LEFT JOIN dbo.Nutrifacts		ScaleNutrifacts		 ON ISC.Nutrifact_ID		= ScaleNutrifacts.NutriFactsID
			LEFT JOIN dbo.Scale_Ingredient	ScaleIngredients	 ON ISC.Scale_Ingredient_ID = ScaleIngredients.Scale_Ingredient_ID
			LEFT JOIN dbo.Scale_Allergen	ScaleAllergen		 ON ISC.Scale_Allergen_ID	= ScaleAllergen.Scale_Allergen_ID
		WHERE
			(@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2)) 
			AND (ii.Add_Identifier = 1 OR ii.Remove_Identifier = 1 OR si.POSDeAuth = 1 OR si.Refresh = 1)
			AND SIV.PrimaryVendor = 1) A

   --Previously processed but still active SAL
   DECLARE @today DATE = GetDate(),
           @salID INT =(SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = 'SAL');
   
   CREATE TABLE #tempDetailSAL(Item_Key INT NOT NULL, 
                               Store_No INT NOT NULL,
                               StartDate DATE,
                               SaleEndDate DATE,
                               POSSale_Price SMALLMONEY,
                               Sale_Multiple TINYINT,
                               POSPrice SMALLMONEY,
                               Multiple TINYINT);
   CREATE NONCLUSTERED INDEX [ixSAL] ON #tempDetailSAL (Item_Key, Store_No);

   INSERT INTO #tempDetailSAL(Item_Key, Store_No, StartDate, SaleEndDate, POSSale_Price, Sale_Multiple, POSPrice, Multiple)
   SELECT Item_Key, Store_No, StartDate, SaleEndDate, POSSale_Price, Sale_Multiple, POSPrice, Multiple
   FROM(
     SELECT A.Item_Key, A.Store_No, Cast(A.StartDate AS DATE) StartDate, Cast(IsNull(A.Sale_End_Date, A.StartDate) AS DATE) SaleEndDate,
            A.POSSale_Price, A.Sale_Multiple, A.POSPrice, A.Multiple,
            Row_Number() over(partition by A.Item_Key, A.Store_No order by A.Sale_End_Date DESC) rowID
     FROM PriceBatchDetail A (NOLOCK)
     INNER JOIN PriceBatchHeader (NOLOCK) B ON B.PriceBatchHeaderID = A.PriceBatchHeaderID
     INNER JOIN (SELECT Item_Key FROM #tempESL GROUP BY Item_Key) C on C.Item_Key = A.Item_Key
                 WHERE A.PriceChgTypeID = @salID
                   AND IsNull(A.CancelAllSales, 0) <> 1
                   AND Cast(A.StartDate AS DATE) <= @today
                   AND Cast(A.Sale_End_Date AS DATE) >= @today AND B.PriceBatchStatusID = 6
   ) A
   WHERE rowID = 1;

   IF(Exists(SELECT 1 FROM #tempDetailSAL))
   BEGIN
     CREATE NONCLUSTERED INDEX ix_ItemKey on #tempESL(Item_Key) INCLUDE(Store_No);

     UPDATE A SET A.CurrentPrice = B.POSSale_Price,
                  A.CurrentMultiple = B.Sale_Multiple,
                  A.Price = B.POSPrice,
                  A.Multiple = B.Multiple,
                  A.PriceChangeType = 'SAL',
                  A.PriceType = 'SAL',
                  A.Sale_Start_Date = B.StartDate,
                  A.Sale_End_Date = B.SaleEndDate
     FROM #tempESL A
     INNER JOIN #tempDetailSAL B ON B.Item_Key = A.Item_Key AND B.Store_No = A.Store_No;
   END

    SELECT * FROM #tempESL ORDER BY Store_No, SubTeam_No, Brand_Name, Identifier;
	END
	ELSE
	BEGIN
		EXEC (@RegionalEST)
	END
	
	IF OBJECT_ID('tempdb..#tempESL') IS NOT NULL DROP TABLE #tempESL;
  IF(OBJECT_ID('tempdb..#tempDetailSAL') IS NOT NULL) DROP TABLE #tempDetailSAL;
	IF OBJECT_ID('tempdb..#ItemIdentifier') IS NOT NULL DROP TABLE #ItemIdentifier;
	IF OBJECT_ID('tempdb..#StoresToIncludeAllIdentifiers') IS NOT NULL DROP TABLE #StoresToIncludeAllIdentifiers;
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile] TO [IRMAAdminRole] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::[dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile] TO [IRMAClientRole] AS [dbo];