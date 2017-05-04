SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemPriceReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemPriceReport]
GO
    

CREATE PROCEDURE dbo.ItemPriceReport
    @Store_No int, 
    @SubTeam_No int, 
    @Category_ID int, 
    @Discontinue_Item int,
    @WFM_Item int
AS 

-- **************************************************************************
-- Procedure: ItemPriceReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from IRMA Client (might be deprecated)
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item reference to dbo.fn_GetDiscontinueStatus
--								to account for schema change
-- **************************************************************************

SELECT Item.Item_Key,
       Identifier, 
       Item.Item_Description,
       Item.Package_Desc1, Item.Package_Desc2, ItemUnit.Unit_Abbreviation, 
       Price.Multiple, Price.Price, 
       ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @Store_No, Item.SubTeam_No, GETDATE()), 0) AS AvgCost, 
       Price.Sale_Multiple, Price.Sale_Price, 
       (CASE WHEN ISNULL(Price.Sale_End_Date,'1980-01-01') > DateAdd(d,-1,GETDate()) THEN Price.Sale_End_Date ELSE NULL END) AS Sales_End_Date,
       Sale_Max_Quantity, 
       Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3,
       Sale_Start_Date, Sale_End_Date
FROM ItemUnit RIGHT JOIN (
       ItemIdentifier INNER JOIN (
         Item INNER JOIN Price ON (Item.Item_Key = Price.Item_Key)
       ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
     ) ON (Item.Package_Unit_ID = ItemUnit.Unit_ID)
WHERE Price.Store_No = @Store_No AND
      ISNULL(Item.Category_ID,0) = ISNULL(@Category_ID, ISNULL(Item.Category_ID,0)) AND
      Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No) AND
      dbo.fn_GetDiscontinueStatus(Item.Item_Key, @Store_No, NULL) <= @Discontinue_Item AND
      Item.WFM_Item >= @WFM_Item AND
      Deleted_Item = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


