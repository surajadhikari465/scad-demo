CREATE PROCEDURE dbo.ProductNotAvaiable
    @SubTeam_No int,
    @WFM_Item tinyint
AS

-- **************************************************************************
-- Procedure: ProductNotAvailable()
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
		Item.Item_Key,
		Item_Description,
		Identifier,
		Not_AvailableNote,
		IsNull(dbo.fn_GetCurrentOnHand(Item.Item_Key, (SELECT TOP(1) Store_No FROM Store WHERE Distribution_Center = 1), Item.SubTeam_No),0) AS OnHand
    FROM
        Item (nolock)
        LEFT JOIN ItemCategory (nolock) ON Item.Category_ID = ItemCategory.Category_ID
        INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    WHERE
		Item.SubTeam_No = @SubTeam_No
		AND Item.WFM_Item >= @WFM_Item
		AND Item.Not_Available = 1
		AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0
		AND Deleted_Item = 0
    ORDER BY
		Category_Name,
		Item_Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductNotAvaiable] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductNotAvaiable] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductNotAvaiable] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductNotAvaiable] TO [IRMAReportsRole]
    AS [dbo];

