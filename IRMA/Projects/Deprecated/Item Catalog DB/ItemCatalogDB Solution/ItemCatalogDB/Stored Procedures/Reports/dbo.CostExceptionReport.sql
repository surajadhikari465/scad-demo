SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CostExceptionReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CostExceptionReport]
GO


CREATE PROCEDURE dbo.CostExceptionReport
    @Store_No int, 
    @SubTeam_No int, 
    @QueryType int, 
    @Cost int, 
    @Discontinue_Item int
AS
--**************************************************************************
-- Procedure: CostExceptionReport
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
BEGIN
    SET NOCOUNT ON

    SELECT * FROM (
        SELECT Item.Item_Key, Identifier, 
               Item.Item_Description, SubTeam.SubTeam_Name, dbo.fn_GetDiscontinueStatus(Item.Item_Key, @Store_No, NULL) AS Discontinue_Item,
               Price.Multiple, Price.Price,
               ISNULL(dbo.fn_AvgCostHistory(Price.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0) As AvgCost
        FROM SubTeam (nolock) INNER JOIN (
               ItemIdentifier (nolock) INNER JOIN (
                 Item (nolock) INNER JOIN (
                   Store (nolock) INNER JOIN Price (nolock) ON (Store.Store_No = Price.Store_No) 			
                 ) ON (Item.Item_Key = Price.Item_Key) 
               ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
             ) ON (SubTeam.SubTeam_No = Item.SubTeam_No)
        WHERE Item.Retail_Sale = 1 AND 
              ISNULL(@SubTeam_No, Item.SubTeam_No) = Item.SubTeam_No AND 
              Store.Store_No = @Store_No AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, @Store_No, NULL) <= @Discontinue_Item) T
    WHERE ((@QueryType = 1 AND (AvgCost > (Price / CASE WHEN Multiple > 0 THEN Multiple ELSE 1 END))) OR 
           (@QueryType = 2 AND (AvgCost > @Cost)))

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

