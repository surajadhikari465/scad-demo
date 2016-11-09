CREATE PROCEDURE dbo.MarginByVendorReport
    @Store_No int,
    @Vendor_ID int
AS

-- **************************************************************************
-- Procedure: MarginByVendorReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item filter in WHERE clause to
--					account for schema change. Renamed file to .sql. Coding Standards.
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT
        Price.Store_No,
		Price.Multiple,
		Price.Price, 
    	ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Store.Store_No, Item.SubTeam_No, GETDATE()), 0) AS AvgCost, 
        Item.Item_Description,
        ItemIdentifier.Identifier,
        ItemBrand.Brand_Name
    FROM
        StoreItemVendor SIV			(nolock)
		INNER JOIN Price			(nolock) ON Price.Item_Key			= SIV.Item_Key AND Price.Store_No = SIV.Store_No
		INNER JOIN Item				(nolock) ON Item.Item_Key			= Price.Item_Key
		INNER JOIN ItemIdentifier	(nolock) ON ItemIdentifier.Item_Key = Item.Item_Key
												AND ItemIdentifier.Default_Identifier = 1
		INNER JOIN Store			(nolock) ON Store.Store_No			= Price.Store_No
		LEFT JOIN ItemBrand			(nolock) ON Item.Brand_ID			= ItemBrand.Brand_ID
    WHERE
        Retail_Sale				= 1
		AND Deleted_Item		= 0
		AND SIV.DiscontinueItem	= 0 
        AND Price.Price			> 0
        AND Price.Store_No		= ISNULL(@Store_No, Price.Store_No)
        AND (WFM_Store = 1 OR Mega_Store = 1)
        AND SIV.Vendor_ID		= @Vendor_ID
        AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
    ORDER BY
        Price.Store_No,
        ItemIdentifier.Identifier

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginByVendorReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginByVendorReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginByVendorReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginByVendorReport] TO [IRMAReportsRole]
    AS [dbo];

