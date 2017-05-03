CREATE PROCEDURE [dbo].[Planogram_GetAccessViaExtendedNonRegTagFile]
    @ItemList			varchar(max),
    @ItemListSeparator	char(1),
    @Store_No			int,
	@StartDate			DateTime

AS

-- ****************************************************************************************************************
-- Procedure: Planogram_GetAccessViaExtendedNonRegTagFile()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-04-30	KM		8755	Change Item.Discontinue_Item to SIV.DiscontinueItem;
-- ****************************************************************************************************************

BEGIN

	BEGIN TRY

		DECLARE 
			@NOW datetime,
			@RegionCode varchar(2),
		    @PriceChgTypeID as int,
			@PLUDigits int,
			@RegPriceChgTypeID as int,
			@OmitShelfTagTypeFields bit,
			@IncludeNonDefaultIdentifiers bit
		
		SELECT @NOW = (getdate())
	
        SELECT 
		       @RegionCode = PrimaryRegionCode,
		       @PLUDigits =
			    CASE ISNUMERIC(RIGHT(PluDigitsSentToScale, 1))
				WHEN 1 THEN CONVERT(int, RIGHT(PluDigitsSentToScale, 1))
				ELSE NULL
			    END
	      FROM dbo.InstanceData (NOLOCK)

        SELECT 
		@OmitShelfTagTypeFields = dbo.fn_InstanceDataValue('OmitShelfTagTypeFields', NULL),
		@IncludeNonDefaultIdentifiers = dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', NULL)

	    SELECT top 1 @RegPriceChgTypeID = PriceChgTypeID from dbo.PriceChgType where On_Sale = 0

	    DECLARE @tblPBD TABLE (PBDID INT)
		DECLARE @Item_Key INT, 
				@message varchar(700)
	    DECLARE pbd_cursor CURSOR FOR 
	    SELECT Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value

          OPEN pbd_cursor
         FETCH NEXT FROM pbd_cursor INTO @Item_Key

         WHILE @@FETCH_STATUS = 0
         BEGIN

        INSERT @tblPBD

        SELECT TOP 1 pbd.PriceBatchDetailID 
          FROM dbo.Pricebatchdetail pbd (nolock)
	      JOIN dbo.PriceChgType pct (nolock) on pct.PriceChgTypeID = pbd.PriceChgTypeID
         WHERE Item_Key = @Item_Key AND store_no = @Store_No
	       AND @StartDate BETWEEN pbd.startdate and pbd.sale_end_date
	       AND pbd.PriceChgTypeID <> @RegPriceChgTypeID
		   AND pbd.Expired = 0
      ORDER BY pct.priority DESC, pbd.startdate

        FETCH NEXT FROM pbd_cursor INTO @Item_Key

        END

        CLOSE pbd_cursor
        DEALLOCATE pbd_cursor

		DECLARE	@Temp TABLE (Store_No int, Item_Key int, Subteam_No int, PBDItemInsertDate datetime, PBDItemChgTypeID tinyint, 
				ItemLastOrderSentDate smalldatetime NULL, ItemOnHandQtyWt decimal(18,4) NULL, ItemLastReceivedDate datetime NULL, 
				ItemLastSoldDate smalldatetime NULL)
	    INSERT INTO @Temp (Store_No, Item_Key, Subteam_No, PBDItemInsertDate, PBDItemChgTypeID)                            
				SELECT	PBD.Store_No, PBD.Item_Key ,PBD.SubTeam_No, PBD.Insert_Date, PBD.ItemChgTypeID
				FROM dbo.PriceBatchDetail PBD (NOLOCK)
				INNER JOIN @tblPBD T ON PBD.PriceBatchDetailID = T.PBDID 
    
         UPDATE T
            SET ItemOnHandQtyWt = ISNULL(ONH.Quantity, 0) + ISNULL(ONH.Weight, 0)
	       FROM @Temp T
	     INNER JOIN OnHand ONH (nolock) ON ONH.Item_Key = T.Item_Key 
									AND ONH.Store_No = T.Store_No 
									AND ONH.SubTeam_No = T.SubTeam_No
	
	     UPDATE T
            SET ItemLastOrderSentDate = (SELECT Top 1 OH.SentDate FROM OrderItem (nolock)
                                            INNER JOIN OrderHeader (NOLOCK) OH ON OrderItem.OrderHeader_ID = OH.OrderHeader_ID
                                            INNER JOIN Vendor RV (nolock) ON RV.Vendor_ID =  OH.ReceiveLocation_ID
                                            WHERE RV.Store_No = T.Store_No
                                                AND OH.SentDate IS NOT NULL
                                                AND OH.CloseDate IS NULL
                                                AND OH.Transfer_To_SubTeam = T.SubTeam_No
                                                AND OH.Return_Order = 0
                                                AND OrderItem.Item_Key = T.Item_Key
                                             ORDER BY OH.SentDate DESC)
	       FROM @Temp T
	
	     UPDATE T
            SET ItemLastReceivedDate = (SELECT TOP 1 IH.DateStamp FROM ItemHistory IH (nolock) 
                                            WHERE IH.Item_Key = T.Item_Key 
                                                AND IH.Store_No = T.Store_No 
                                                AND IH.SubTeam_No = T.SubTeam_No 
                                                AND IH.Adjustment_ID = 5 --For receiving
                                                ORDER BY IH.ItemHistoryID DESC)
	       FROM @Temp T

	     UPDATE T
            SET ItemLastSoldDate = (SELECT TOP 1 IH.DateStamp FROM ItemHistory IH (nolock) 
                                            WHERE IH.Item_Key = T.Item_Key 
                                                AND IH.Store_No = T.Store_No 
                                                AND IH.SubTeam_No = T.SubTeam_No 
                                                AND IH.Adjustment_ID = 3 --For sales
                                                ORDER BY IH.ItemHistoryID DESC)
	       FROM @Temp T

    --SELECT DISTINCT
		SELECT
        ------------------
        -- SignBatch
        ------------------
		[Event] = 'NON-REG PLANOGRAM PRINT',														
		[AbbreviatedBatchDesc] = PBH.BatchDescription,									
		[Store_No] = PBD.Store_No,														
		[BusinessUnit_ID] = ST.BusinessUnit_ID,

		------------------
        -- SignItem
        ------------------
		[Region] = @RegionCode,																											
		[Item_Key] = PBD.Item_Key,														
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

		[POS_Desc] =  RTRIM(ISNULL(PBD.POS_Description,  I.POS_Description)),			
		[Item_Desc] = RTRIM(ISNULL(PBD.Item_Description, I.Item_Description)),			
		[Sign_Desc] = RTRIM(ISNULL(PBD.Sign_Description, I.Sign_Description)),			

		[Brand_Name] = LTRIM(RTRIM(PBD.Brand_Name)),											

		[Price] = PBD.POSPrice,															
		[Multiple] = PBD.Multiple,														

		[MSRP_Price] = PBD.MSRPPrice,													

		[Sale_Price] =																	
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.POSSale_Price
				ELSE NULL
			END,
		[Sale_Multiple] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.Sale_Multiple
				ELSE NULL
			END,
		[Sale_Start_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.StartDate
				ELSE NULL
			END,
		[Sale_End_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.Sale_End_Date
				ELSE NULL
			END,
        [PackageSize] = CONVERT(real, PBD.Package_Desc1),								
		[PackageUnit] = PBD.Package_Unit,												
		[Item_Size] = CONVERT(real, PBD.Package_Desc2),									
		[RetailUnit] = PBD.Retail_Unit_Abbr,											

		[SubTeam_No] = SB.SubTeam_No,													
		[SubTeam] = RTRIM(SB.SubTeam_Name),												

		[Ingredients] = RTRIM(PBD.Ingredients),											
		[Vendor_ID] = RTRIM(V.Vendor_ID),												
		[Vendor_Key] = RTRIM(V.Vendor_Key),												
		[Vendor_Name] = RTRIM(V.CompanyName),											
		[Vendor_Item_ID] = RTRIM(IV.Item_ID),											
		[CaseSize] = CAST (VCH.Package_Desc1 AS INT),																	

		[Origin] = RTRIM(PBD.Origin_Name),												
		[Organic] = PBD.Organic,
		[Discontinued] = SIV.DiscontinueItem,											
		[LinkedItem] = PBD.LinkedItem,													
		[SoldByWeight] = PBD.Sold_By_Weight,											

		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),											

		[PriceBatchHeaderID] = PBD.PriceBatchHeaderID,									
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
		[PriceBatchDetailID] = PBD.PriceBatchDetailID,
		[DRLUOM] = dbo.fn_GET_DRLUOM_for_item(PBD.Item_Key,PBD.Store_No),
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
		PM.PricingMethod_ID,
		P.Sale_Earned_Disc1, 
		P.Sale_Earned_Disc2, 
		P.Sale_Earned_Disc3, 

		-- Competitor pricing info
		COMP.CompetitorID, 
		COMP.CompetitorStore, 
		COMP.CompetitorSaleMultiple, 
		COMP.CompetitorSalePrice, 
		COMP.CompetitorPriceCheckDate,

		-- Local Tag
		P.LocalItem as LocalItem,

		-- Tag required condition fields, Bug 13681
		[PBDItemInsertDate] = T.PBDItemInsertDate,
		[PBDItemChgTypeID] =	T.PBDItemChgTypeID,			
		[ItemOnHandQtyWt] =	T.ItemOnHandQtyWt,									
		[ItemLastOrderSentDate] = T.ItemLastOrderSentDate,                                                                                                                                                  
		[ItemLastReceivedDate] =  T.ItemLastReceivedDate,                                         
        [ItemLastSoldDate] =  T.ItemLastSoldDate
		
      FROM 
		dbo.PriceBatchDetail PBD (nolock) 
		INNER JOIN @tblPBD IL ON IL.PBDID = PBD.PriceBatchDetailID
		INNER JOIN @Temp T ON T.Item_Key = PBD.Item_Key
		INNER JOIN dbo.Store ST (nolock) ON PBD.Store_No = ST.Store_No AND PBD.Store_No = @Store_No
		INNER JOIN dbo.Price P (nolock) ON PBD.Item_Key = P.Item_Key AND PBD.Store_No = P.Store_No
        INNER JOIN dbo.Item I (nolock) ON I.Item_Key = PBD.Item_Key
		INNER JOIN dbo.SubTeam SB (nolock) ON (I.SubTeam_No = SB.SubTeam_No)
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = SB.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
        INNER JOIN dbo.ItemIdentifier II (nolock) ON PBD.Item_Key = II.Item_Key AND (II.Default_Identifier = 1
				OR (II.Default_Identifier = 0 AND @IncludeNonDefaultIdentifiers = 1))
		INNER JOIN dbo.Planogram PL (nolock) ON PBD.Item_Key = PL.Item_Key AND  PL.Store_No = ST.PSI_Store_No
		INNER JOIN dbo.Vendor V (nolock) ON V.Vendor_ID = PBD.Vendor_ID
		INNER JOIN dbo.ItemVendor IV (nolock) ON PBD.Item_Key = IV.Item_Key AND PBD.Vendor_ID = IV.Vendor_ID 
		INNER JOIN dbo.StoreItem SI (nolock)
			ON ST.Store_no=SI.Store_No AND I.Item_Key=SI.Item_Key AND SI.Authorized = 1  
		INNER JOIN dbo.StoreItemVendor SIV (nolock) ON ST.Store_no = SIV.Store_No AND I.Item_Key = SIV.Item_Key AND SIV.Vendor_ID = V.Vendor_ID 
		INNER JOIN dbo.PricingMethod PM (NOLOCK) ON P.PricingMethod_ID = PM.PricingMethod_ID
		CROSS APPLY (Select Item_Key, CompetitorID, CompetitorStore, CompetitorSaleMultiple, CompetitorSalePrice, CompetitorPriceCheckDate from dbo.fn_getCompetitivePricingInfo (PBD.Item_Key, ST.Store_No)) AS Comp
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
        LEFT JOIN dbo.PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
		LEFT OUTER JOIN dbo.VendorCostHistory VCH (nolock) ON SIV.StoreItemVendorID=VCH.StoreItemVendorID
		LEFT OUTER JOIN dbo.ItemAttribute IA (nolock) ON IA.Item_Key = PBD.Item_Key
		LEFT JOIN dbo.NatItemClass CLS ON CLS.ClassID = I.ClassID
		LEFT JOIN dbo.NatItemCat CAT ON CAT.NatCatID = CLS.NatCatID
		LEFT JOIN dbo.LabelType LBL ON LBL.LabelType_ID = I.LabelType_ID
		LEFT JOIN dbo.PriceBatchHeader PBH (NOLOCK) ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
		WHERE VCH.VendorCostHistoryID = (Select Top 1 VCH2.VendorCostHistoryID 
					FROM dbo.VendorCostHistory VCH2 (nolock) 
					WHERE SIV.StoreItemVendorID = VCH2.StoreItemVendorID 
						AND @NOW BETWEEN VCH2.startdate AND VCH2.enddate
					ORDER BY VCH2.VendorCostHistoryID Desc) 
			-- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
		    AND (@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))
		ORDER BY 
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

		RAISERROR ('Planogram_GetAccessViaExtendedNonRegTagFile failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetAccessViaExtendedNonRegTagFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetAccessViaExtendedNonRegTagFile] TO [IRMAClientRole]
    AS [dbo];

