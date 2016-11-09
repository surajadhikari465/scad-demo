
CREATE PROCEDURE [dbo].[InsertUpdateItemOverride]
    @Item_Key						INT ,
    @StoreJurisdictionID			INT ,
    @Item_Description				VARCHAR(60) ,
    @Sign_Description				VARCHAR(60) ,
    @Package_Desc1					DECIMAL(9, 4) ,
    @Package_Desc2					DECIMAL(9, 4) ,
    @Package_Unit_ID				INT ,
    @Retail_Unit_ID					INT ,
    @Vendor_Unit_ID					INT ,
    @Distribution_Unit_ID			INT ,
    @POS_Description				VARCHAR(26) ,
    @Food_Stamps					BIT ,
    @Price_Required					BIT ,
    @Quantity_Required				BIT ,
    @Manufacturing_Unit_ID			INT ,
    @QtyProhibit					BIT ,
    @GroupList						INT ,
    @Case_Discount					BIT ,
    @Coupon_Multiplier				BIT ,
    @Misc_Transaction_Sale			SMALLINT ,
    @Misc_Transaction_Refund		SMALLINT ,
    @Ice_Tare						INT ,
    
	-- new for 4.8
	@Brand_ID						INT ,
    @Origin_Id						INT ,
    @CountryProc_Id					INT ,
    @SustainabilityRankingRequired	BIT ,
    @SustainabilityRankingID		INT ,
    @LabelType_ID					INT ,
    @CostedByWeight					BIT ,
    @Average_Unit_Weight			DECIMAL(9, 4) ,
    @Ingredient						BIT ,
    @Recall_Flag					BIT ,
    @LockAuth						BIT ,
    @Not_Available					BIT ,
    @Not_AvailableNote				VARCHAR(255) ,
    @FSA_Eligible					BIT ,
    @Product_Code					VARCHAR(15) ,
    @Unit_Price_Category			INT

AS 

-- ****************************************************************************************************************
-- Procedure: InsertUpdateItemOverride()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- This procedure updates (or inserts) data in the ItemOverride table.  It is called from StoreJurisdictionDAO.vb
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-02	KM		9251	Robin added all of the new override columns so that EIM would work.  I put this template here.
-- 2015-10-05   MZ     16646    Stage IRMA Item Locale Events for Alt Jurisdiction Changes 
-- ****************************************************************************************************************

DECLARE @OriginalCountryProc_Id int, 
		@OriginalOrigin_Id int,
		@OriginalRetailSize decimal(9,4), --[Package_Desc2]
		@OriginalRetailUnit int,          --[Package_Unit_ID]
		@OriginalRetailUOM int,           --[Retail_Unit_ID]
		@OriginalSignDescription varchar(60)

DECLARE @Error_No int

BEGIN

	-- Create the ItemOverride record if it does not already exist.
    IF NOT EXISTS(SELECT * FROM ItemOverride WHERE Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID)
    BEGIN
        INSERT INTO ItemOverride
		( 
			Item_Key ,
            StoreJurisdictionID ,
            Item_Description ,
            Sign_Description ,
            Package_Desc1 ,
            Package_Desc2 ,
            Package_Unit_ID ,
            Retail_Unit_ID ,
            Vendor_Unit_ID ,
            Distribution_Unit_ID ,
            POS_Description ,
            Food_Stamps ,
            Price_Required ,
            Quantity_Required ,
            Manufacturing_Unit_ID ,
            QtyProhibit ,
            GroupList ,
            Case_Discount ,
            Coupon_Multiplier ,
            Misc_Transaction_Sale ,
            Misc_Transaction_Refund ,
            Ice_Tare ,
                  
			-- new for 4.8
			Brand_Id ,
            Origin_Id ,
            CountryProc_Id ,
            SustainabilityRankingRequired ,
            SustainabilityRankingID ,
            LabelType_ID ,
            CostedByWeight ,
            Average_Unit_Weight ,
            Ingredient ,
            Recall_Flag ,
            LockAuth ,
            Not_Available ,
            Not_AvailableNote ,
            FSA_Eligible ,
            Product_Code ,
            Unit_Price_Category                           
		)
        VALUES  
		( 
			@Item_Key ,
            @StoreJurisdictionID ,
            @Item_Description ,
            @Sign_Description ,
            @Package_Desc1 ,
            @Package_Desc2 ,
            @Package_Unit_ID ,
            @Retail_Unit_ID ,
            @Vendor_Unit_ID ,
            @Distribution_Unit_ID ,
            @POS_Description ,
            @Food_Stamps ,
            @Price_Required ,
            @Quantity_Required ,
            @Manufacturing_Unit_ID ,
            @QtyProhibit ,
            @GroupList ,
            @Case_Discount ,
            @Coupon_Multiplier ,
            @Misc_Transaction_Sale ,
            @Misc_Transaction_Refund ,
            @Ice_Tare ,
            
			-- new for 4.8
			@Brand_Id ,
            @Origin_Id ,
            @CountryProc_Id ,
            @SustainabilityRankingRequired ,
            @SustainabilityRankingID ,
            @LabelType_ID ,
            @CostedByWeight ,
            @Average_Unit_Weight ,
            @Ingredient ,
            @Recall_Flag ,
            @LockAuth ,
            @Not_Available ,
            @Not_AvailableNote ,
            @FSA_Eligible ,
            @Product_Code ,
            @Unit_Price_Category                           
		)

		SELECT @Error_No = @@ERROR

		IF @Error_No = 0
			If (@CountryProc_Id is not null or @Origin_Id is not null or @Package_Desc2 is not null or @Package_Unit_ID is not null
			    or @Retail_Unit_ID is not null or @Sign_Description is not null)
				EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, @StoreJurisdictionID
	END

	-- Otherwise, update the ItemOverride data.
	ELSE
		BEGIN
			SELECT
				@OriginalCountryProc_Id = ior.CountryProc_Id,
				@OriginalOrigin_Id = ior.Origin_Id,
				@OriginalRetailSize = ior.Package_Desc2,
				@OriginalRetailUnit = ior.Package_Unit_ID,
				@OriginalRetailUOM = ior.Retail_Unit_ID,
				@OriginalSignDescription = ior.Sign_Description
			  FROM
				ItemOverride ior
			 WHERE   
				ior.Item_Key = @Item_Key
				AND ior.StoreJurisdictionID = @StoreJurisdictionID

			UPDATE  ItemOverride
			SET     
				Item_Description				= @Item_Description ,
                Sign_Description				= @Sign_Description ,
                Package_Desc1					= @Package_Desc1 ,
                Package_Desc2					= @Package_Desc2 ,
                Package_Unit_ID					= @Package_Unit_ID ,
                Retail_Unit_ID					= @Retail_Unit_ID ,
                Vendor_Unit_ID					= @Vendor_Unit_ID ,
                Distribution_Unit_ID			= @Distribution_Unit_ID ,
                POS_Description					= @POS_Description ,
                Food_Stamps						= @Food_Stamps ,
                Price_Required					= @Price_Required ,
                Quantity_Required				= @Quantity_Required ,
                Manufacturing_Unit_ID			= @Manufacturing_Unit_ID ,
                QtyProhibit						= @QtyProhibit ,
                GroupList						= @GroupList ,
                Case_Discount					= @Case_Discount ,
                Coupon_Multiplier				= @Coupon_Multiplier ,
                Misc_Transaction_Sale			= @Misc_Transaction_Sale ,
                Misc_Transaction_Refund			= @Misc_Transaction_Refund ,
                Ice_Tare						= @Ice_Tare ,
                Brand_Id						= @Brand_Id ,
                Origin_Id						= @Origin_Id ,
                CountryProc_Id					= @CountryProc_Id ,
                SustainabilityRankingRequired	= @SustainabilityRankingRequired ,
                SustainabilityRankingID			= @SustainabilityRankingID ,
                LabelType_ID					= @LabelType_ID ,
                CostedByWeight					= @CostedByWeight ,
                Average_Unit_Weight				= @Average_Unit_Weight ,
                Ingredient						= @Ingredient ,
                Recall_Flag						= @Recall_Flag ,
                LockAuth						= @LockAuth ,
                Not_Available					= @Not_Available ,
                Not_AvailableNote				= @Not_AvailableNote ,
                FSA_Eligible					= @FSA_Eligible ,
                Product_Code					= @Product_Code ,
                Unit_Price_Category				= @Unit_Price_Category
			WHERE   
				Item_Key = @Item_Key
				AND StoreJurisdictionID = @StoreJurisdictionID
			
			SELECT @Error_No = @@ERROR


			IF @Error_No = 0
				BEGIN
					IF ((@OriginalCountryProc_Id is not NULL AND @OriginalCountryProc_Id <> @CountryProc_Id)
					 OR (@OriginalCountryProc_Id is NULL AND @CountryProc_Id is not NULL)
					 OR (@OriginalOrigin_Id is not NULL AND @OriginalOrigin_Id <> @Origin_Id)
					 OR (@OriginalOrigin_Id is NULL AND @Origin_Id is not NULL)
					 OR (@OriginalRetailSize is not NULL AND @OriginalRetailSize <> @Package_Desc2)
					 OR (@OriginalRetailSize is NULL AND @Package_Desc2 is not NULL)
					 OR (@OriginalRetailUnit is not NULL AND @OriginalRetailUnit <> @Package_Unit_ID)
					 OR (@OriginalRetailUnit is NULL AND @Package_Unit_ID is not NULL)
					 OR (@OriginalRetailUOM is not NULL AND @OriginalRetailUOM <> @Retail_Unit_ID)
					 OR (@OriginalRetailUOM is NULL AND @Retail_Unit_ID is not NULL)
					 OR (@OriginalSignDescription is not NULL AND @OriginalSignDescription <> @Sign_Description)
					 OR (@OriginalSignDescription is NULL AND @Sign_Description is not NULL))	
						EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, @StoreJurisdictionID
				END
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertUpdateItemOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertUpdateItemOverride] TO [IRMASLIMRole]
    AS [dbo];

