-- =============================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- Function to fetch the Current on Hold Quantity for the specific item 
--  based on store and SubTeam.
-- =============================================
CREATE FUNCTION [dbo].[fn_CurOnHoldQtyAvgCostHistory] 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int)
RETURNS decimal(18,4)
AS
BEGIN
	 DECLARE @CurrOnHold decimal(18,4)
	 
    SELECT @CurrOnHold =   (SELECT TOP 1 Quantity
                         FROM AvgCostHistory (nolock)
                         WHERE Item_Key = ISNULL(@Item_Key,Item_Key)
							AND Store_No = ISNULL(@Store_No,store_No)
							AND SubTeam_No = ISNULL(@SubTeam_No,SubTeam_No)
                             AND Effective_Date <= GETDATE()
                         ORDER BY Effective_Date DESC)   
    RETURN @CurrOnHold
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CurOnHoldQtyAvgCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CurOnHoldQtyAvgCostHistory] TO [IRMAReportsRole]
    AS [dbo];

