SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'WFMM_GetItem')
	BEGIN
		DROP Procedure [dbo].WFMM_GetItem
	END
GO

CREATE PROCEDURE dbo.WFMM_GetItem
	@Store_No				int,
    @TransferToSubTeam_No	int,
    @User_ID				int,
	@Item_Key				int,
	@Identifier				varchar(13)

AS

-- **************************************************************************
-- Procedure: WFMM_GetItem()
--    Author: Billy Blackerby
--      Date: 05.05.11
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item data to the 
-- order interface
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 05.05.11		BBB   	xxxxx	Creation
-- 01.03.12		FA		3476	Set @IdentifierItem_Key to Item_Key from 
--								ItemIdentifier table
-- 05.21.12		RE		xxxxx	Performance fix: Modified @item_key / @identifier 
--								search logic to prevent multiple table scans. 
-- 08.08.12     FA      7399    Fixed the code to get the correct item_key 
-- 09.19.12		hk		7427    Return vendor ID
-- 09.22.12     FA      8096    Fixed the bug to return alternate identifiers
-- 10.16.12		HK		7419	Return store vendor ID

-- 12.10.12     FA      8795    Add two fields for retail unit name and ID
-- 2013/01/04	KM		9251	Check ItemOverride for new 4.8 override values (Not_Available, CostedByWeight);
-- 2013/01/11	DN		8755	Reference Reference the field DiscontinueItem 
--								in the StoreItemVendor table instead of the 
--								Discontinue_Item field in the Item table.
-- 01.16.13		HK		9779    Return IsItemAuthorized
-- 07-03-2015   FA      9488    Added Brand field
-- **************************************************************************

BEGIN
    SET NOCOUNT ON  

	--**************************************************************************
	--Declare variables
	--**************************************************************************
    DECLARE @error_no			int  
    DECLARE @IsPBP				bit  
    DECLARE @IsSuperUser		bit  

	
	--**************************************************************************
	--Set variables
	--**************************************************************************	
	SET @error_no = 0      
  
    SELECT  @IsPBP = PriceBatchProcessor ,
            @IsSuperUser = SuperUser
    FROM    Users
    WHERE   User_Id = @User_Id  
    
	IF @Item_Key IS NULL
		SELECT @Item_Key = item_key 
		FROM dbo.ItemIdentifier 
		WHERE identifier = @Identifier and deleted_identifier = 0 and remove_identifier = 0
		
	
	--**************************************************************************
	--Update Price.Scan columns
	--**************************************************************************	
    IF @IsPBP = 1 AND @IsSuperUser = 0  
        UPDATE  Price
        SET     LastScannedUserId_DTS = @User_Id ,
                LastScannedDate_DTS = GETDATE()
        WHERE   Item_Key = @Item_Key
                AND Store_No = @Store_No  
    ELSE  
        UPDATE  Price
        SET     LastScannedUserId_NonDTS = @User_Id ,
                LastScannedDate_NonDTS = GETDATE()
        WHERE   Item_Key = @Item_Key
                AND Store_No = @Store_No  
  
	--**************************************************************************
	--Main Query
	--**************************************************************************	
    IF @error_no = 0  
		BEGIN      
			SELECT  
			   [Item_Key]				= i.Item_Key,  
			   [Identifier]				= ii.Identifier,  
			   [Item_Description]		= ISNULL(ior.Item_Description, i.Item_Description),  
			   [POS_Description]		= ISNULL(ior.POS_Description, i.POS_Description),  
			   [RetailSubTeam_No]		= i.SubTeam_No,  
			   [RetailSubTeam_Name]		= st.SubTeam_Name,
			   [TransferToSubTeam_Name] = CASE
											WHEN ISNULL(@TransferToSubTeam_No, i.SubTeam_No) <> i.SubTeam_No THEN
												(SELECT SubTeam_Name FROM SubTeam (nolock) WHERE SubTeam_No = @TransferToSubTeam_No)
											ELSE
												NULL
										  END,
			   [Package_Desc1]			= ISNULL(ior.Package_Desc1, i.Package_Desc1),  
			   [Package_Desc2]			= ISNULL(ior.Package_Desc2, i.Package_Desc2),  
			   [Package_Unit_Abbr]		= pu.Unit_Abbreviation,  
			   [Not_Available]			= ISNULL(ior.Not_Available, i.Not_Available),  
			   [Discontinue_Item]		= siv.DiscontinueItem,  
			   [Sign_Description]		= ISNULL(ior.Sign_Description, i.Sign_Description),  
			   [Sold_By_Weight]			= ISNULL(ru.Weight_Unit, 0),  
			   [WFM_Item]				= i.WFM_Item,  
			   [HFM_Item]				= i.HFM_Item,  
			   [Retail_Sale]			= i.Retail_Sale,  
			   [Vendor_Unit_ID]			= ISNULL(ior.Vendor_Unit_ID, i.Vendor_Unit_ID),  
			   [Vendor_Unit_Name]		= vu.Unit_Abbreviation,  
			   [Multiple]				= p.Multiple,  
			   [Price]					= p.Price,  
			   [PriceChgTypeDesc]		= pct.PriceChgTypeDesc,
			   [On_Sale]				= pct.On_Sale,  
			   [Sale_Start_Date]		= p.Sale_Start_Date,  
			   [Sale_End_Date]			= p.Sale_End_Date,  
			   [Sale_Multiple]			= p.Sale_Multiple,  
			   [Sale_Price]				= p.Sale_Price,  
			   [Sale_Earned_Disc1]		= p.Sale_Earned_Disc1,  
			   [Sale_Earned_Disc2]		= p.Sale_Earned_Disc2,  
			   [Sale_Earned_Disc3]		= p.Sale_Earned_Disc3,  
			   [AvgCost]				= (SELECT dbo.fn_AvgCostHistory(i.Item_Key, s.Store_No, i.SubTeam_No, GETDATE())),  
			   [CanInventory]			= CASE 
											WHEN EXISTS (SELECT Subteam_No FROM SubTeam (nolock) WHERE SubTeam_No = ISNULL(@TransferToSubTeam_No, i.SubTeam_No) AND SubTeamType_ID IN (2,3,4)) OR (st.SubTeamType_ID = 4) OR (i.SubTeam_No = ISNULL(@TransferToSubTeam_No, i.SubTeam_No))  THEN 
												1 
											ELSE 
												0 
										  END,  
			   [IsSellable]				= CASE 
											WHEN (s.WFM_Store = 1 AND i.WFM_Item = 1) OR (s.Mega_Store = 1 AND i.HFM_Item = 1) OR (s.WFM_Store = 0 AND s.Mega_Store = 0) THEN
												1 
											ELSE 
												0
										  END,
			   [VendorName]				= v.CompanyName,
			   [CostedByWeight]			= ISNULL(ior.CostedByWeight, i.CostedByWeight),
			   [Vendor_ID]				= v.Vendor_ID,
			   [StoreVendor_ID]			= sv.Vendor_ID,
			   [Retail_Unit_Name]       = ru.Unit_Name,
			   [Retail_Unit_ID]		    = ru.Unit_ID,
			   [IsItemAuthorized]		= (SELECT dbo.fn_IsItemAuthorizedForStore(i.Item_Key, s.Store_No)),
			   [Brand]                  = ib.Brand_Name
			FROM 
				Item						(nolock) i 
				INNER JOIN  ItemIdentifier	(nolock) ii		ON	ii.Item_Key										= i.Item_Key 
				INNER JOIN  ItemBrand       (nolock) ib     ON  i.Brand_ID                                      = ib.Brand_ID																																	
				INNER JOIN  Price			(nolock) p		ON	p.Item_Key										= i.Item_Key 
															AND p.Store_No										= @Store_No
				INNER JOIN  PriceChgType	(nolock) pct	ON	pct.PriceChgTypeID								= p.PriceChgTypeID  
				INNER JOIN  SubTeam			(nolock) st		ON  st.SubTeam_No									= i.SubTeam_No  
				INNER JOIN  Store			(nolock) s		ON	s.Store_No										= p.Store_No  
				INNER JOIN  StoreItemVendor (nolock) siv	ON	siv.Item_Key									= i.Item_Key 
															AND siv.Store_No									= @Store_No
															AND siv.PrimaryVendor								= 1
				INNER JOIN  Vendor			(nolock) v		ON	v.Vendor_ID										= siv.Vendor_ID
				INNER JOIN  Vendor			(nolock) sv		ON	sv.Store_no										= s.Store_No
				LEFT JOIN	ItemOverride	(nolock) ior	ON  ior.Item_Key									= i.Item_Key 
															AND ior.StoreJurisdictionID							= s.StoreJurisdictionID
				LEFT JOIN   ItemUnit		(nolock) pu		ON ISNULL(ior.Package_Unit_Id, i.Package_Unit_ID)	= pu.Unit_ID  
				LEFT JOIN	ItemUnit		(nolock) ru		ON ISNULL(ior.Retail_Unit_id, i.Retail_Unit_ID)		= ru.Unit_ID  
				LEFT JOIN   ItemUnit		(nolock) vu		ON ISNULL(ior.Vendor_Unit_Id, i.Vendor_Unit_ID)		= vu.Unit_ID

			WHERE 
				i.Deleted_Item				= 0 
				AND i.Remove_Item			= 0  
				AND ii.Deleted_Identifier	= 0
				AND ii.Remove_Identifier	= 0
				AND i.Item_Key				= @Item_Key
				AND ii.Identifier			= ISNULL(@Identifier, ii.Identifier)

			SELECT @error_no = @@ERROR  
		END  
  
    SET NOCOUNT OFF
END
GO
