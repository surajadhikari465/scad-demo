SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.ItemListReport') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.ItemListReport
GO


CREATE PROCEDURE dbo.ItemListReport
    @Store_No int,
    @Item_Description varchar(255),
    @Identifier varchar(255),
    @Item_ID varchar(255),
    @Discontinue_Item tinyint,
    @WFM_Item tinyint,
    @SubTeam_No int,
    @VendorName varchar(255)
AS

-- **************************************************************************
-- Procedure: GetStoreItemSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ItemCatalogLib project within IRMA Client solution
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item reference by adding
--								DiscontinueItem reference in ItemVendor join
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT DISTINCT Item_Description, Identifier, Package_Desc1, Package_Desc2, ItemUnit.Unit_Name, 
        SubTeam_Name, 
        ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @Store_No, Item.SubTeam_No, GETDATE()), 0) as AvgCost, 
        Multiple, Price,
        ROUND(dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * CasePriceDiscount * Package_Desc1, 2) As Case_Price
    FROM Item (nolock)
		LEFT JOIN ItemUnit (nolock) ON ItemUnit.Unit_ID = Item.Package_Unit_ID
		INNER JOIN SubTeam (nolock) ON SubTeam.SubTeam_No = Item.SubTeam_No
		INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
		INNER JOIN Price (nolock) ON Item.Item_Key = Price.Item_Key AND @Store_No = Price.Store_No
		INNER JOIN StoreSubTeam SST (nolock) ON SST.Store_No = Price.Store_No AND SST.SubTeam_No = Item.SubTeam_No
		LEFT JOIN
			(SELECT IV.Item_Key, IV.Vendor_ID, Item_ID, SIV.DiscontinueItem
			 FROM ItemVendor IV (nolock)
			 INNER JOIN
				StoreItemVendor SIV (nolock)
				ON SIV.Store_No = @Store_No AND SIV.Item_Key = IV.Item_Key AND SIV.Vendor_ID = IV.Vendor_ID
			 WHERE @CurrDate < ISNULL(IV.DeleteDate, DATEADD(day, 1, @CurrDate))
			 AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
			) As ItemVendor
			ON Item.Item_Key = ItemVendor.Item_Key
		LEFT JOIN Vendor (nolock) ON ItemVendor.Vendor_ID = Vendor.Vendor_ID
    WHERE 
		Deleted_Item = 0
		AND CompanyName LIKE ISNULL('%' + @VendorName + '%', CompanyName)
		AND Item_Description LIKE ISNULL('%' + @Item_Description + '%', Item_Description)
		AND Identifier LIKE ISNULL(@Identifier + '%', Identifier)
		AND ISNULL(Item_ID, '') = ISNULL(@Item_ID, ISNULL(Item_ID, ''))
		AND ItemVendor.DiscontinueItem = ISNULL(@Discontinue_Item, ItemVendor.DiscontinueItem)
		AND WFM_Item = ISNULL(@WFM_Item, WFM_Item)
		AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
    ORDER BY Item_Description

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


