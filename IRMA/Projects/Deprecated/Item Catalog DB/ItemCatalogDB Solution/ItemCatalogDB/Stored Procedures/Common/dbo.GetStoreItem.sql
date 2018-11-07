IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetStoreItem' 
	   AND 	  type = 'P')
    DROP PROCEDURE dbo.GetStoreItem
GO

CREATE PROCEDURE dbo.GetStoreItem 
	@Store_No					int,
    @TransferToSubTeam_No		int,
    @User_ID					int,
    @Item_Key					int OUTPUT,
    @Identifier					varchar(13) OUTPUT,
    @Price						smallmoney OUTPUT,
    @Multiple					tinyint OUTPUT,
    @On_Sale					bit OUTPUT,
    @Sale_Start_Date			smalldatetime OUTPUT,
    @Sale_End_Date				smalldatetime OUTPUT,
    @Sale_Multiple				tinyint OUTPUT,
    @Sale_Price					smallmoney OUTPUT,
    @Sale_Earned_Disc1			tinyint OUTPUT,
    @Sale_Earned_Disc2			tinyint OUTPUT,
    @Sale_Earned_Disc3			tinyint OUTPUT,
    @AvgCost					smallmoney OUTPUT,
    @CanInventory				bit OUTPUT,
    @IsSellable					bit OUTPUT,
	@Item_Description			varchar(255) OUTPUT,
	@POS_Description			varchar(255) OUTPUT,
    @RetailSubTeam_No			int OUTPUT,
	@RetailSubTeam_Name			varchar(255) OUTPUT,
    @TransferToSubTeam_Name		varchar(255) OUTPUT,
    @Package_Desc1				int OUTPUT,
	@Package_Desc2				decimal (9,4) OUTPUT,
    @Package_Unit_Abbr			varchar(255) OUTPUT,
	@Not_Available				bit OUTPUT,
    @Discontinue_Item			bit OUTPUT,
    @Sign_Description			varchar(255) OUTPUT,
    @Sold_By_Weight				bit OUTPUT,
    @WFM_Item					bit OUTPUT,
    @HFM_Item					bit OUTPUT,
    @Retail_Sale				bit OUTPUT,
    @Vendor_Unit_ID				int OUTPUT,
    @Vendor_Unit_Name			varchar(255) OUTPUT,
    @PriceChgTypeDesc			varchar(5) OUTPUT,
    @VendorName					varchar(50) OUTPUT
AS

-- ****************************************************************************************************************
-- Procedure: GetStoreItem()
--    Author: unknown
--      Date: unknown
--
-- Description:
--	Called from ItemCatalogLib\StoreItem.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2009/09/21	RE		11133	Added StoreJurisdiction support
-- 2012/12/19	KM		8747	Get the Not_Available value from ItemOverride if appropriate;
-- 01/14/2013	BAS		8755	Update i.Discontinue_Item reference to dbo.fn_GetDiscontinueStatus
--								to account for schema change
-- ****************************************************************************************************************

BEGIN  
    SET NOCOUNT ON  
  
    DECLARE @error_no int  
    SET @error_no = 0  
  
    DECLARE @IdentifierItem_Key int  
    DECLARE @IsPBP bit  
    DECLARE @IsSuperUser bit  
      
    IF @Item_Key IS NULL  
		BEGIN  
			SELECT @IdentifierItem_Key = (	SELECT TOP 1 Item_Key   
											FROM   
												ItemIdentifier   
											WHERE   
												Deleted_Identifier = 0   
												AND Remove_Identifier = 0   
												AND Identifier = @Identifier )  
	  
			SELECT @error_no = @@ERROR  
		END  
  
    SELECT @IsPBP = PriceBatchProcessor FROM Users WHERE User_Id = @User_Id  
    SELECT @IsSuperUser = SuperUser FROM Users WHERE User_Id = @User_Id  
      
    IF @IsPBP = 1 AND @IsSuperUser = 0  
		UPDATE Price SET LastScannedUserId_DTS = @User_Id, LastScannedDate_DTS = GETDATE() WHERE Item_Key = ISNULL(@Item_Key, @IdentifierItem_Key) AND Store_No = @Store_No  
    ELSE  
		UPDATE Price SET LastScannedUserId_NonDTS = @User_Id, LastScannedDate_NonDTS = GETDATE() WHERE Item_Key = ISNULL(@Item_Key, @IdentifierItem_Key) AND Store_No = @Store_No  
  
    IF @error_no = 0  
		BEGIN      
			SELECT  
			   @Item_Key = ISNULL(OVR.Item_Key, Item.Item_Key),  
			   @Identifier = Identifier,  
			   @Item_Description = ISNULL(OVR.Item_Description, Item.Item_Description),  
			   @POS_Description = ISNULL(OVR.POS_Description, Item.POS_Description),  
			   @RetailSubTeam_No = Item.SubTeam_No,  
			   @RetailSubTeam_Name = SubTeam_Name,  
			   @Package_Desc1 = ISNULL(OVR.Package_Desc1, Item.Package_Desc1),  
			   @Package_Desc2 = ISNULL(OVR.Package_Desc2, Item.Package_Desc2),  
			   @Package_Unit_Abbr = PU.Unit_Abbreviation,  
			   @Not_Available = ISNULL(OVR.Not_Available, Item.Not_Available),
			   @Discontinue_Item = dbo.fn_GetDiscontinueStatus(Item.Item_Key, @Store_No, NULL),  
			   @Sign_Description = ISNULL(OVR.Sign_Description, Item.Sign_Description),  
			   @Sold_By_Weight = ISNULL(RU.Weight_Unit, 0),  
			   @WFM_Item = WFM_Item,  
			   @HFM_Item = HFM_Item,  
			   @Retail_Sale = Retail_Sale,  
			   @Vendor_Unit_ID = ISNULL(OVR.Vendor_Unit_ID, Item.Vendor_Unit_ID),  
			   @Vendor_Unit_Name = VU.Unit_Abbreviation,  
			   @Multiple = Multiple,  
			   @Price = Price,  
			   @PriceChgTypeDesc = PCT.PriceChgTypeDesc,
			   @On_Sale = On_Sale,  
			   @Sale_Start_Date = Sale_Start_Date,  
			   @Sale_End_Date = Sale_End_Date,  
			   @Sale_Multiple = Sale_Multiple,  
			   @Sale_Price = Sale_Price,  
			   @Sale_Earned_Disc1 = Sale_Earned_Disc1,  
			   @Sale_Earned_Disc2 = Sale_Earned_Disc2,  
			   @Sale_Earned_Disc3 = Sale_Earned_Disc3,  
			   @AvgCost = (SELECT dbo.fn_AvgCostHistory(@Item_Key, @Store_No, ISNULL(@TransferToSubTeam_No, Item.SubTeam_No), GETDATE())),  
			   @CanInventory = CASE WHEN EXISTS (SELECT Subteam_No FROM SubTeam (nolock) WHERE SubTeam_No = ISNULL(@TransferToSubTeam_No, Item.SubTeam_No) AND SubTeamType_ID IN (2,3,4))  
												 OR (SubTeam.SubTeamType_ID = 4) OR (Item.SubTeam_No = ISNULL(@TransferToSubTeam_No, Item.SubTeam_No))  
							   THEN 1 ELSE 0 END,  
			   @IsSellable = CASE WHEN (WFM_Store = 1 AND WFM_Item = 1) OR (Mega_Store = 1 AND HFM_Item = 1) OR (WFM_Store = 0 AND Mega_Store = 0) THEN 1 ELSE 0 END,
			   @VendorName = V.CompanyName
			   
			FROM 
				Item (nolock)  
				LEFT OUTER JOIN  ItemOverride OVR ON  Item.Item_Key = OVR.Item_Key AND OVR.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store WHERE Store_No = @Store_No)                  
				INNER JOIN  Price (nolock) ON Item.Item_Key = Price.Item_Key AND Price.Item_Key = ISNULL(@Item_Key, @IdentifierItem_Key)  AND Price.Store_No = @Store_No  
				INNER JOIN  PriceChgType PCT (nolock) ON Price.PriceChgTypeID = PCT.PriceChgTypeID  
				INNER JOIN  SubTeam (nolock) ON SubTeam.SubTeam_No = Item.SubTeam_No  
				INNER JOIN  Store (nolock) ON Price.Store_No = Store.Store_No  
				INNER JOIN  ItemIdentifier II (NOLOCK) ON II.Item_Key = Item.Item_Key AND Default_Identifier = 1  
				INNER JOIN  StoreItemVendor SIV (nolock) ON SIV.Item_Key = Item.Item_Key AND SIV.Store_No = @Store_No
				INNER JOIN  Vendor V (nolock) ON V.Vendor_ID = SIV.Vendor_ID
				LEFT JOIN   ItemUnit PU (nolock) ON ISNULL(OVR.Package_Unit_Id, Item.Package_Unit_ID) = PU.Unit_ID  
				LEFT JOIN	ItemUnit RU (nolock) ON ISNULL(OVR.Retail_Unit_id, Item.Retail_Unit_ID) = RU.Unit_ID  
				LEFT JOIN   ItemUnit VU (nolock) ON ISNULL(OVR.Vendor_Unit_Id, Item.Vendor_Unit_ID) = VU.Unit_ID  
			WHERE Deleted_Item = 0 AND Remove_Item = 0  
	      
			SELECT @error_no = @@ERROR  
		END  
  
    IF @error_no = 0 AND ISNULL(@TransferToSubTeam_No, @RetailSubTeam_No) <> @RetailSubTeam_No  
		BEGIN  
			SELECT @TransferToSubTeam_Name = SubTeam_Name FROM SubTeam (nolock) WHERE SubTeam_No = @TransferToSubTeam_No  
			SELECT @error_no = @@ERROR  
		END  
  
    IF @error_no <> 0  
		BEGIN  
			IF @@TRANCOUNT <> 0  
				ROLLBACK TRAN  
				
			DECLARE @Severity smallint  
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)  
			RAISERROR ('GetStoreItem failed with @@ERROR: %d', @Severity, 1, @error_no)  
		END  
  
    SET NOCOUNT OFF  
END
GO