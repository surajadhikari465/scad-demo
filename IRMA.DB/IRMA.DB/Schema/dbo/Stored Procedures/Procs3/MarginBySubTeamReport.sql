CREATE PROCEDURE dbo.MarginBySubTeamReport
    @Store_No int, 
    @SubTeam_No int
AS

-- **************************************************************************
-- Procedure: MarginBySubTeamReport()
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
	SELECT
        SubTeam_Name,
        Item_Description,
        Price.Store_No, Price.Multiple, Price.Price, 
    	ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Store.Store_No, Item.SubTeam_No, GETDATE()), 0) AS AvgCost, 
        Brand_Name,
        Identifier
    FROM
		Item (nolock)
		INNER JOIN SubTeam			(nolock) ON SubTeam.SubTeam_No	= Item.SubTeam_No
		LEFT JOIN ItemBrand			(nolock) ON Item.Brand_ID		= ItemBrand.Brand_ID
		INNER JOIN ItemIdentifier	(nolock) ON Item.Item_Key		= ItemIdentifier.Item_Key AND ItemIdentifier.Default_Identifier = 1
		INNER JOIN Price			(nolock) ON Item.Item_Key		= Price.Item_Key
		INNER JOIN Store			(nolock) ON Store.Store_No		= Price.Store_No       
    WHERE
        Retail_Sale = 1 
		AND Deleted_Item = 0
		AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0 
        AND Price.Price > 0
        AND Price.Store_No = ISNULL(@Store_No, Price.Store_No)
        AND (WFM_Store = 1 OR Mega_Store = 1)
        AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
    ORDER BY
        SubTeam_Name, Identifier

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginBySubTeamReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginBySubTeamReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginBySubTeamReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarginBySubTeamReport] TO [IRMAReportsRole]
    AS [dbo];

