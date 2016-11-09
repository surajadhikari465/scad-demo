
CREATE PROCEDURE [dbo].[UpdateItemInfo]
    @Item_Key						int,
    @POS_Description				varchar(26), 
    @Item_Description				varchar(60), 
    @Sign_Description				varchar(60),    
	@Min_Temperature				smallint, 
    @Max_Temperature				smallint,
    @Average_Unit_Weight			decimal(9,4),     
    @Package_Desc1					decimal(9,4), 
    @Package_Desc2					decimal(9,4), 
    @Package_Unit_ID				int,     
    @Retail_Unit_ID					int, 
    @SubTeam_No						int, 
    @Brand_ID						int, 
    @Category_ID					int, 
    @Origin_ID						int, 
    @Retail_Sale					bit, 
    @Keep_Frozen					bit, 
    @Full_Pallet_Only				bit, 
    @Shipper_Item					bit, 
    @WFM_Item						bit, 
    @Units_Per_Pallet				int, 
    @Vendor_Unit_ID					int,
    @Distribution_Unit_ID			int,    
    @Tie							tinyint,
    @High							tinyint,
    @Yield							decimal(9,4),
    @NoDistMarkup					bit,
    @Organic						bit,
    @Refrigerated					bit,
    @Not_Available					bit,
    @Pre_Order						bit,
    @ItemType_ID					int,
    @Sales_Account					varchar(6),
    @HFM_Item						tinyint,   
    @Not_AvailableNote				varchar(255),
    @CountryProc_ID					int,
    @Manufacturing_Unit_ID			int,
    @EXEDistributed					bit,
    @NatClassID						int, 
	@DistSubTeam_No					int,
    @CostedByWeight					bit,
    @TaxClassID						int,
    @LabelType_ID					int,
    @User_ID						int,
	@User_ID_Date					varchar(255),
	@Manager_ID						tinyint,
    @Recall_Flag					bit,
    -- new param placed at the end so it can be optional
	-- to not break existing calls that do not pass in a value for it
    @ProdHierarchyLevel4_ID			int = NULL,
    @LockAuth_Flag					bit,
    @PurchaseThresholdCouponSubTeam bit,
    @PurchaseThresholdCouponAmount	smallmoney,
    @HandlingChargeOverride			smallmoney,
	@CatchweightRequired			bit,
	@COOL							bit,
	@BIO							bit,
	@Ingredient						bit,
    @SustainabilityRankingRequired	bit,
    @SustainabilityRankingID		int,
	@UseLastReceivedCost			bit,
	@GiftCard						bit = 0

AS 

-- **************************************************************************  
-- Procedure: UpdateItemInfo()  
--  
-- Description:  
-- This procedure is called to return item data to the order interface  
--  
-- Modification History:  
-- Date			Init		TFS		Comment  
-- 01.04.13		BS			8755	Codingn Standards. Updated extension to .sql.
--									Removed Discontinue_Item to account for 
--									schema change
-- 2013-04-14	KM			11774	Add PCMSGiftCard to the update set;
-- 2013-04-15   MZ          11918   Made @PCMSGiftCard optional by default it to 0
--                                  so that it won't break EIM Item Upload.
-- 2013-04-30	KM			11774	Rename @PCMSGiftCard to just @GiftCard;
-- 2014-08-28	KM			15398	Remove an item's identifiers from ValidatedScanCode
--									if the item is no longer Retail Sale;
-- 2015-09-16	KM			11732	Undo the last change;
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

	DECLARE @OrigTaxClassID int
	DECLARE @Sign_DescriptionUpdated bit
	DECLARE @Retail_Unit_IDUpdated bit
	DECLARE @Origin_IDUpdated bit
	DECLARE @LabelType_IDUpdated bit
	DECLARE @CountryProc_IDUpdated bit
	
	--Check if Tax Class was a changed field, if so then update the TaxClassModifiedDate field
	SELECT @OrigTaxClassID = TaxClassID FROM Item WHERE Item_Key = @Item_Key

	-- set the falgs to indicate whether interested fields got updated or not

	SELECT @Sign_DescriptionUpdated = CASE	WHEN @Sign_Description IS NOT NULL AND Sign_Description IS NOT NULL AND @Sign_Description <> Sign_Description THEN 1
											WHEN (@Sign_Description IS NULL AND Sign_Description IS NOT NULL) OR (@Sign_Description IS NOT NULL AND Sign_Description IS NULL) THEN 1
											ELSE 0
										END,
			@Retail_Unit_IDUpdated = CASE	WHEN @Retail_Unit_ID IS NOT NULL AND Retail_Unit_ID IS NOT NULL AND @Retail_Unit_ID <> Retail_Unit_ID THEN 1
											WHEN (@Retail_Unit_ID IS NULL AND Retail_Unit_ID IS NOT NULL) OR (@Retail_Unit_ID IS NOT NULL AND Retail_Unit_ID IS NULL) THEN 1
											ELSE 0
									 END,
			@Origin_IDUpdated = CASE	WHEN @Origin_ID IS NOT NULL AND Origin_ID IS NOT NULL AND @Origin_ID <> Origin_ID THEN 1
										WHEN (@Origin_ID IS NULL AND Origin_ID IS NOT NULL) OR (@Origin_ID IS NOT NULL AND Origin_ID IS NULL) THEN 1
										ELSE 0
								END,
			@LabelType_IDUpdated = CASE	WHEN @LabelType_ID IS NOT NULL AND LabelType_ID IS NOT NULL AND @LabelType_ID <> LabelType_ID THEN 1
										WHEN (@LabelType_ID IS NULL AND LabelType_ID IS NOT NULL) OR (@LabelType_ID IS NOT NULL AND LabelType_ID IS NULL) THEN 1
										ELSE 0
									END,
			@CountryProc_IDUpdated = CASE	WHEN @CountryProc_ID IS NOT NULL AND CountryProc_ID IS NOT NULL AND @CountryProc_ID <> CountryProc_ID THEN 1
											WHEN (@CountryProc_ID IS NULL AND CountryProc_ID IS NOT NULL) OR (@CountryProc_ID IS NOT NULL AND CountryProc_ID IS NULL) THEN 1
											ELSE 0
									 END
	FROM Item
	WHERE 
		Item_Key = @Item_Key
	
	IF @TaxClassID <> @OrigTaxClassId
		UPDATE Item SET TaxClassModifiedDate = GETDATE() WHERE Item_Key = @Item_Key

    UPDATE Item
    SET POS_Description = @POS_Description, 
        Item_Description = @Item_Description, 
        Sign_Description = @Sign_Description, 
        Min_Temperature = @Min_Temperature,
        Max_Temperature = @Max_Temperature,
        Package_Desc1 = @Package_Desc1,
        Package_Desc2 = @Package_Desc2,
        Average_Unit_Weight = @Average_Unit_Weight, 
        Package_Unit_ID = @Package_Unit_ID,
        Retail_Unit_ID = @Retail_Unit_ID,
        SubTeam_No = @SubTeam_No,
        Brand_ID = @Brand_ID,
        Category_ID = @Category_ID,
        ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID,
        Origin_ID = @Origin_ID,
        Retail_Sale = @Retail_Sale,
        Keep_Frozen = @Keep_Frozen,
        Full_Pallet_Only = @Full_Pallet_Only,
        Shipper_Item = @Shipper_Item,
        WFM_Item = @WFM_Item,
        Units_Per_Pallet = @Units_Per_Pallet,
        Vendor_Unit_ID = @Vendor_Unit_ID,
        Distribution_Unit_ID = @Distribution_Unit_ID,        
        Tie = @Tie,
        High = @High,
        Yield = @Yield,
        NoDistMarkup = @NoDistMarkup,
        Organic = @Organic,
        Refrigerated = @Refrigerated,
        Not_Available = @Not_Available,
        Pre_Order = @Pre_Order,
        ItemType_ID = @ItemType_ID,
        Sales_Account = @Sales_Account,
        HFM_Item = @HFM_Item,
        Not_AvailableNote = @Not_AvailableNote,
        CountryProc_ID = @CountryProc_ID,
        Manufacturing_Unit_ID = @Manufacturing_Unit_ID,
        EXEDistributed = @EXEDistributed,
        ClassID = @NatClassID, 
		DistSubTeam_No = @DistSubTeam_No,
        CostedByWeight = @CostedByWeight,
        TaxClassID = @TaxClassID,
        LabelType_ID = @LabelType_ID,
        User_ID = @User_ID,
		User_ID_Date = @User_ID_Date,
		Manager_ID = @Manager_ID,
		Recall_Flag = @Recall_Flag,
		LockAuth = @LockAuth_Flag,
		PurchaseThresholdCouponSubTeam = @PurchaseThresholdCouponSubTeam,
		PurchaseThresholdCouponAmount = @PurchaseThresholdCouponAmount,
		CatchweightRequired = @CatchweightRequired,
		COOL = @COOL,
		BIO = @BIO,
		Ingredient = @Ingredient,
		SustainabilityRankingRequired = @SustainabilityRankingRequired,
        SustainabilityRankingID = @SustainabilityRankingID,
		LastModifiedUser_ID = @User_ID,
		LastModifiedDate = @User_ID_Date,
		UseLastReceivedCost = @UseLastReceivedCost,
		GiftCard = @GiftCard

    FROM 
		dbo.Item (rowlock)
    
	WHERE 
		Item_Key = @Item_Key
    
    --Update the Facility Handling Charge Override in the ItemVendor table
    UPDATE ItemVendor SET 
		CaseDistHandlingChargeOverride = @HandlingChargeOverride
    WHERE 
		Item_Key = @Item_Key AND Vendor_ID IN	(
													SELECT IV.Vendor_Id	FROM 
														dbo.ItemVendor	IV	(NOLOCK)
														JOIN dbo.Vendor V	(NOLOCK) ON V.Vendor_Id = IV.Vendor_Id
														JOIN dbo.Store	S	(NOLOCK) ON S.Store_No	= V.Store_No
													WHERE 
														IV.Item_Key = @Item_Key AND S.Distribution_Center = 1
												)

	---- Queue event for mammoth to refresh its data.
	IF	@OrigTaxClassID  = 1 or
		@Sign_DescriptionUpdated = 1 or
		@Retail_Unit_IDUpdated = 1 or
		@Origin_IDUpdated = 1 or 
		@LabelType_IDUpdated  =  1 or
		@CountryProc_IDUpdated  = 1
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
	END

	SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemInfo] TO [IRMAReportsRole]
    AS [dbo];

