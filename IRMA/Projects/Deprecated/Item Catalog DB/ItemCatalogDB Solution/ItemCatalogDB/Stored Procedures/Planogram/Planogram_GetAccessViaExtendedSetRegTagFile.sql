IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planogram_GetAccessViaExtendedSetRegTagFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Planogram_GetAccessViaExtendedSetRegTagFile]
GO

CREATE PROCEDURE [dbo].Planogram_GetAccessViaExtendedSetRegTagFile
    @ItemList			varchar(max),
    @ItemListSeparator	char(1),
	@Store_No			int,
	@StartDate			DateTime

AS

-- ****************************************************************************************************************
-- Procedure: Planogram_GetAccessViaExtendedSetRegTagFile()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-04-30	KM		8755	Use new discontinue function to check for discontinue status;
-- ****************************************************************************************************************

BEGIN
	BEGIN TRY

	DECLARE 
	    @NOW datetime,
		@RegionCode varchar(2),
		@PriceChgTypeID as int,
		@PLUDigits int,
        @OmitShelfTagTypeFields bit,
		@IncludeNonDefaultIdentifiers bit
					
	SELECT @NOW = (getdate())
	SELECT TOP 1 @PriceChgTypeID = PriceChgTypeID FROM dbo.PriceChgType (nolock) WHERE On_Sale = 0

    SELECT 
		@RegionCode = PrimaryRegionCode,
		@PLUDigits =
			CASE ISNUMERIC(RIGHT(PluDigitsSentToScale, 1))
				WHEN 1 THEN CONVERT(int, RIGHT(PluDigitsSentToScale, 1))
				ELSE NULL
			END
	FROM dbo.InstanceData (NOLOCK)

	SELECT @OmitShelfTagTypeFields = dbo.fn_InstanceDataValue('OmitShelfTagTypeFields', NULL),
	       @IncludeNonDefaultIdentifiers = dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', NULL)

    SELECT DISTINCT
	    ------------------
        -- SignBatch
        ------------------
		[Event] = 'REG PLANOGRAM PRINT',															
		[AbbreviatedBatchDesc] = CONVERT(varchar(5), GETDATE(), 1) + ' ' + CONVERT(varchar, GETDATE(), 8) + ' Reg Planogram',	
		[Store_No] = P.Store_No,														
		[BusinessUnit_ID] = ST.BusinessUnit_ID,

		------------------
        -- SignItem
        ------------------
		[Region] = @RegionCode,															
		[Item_Key] = PL.Item_Key,														
		[PS_SubTeam_No] = SST.PS_SubTeam_No,											
		[NationalCategoryID] = CAT.NatCatID,											
		[NationalCategoryName] = RTRIM(CAT.NatCatName),									
		[NationalClassID] = CLS.ClassID,												
		[NationalClassName] = RTRIM(CLS.ClassName),										
		[Identifier] =	II.Identifier,													
		[NumPluDigitsSentToScale] =														
			CASE II.IdentifierType
				WHEN 'O' THEN COALESCE(@PLUDigits, II.NumPluDigitsSentToScale, 4)
				ELSE NULL
			END,
		[POS_Desc] =  I.POS_Description,			
		[Item_Desc] = I.Item_Description,			
		[Sign_Desc] = I.Sign_Description,			
		[Brand_Name] = B.Brand_Name,										
		[Price] = P.POSPrice,															
		[Multiple] = P.Multiple,														
		[MSRP_Price] = P.MSRPPrice,													
		[Sale_Price] =																	
			CASE
				WHEN PCT.On_Sale = 1 THEN P.POSSale_Price
				ELSE NULL
			END,
		[Sale_Multiple] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_Multiple
				ELSE NULL
			END,
		[Sale_Start_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_Start_Date
				ELSE NULL
			END,
		[Sale_End_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_End_Date
				ELSE NULL
			END,
		[PackageSize] = CONVERT(real, I.Package_Desc1),								
		[PackageUnit] = PU.Unit_Abbreviation,												
		[Item_Size] = CONVERT(real, I.Package_Desc2),									
		[RetailUnit] = RU.Unit_Abbreviation,												

		[SubTeam_No] = SB.SubTeam_No,													
		[SubTeam] = RTRIM(SB.SubTeam_Name),												

		[Ingredients] = RTRIM(I.Ingredients),												
		[Vendor_ID] = RTRIM(V.Vendor_ID),		
		[Vendor_Key] = RTRIM(V.Vendor_Key),												
		[Vendor_Name] = RTRIM(V.CompanyName),											
		[Vendor_Item_ID] = RTRIM(IV.Item_ID),											
		[CaseSize] = CAST (VCH.Package_Desc1 AS INT),																	
		
		[Origin] = RTRIM(ITO.Origin_Name),	
		[Organic] = I.Organic,
		[Discontinued] = dbo.fn_GetDiscontinueStatus(I.Item_Key, @Store_No, NULL),
		[LinkedItem] = P.LinkedItem,												
		[SoldByWeight] = ISNULL(RU.Weight_Unit, 0),												

		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),											

		[PriceBatchHeaderID] = NULL, 							
		[PriceChangeType] = ISNULL(PCT.PriceChgTypeDesc, 'Item'),
		[Product_Code] = I.Product_Code,
		[ElectronicShelfTag] = P.ElectronicShelfTag,

		-- Planogram info
		[ProductGroup] = PL.ProductGroup,
		[ShelfIdentifier] = PL.ShelfIdentifier,
		[ProductPlacement] = PL.ProductPlacement,
		[MaxUnits] = PL.MaxUnits,
		[ProductFacings] = PL.ProductFacings,
		[ProductPlanogramCode] = PL.ProductPlanogramCode,

		-- the following fields are required by the VB file writer code
		[Store_Name] = RTRIM(ST.Store_Name),
		[PriceBatchDetailID] = 0,
		[DRLUOM] = dbo.fn_GET_DRLUOM_for_item(PL.Item_Key,P.Store_No),
		[tagExt] = 'tag',
		[LabelTypeID] = LBL.LabelType_ID,
		[LabelTypeDesc] = LBL.LabelTypeDesc,
		[ShelfTagTypeID] = 1,
		[ExemptTagTypeID] = NULL,
		[ExemptTagExt] = NULL,
		[ExemptTagDesc] = NULL,
		
		-- all flex item attributes
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
		IA.Check_Box_20, 
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

		--User Defined 1
		P.PricingMethod_ID,
		p.Sale_Earned_Disc1, 
		p.Sale_Earned_Disc2, 
		p.Sale_Earned_Disc3, 

		-- Competitor pricing info
		COMP.CompetitorID, 
		COMP.CompetitorStore, 
		COMP.CompetitorSaleMultiple, 
		COMP.CompetitorSalePrice, 
		COMP.CompetitorPriceCheckDate,

		-- Local Tag
		P.LocalItem as LocalItem, 

	    -- Tag required condition fields, Bug 13681
		[PBDItemInsertDate] = NULL,
		[PBDItemChgTypeID] =	NULL,			
		[ItemOnHandQtyWt] =	NULL,									
		[ItemLastOrderSentDate] = NULL,                                                                                                                                                  
		[ItemLastReceivedDate] =  NULL,                                         
        [ItemLastSoldDate] =  NULL 
      FROM 
		dbo.Price P (nolock)
        INNER JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL
            ON IL.Key_Value = P.Item_Key 
        INNER JOIN dbo.Item I (nolock)
            ON I.Item_Key = IL.Key_Value
		INNER JOIN dbo.Store ST (nolock)
			ON P.Store_No = ST.Store_No
		INNER JOIN dbo.Planogram PL (nolock)
			ON P.Item_Key = PL.Item_Key
			AND PL.Store_No = ST.PSI_Store_No
		JOIN dbo.StoreItem SI (nolock) 
		    ON ST.Store_no = SI.Store_No 
			AND I.Item_Key = SI.Item_Key 
			AND SI.Authorized = 1
        INNER JOIN dbo.SubTeam SB (nolock)
            ON (I.SubTeam_No = SB.SubTeam_No)
        INNER JOIN dbo.ItemIdentifier II (nolock)
            ON I.Item_Key = II.Item_Key
			AND (II.Default_Identifier = 1
				OR (II.Default_Identifier = 0 AND @IncludeNonDefaultIdentifiers = 1))
		INNER JOIN dbo.PriceChgType PCT (nolock) 
			ON P.PriceChgTypeID = PCT.PriceChgTypeID
		INNER JOIN dbo.ItemBrand B (nolock)
			ON I.Brand_ID = B.Brand_ID
		INNER JOIN dbo.ItemUnit as PU (nolock)
			ON I.Package_Unit_ID = PU.Unit_ID
		INNER JOIN dbo.ItemUnit as RU (nolock)
			ON I.Retail_Unit_ID = RU.Unit_ID
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = SB.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
		CROSS APPLY (Select Item_Key, CompetitorID, CompetitorStore, CompetitorSaleMultiple, CompetitorSalePrice, CompetitorPriceCheckDate from dbo.fn_getCompetitivePricingInfo (I.Item_Key, ST.Store_No)) AS Comp
		INNER JOIN dbo.StoreItemVendor SIV (nolock)
			ON ST.Store_no=SIV.Store_No 
			AND I.Item_Key=SIV.Item_Key 
        INNER JOIN dbo.Vendor V (nolock)
            ON SIV.Vendor_ID = V.Vendor_ID
		LEFT JOIN dbo.VendorCostHistory VCH (nolock)
			ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
		LEFT JOIN dbo.ItemVendor IV (nolock)
			ON IL.Key_Value = IV.Item_Key and
			   V.Vendor_ID = IV.Vendor_ID
		LEFT JOIN dbo.ItemAttribute IA (nolock)
			ON IA.Item_Key = IL.Key_Value
		LEFT JOIN dbo.NatItemClass CLS (NOLOCK) 
		    ON CLS.ClassID = I.ClassID
		LEFT JOIN dbo.NatItemCat CAT (NOLOCK) 
		    ON CAT.NatCatID = CLS.NatCatID
		LEFT JOIN dbo.ItemOrigin ITO
		    ON ITO.Origin_ID = I.Origin_ID
		LEFT JOIN dbo.LabelType LBL (NOLOCK) ON LBL.LabelType_ID = I.LabelType_ID
		LEFT JOIN 
			(SELECT 
				[Store_No] = SI1.Store_No, 
				[Item_Key] = SI1.Item_Key, 
				[HasFlagsSet] = MAX(CONVERT(tinyint, COALESCE(TOV.TaxFlagValue, TF.TaxFlagValue, 0)))
			FROM dbo.StoreItem SI1 (NOLOCK)
				INNER JOIN dbo.Store S1 (NOLOCK) ON S1.Store_No = SI1.Store_No
				INNER JOIN dbo.Item I1 (NOLOCK) ON I1.Item_Key = SI1.Item_Key
				LEFT JOIN dbo.TaxFlag TF (NOLOCK) ON TF.TaxClassID = I1.TaxClassID AND TF.TaxJurisdictionID = S1.TaxJurisdictionID
				LEFT JOIN dbo.TaxOverride TOV (NOLOCK) ON TOV.Store_No = S1.Store_No AND TOV.Item_Key = I1.Item_Key AND TOV.TaxFlagKey = TF.TaxFlagKey
			GROUP BY SI1.Store_No, SI1.Item_Key) TAX ON TAX.Store_No = ST.Store_No AND TAX.Item_Key = I.Item_Key
        WHERE 
			PL.Item_Key = IL.Key_Value
			AND ST.store_No = @Store_No
			AND SIV.PrimaryVendor = 1
			AND P.PriceChgTypeID = @PriceChgTypeID
			AND VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
		     -- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
		    AND (@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))
		ORDER BY 
			LabelTypeDesc,
			PL.ProductPlanogramCode, 
			PL.ProductGroup, 
			PL.ShelfIdentifier,
			PL.ProductPlacement

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('Planogram_GetAccessViaExtendedSetRegTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END
GO