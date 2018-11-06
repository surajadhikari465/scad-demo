 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DailyItemAvgCostChgRpt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DailyItemAvgCostChgRpt]
GO

-- =========================================================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- To fetch the data required for DailyItemAverageCostChanges report.
-- =========================================================================

CREATE PROCEDURE [dbo].[DailyItemAvgCostChgRpt]
	(@SKU varchar(13), 
	 @Store_No int,
     @SubTeam_No int)
 AS

BEGIN

SET NOCOUNT ON
  --To fetch the Item_Key value to the corresponding SKU.
    DECLARE @Item_KEY int
    IF @SKU IS NOT NULL 
    BEGIN
		select @Item_KEY=Item_key from itemidentifier 
		where identifier=@SKU AND DELETED_IDENTIFIER = 0--IDENTIFIER IS NOT LOGICALLY DELETED
		group by item_key
   END
   SELECT    @Item_KEY 
   --To fetch the report required data from the following Query.
   SELECT 
   si.Store_No as FacilityID,
   S.Store_Name as FacilityName,
   ST.SubTeam_No as SubTeam,
   REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier) as SKU,
   I.Item_Description as Description,
   I.Category_ID as Class,
   l4.ProdHierarchyLevel3_ID as Level3,
   i.ProdHierarchyLevel4_ID as Level4,
	--Item_key,store_no,SubTeam_No,
	dbo.fn_CurAvgCostHistory(I.Item_key,S.Store_No,I.SubTeam_No) as CurrAvgCost,
	dbo.fn_PrvAvgCostHistory(I.Item_key,S.Store_No,I.SubTeam_No) as PrvAvgCost,
	dbo.fn_CurOnHoldQtyAvgCostHistory(I.Item_key,S.Store_No,I.SubTeam_No) as CurrOnHand,
	dbo.fn_PrvOnHoldQtyAvgCostHistory(I.Item_key,S.Store_No,I.SubTeam_No) as PrvOnHand
   from
	storeItem si (nolock)
	inner join dbo.Item I (nolock)
	on si.Item_Key = I.Item_Key
    inner join dbo.ItemIdentifier II (nolock) on II.Item_Key = I.Item_Key and default_identifier = 1--always 
    LEFT join dbo.ProdHierarchyLevel4  l4 (nolock) on I.ProdHierarchyLevel4_ID = l4.ProdHierarchyLevel4_ID
	inner join store  s (nolock) on s.store_no = si.store_no
	inner join subteam st on st.subteam_no = i.subteam_no

		WHERE S.Store_No = ISNULL(@Store_No,S.Store_NO)
		AND I.Item_Key = isnull(@Item_Key,I.Item_Key)
		AND I.SubTeam_No = ISNULL(@SubTeam_No,I.SubTeam_No)
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO