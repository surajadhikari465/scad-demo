CREATE PROCEDURE [dbo].[ShrinkReport]
    @Store_No int,
    @SubTeam_List varchar(8000),
    @BeginDate varchar(20),
    @EndDate varchar(20)
AS

-- ***********************************************************************************************
-- Procedure: ShrinkReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from Shrink Report in Report Manager
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/21/2011	MD   	    	Removed hard-coded value treatment and applied coding standards
-- 03/21/2011   MD      1406    The Quantity and Weight do not need to be multiplied by adjustment_type
--								removed the multiplication
-- 09/12/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- ***********************************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON

	DECLARE @tblSubTeamList table(SubTeam_No int)

	INSERT INTO @tblSubTeamList
		SELECT Key_Value
		FROM fn_Parse_List(@SubTeam_List, ',') FN
		WHERE EXISTS (SELECT * FROM SubTeam WHERE SubTeam_No = FN.Key_Value)

    SELECT @Store_No, 
           (SELECT ISNULL(S.Store_Name, '') 
			FROM Store (nolock) S 
			WHERE S.Store_No = @Store_No) as StoreName, 
           ItemHistory.Item_Key, Quantity as Quantity, Weight as Weight, 
           ISNULL(dbo.fn_GetUnitCostForSpoilage(ItemHistory.Item_Key, ItemHistory.Store_No, ItemHistory.SubTeam_No, ItemHistory.DateStamp), 0) As AvgCost,
           Item_Description, Identifier, 
           (SELECT SubTeam_Name FROM SubTeam (nolock) WHERE SubTeam_No = ISNULL(SubTeamList.SubTeam_No, ItemHistory.SubTeam_No)) As SubTeam_Name, 
           ItemHistory.Adjustment_ID, AdjustmentReason
    FROM ItemHistory (nolock) 
	INNER JOIN 
		@tblSubTeamList SubTeamList 
		ON ItemHistory.SubTeam_No = SubTeamList.SubTeam_No
    INNER JOIN
        Item (nolock)
        ON ItemHistory.Item_Key = Item.Item_Key
    INNER JOIN 
        ItemIdentifier (nolock) 
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    INNER JOIN
		ItemAdjustment (nolock) ia
		ON ItemHistory.Adjustment_ID = ia.Adjustment_ID
    WHERE DateStamp >= @BeginDate AND DateStamp < DATEADD(d,1,@EndDate) AND
--          ItemHistory.SubTeam_No = ISNULL(@SubTeam_No, ItemHistory.SubTeam_No) AND 
          ItemHistory.Store_No = @Store_No AND
          ItemHistory.Adjustment_ID = 1
	ORDER BY SubTeamList.SubTeam_No

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShrinkReport] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShrinkReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShrinkReport] TO [IRMAReportsRole]
    AS [dbo];

