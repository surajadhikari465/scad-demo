-- ***********************************************************************************************
-- Procedure: GetShrinkCorrections()
--    Author: Faisal Ahmed
--      Date: n/a
--
-- Description:
-- This procedure is called from ShrinkCorrectionsDAO
--
-- Modification History:
-- Date        Init   TFS    Comment
-- 09/22/2010 BBB    13534 Removed hard-coded value treatment and added
--        call to ItemAdjustment; applied coding standards
-- 01/23/2010   FA    Calculates costed-by-weight field when considering retail unit
-- 03/21/2011   MD      1406    The Quantity and Weight do not need to be multiplied by Adjustment_Type
--        removed the multiplication
-- 10/22/2012   FA      4602    returned local time instead of server time. This value is used 
--                              for display only.
-- 10/24/2012   FA      8183    Fixed the date range in WHERE clause
-- 03/20/2013   MZ      11482 & Added OriginalDateStamp om the query to make bug fix for 4602 complete.
--                      11496
-- 03/24/2017   EM      23481   Fixed divide by zero bug when lacking Average_Unit_Weight
--                              Added 'AND IsNull(i.Average_Unit_Weight,0)>0' to Qty CASE statement 
-- 02/01/2018   EM      22815   Retrieving inventory adjustment code ID in addition to 'wType' description.
--                              Also added shrink subtype id and description to query
-- ***********************************************************************************************

CREATE Procedure dbo.GetShrinkCorrections
    @Store_No int, 
    @SubTeam_No int, 
    @StartDate smalldatetime, 
    @EndDate smalldatetime,
    @InventoryAdjustmentCode_Id int = 0,
    @ShrinkSubtype_Id int = 0
AS

DECLARE @CentralTimeZoneOffset int
SELECT  @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

SELECT 
      Identifier              = ii.Identifier
    , Item_Description        = i.Item_Description
    , ItemSubteam             = st.SubTeam_Name
    , Category_Name           = ic.Category_Name
    , Brand_Name              = ib.Brand_Name
    , OriginalDateStamp       = ih.DateStamp
    , DateStamp               = DATEADD(HOUR, @CentralTimeZoneOffset, ih.DateStamp) -- TFS 4602, Faisal Ahmed 
    , SubTeam_No              = ih.SubTeam_No
    , Qty                     = CASE
                                WHEN i.CostedByWeight = 0 THEN
                                    SUM(Quantity) + SUM(Weight)
                                ELSE
                                    CASE
                                        WHEN dbo.fn_IsRetailUnitNotCostedByWeight(ih.Item_key) = 1 
                                            AND IsNull(i.Average_Unit_Weight,0)>0 THEN
                                            (SUM(Quantity) + SUM(Weight))/i.Average_Unit_Weight
                                        ELSE
                                            SUM(Quantity) + SUM(Weight)
                                    END
                              END
    ,wType                   = ISNULL(iac.Abbreviation, 'SP') 
    ,CostedByWeight          = CASE 
                                WHEN dbo.fn_IsRetailUnitNotCostedByWeight(ih.Item_key) = 1 THEN
                                    0
                                ELSE
                                    i.CostedByWeight
                              END
    , UnitCost                = dbo.fn_GetUnitCostForSpoilage(ih.Item_Key, ih.Store_No, ih.SubTeam_No, ih.DateStamp)
    , InventoryAdjustment_ID = iac.InventoryAdjustmentCode_ID
    , ShrinkSubtype_ID       = sst.ShrinkSubType_ID
    , ShrinkSubtype_Desc	 = sst.ReasonCodeDescription
    , UserName                = u.UserName
FROM 
    dbo.ItemHistory ih
    INNER JOIN dbo.Item i                       ON i.Item_Key = ih.Item_Key
    INNER JOIN dbo.ItemIdentifier ii            ON ii.Item_Key = i.Item_Key AND ii.Default_Identifier = 1
    INNER JOIN dbo.SubTeam st                   ON st.SubTeam_No = i.SubTeam_No
    INNER JOIN dbo.Users u                      ON u.User_ID  = ih.CreatedBy
    INNER JOIN dbo.ItemAdjustment ia            ON ih.Adjustment_ID = ia.Adjustment_ID
     LEFT JOIN dbo.InventoryAdjustmentCode iac  ON iac.InventoryAdjustmentCode_ID = ih.InventoryAdjustmentCode_ID
     LEFT JOIN dbo.ItemCategory ic              ON ic.Category_ID  = i.Category_ID
     LEFT JOIN dbo.ItemBrand ib                 ON ib.Brand_ID = i.Brand_ID
     LEFT JOIN dbo.ItemHistoryShrinkSubType x   ON x.ItemHistoryID  = ih.ItemHistoryID
     LEFT JOIN dbo.ShrinkSubType sst            ON sst.ShrinkSubType_ID = x.ShrinkSubType_ID
WHERE 
    ih.Store_No    = @Store_No 
    AND ih.SubTeam_No  = @SubTeam_No
    -- filter by local timezone for the provided dates using the regional time zone offset (datestamps stored as central time0
    AND ih.DateStamp BETWEEN DATEADD(HOUR, -1 * @CentralTimeZoneOffset, @StartDate) AND DATEADD(HOUR, -1 * @CentralTimeZoneOffset, @EndDate)
    AND ih.Adjustment_ID = 1
    AND (	--both type & subtype are not specified, OR the subtype matches, OR the subtype is not specified & the type matches
		(ISNULL(@ShrinkSubtype_Id, 0 )<1 AND ISNULL(@InventoryAdjustmentCode_Id, 0 )<1)
		OR (sst.ShrinkSubType_ID = @ShrinkSubtype_Id)
		OR ( ISNULL(@ShrinkSubtype_Id, 0 ) < 1 AND iac.InventoryAdjustmentCode_ID = @InventoryAdjustmentCode_Id)
		)
GROUP BY 
    ih.Store_No, ii.Identifier, ih.Item_Key, i.Item_Description, st.SubTeam_Name,
    ic.Category_Name, ib.Brand_Name, DateStamp, ih.SubTeam_No, iac.Abbreviation,
    u.UserName, CostedByWeight, i.Average_Unit_Weight, sst.ShrinkSubType_ID,
    sst.ReasonCodeDescription, iac.Abbreviation, iac.InventoryAdjustmentCode_ID
HAVING
    SUM(Quantity + Weight) <> 0.0
ORDER BY 
    Identifier, 
    DateStamp DESC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShrinkCorrections] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShrinkCorrections] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShrinkCorrections] TO [IRMAReportsRole]
    AS [dbo];

GO