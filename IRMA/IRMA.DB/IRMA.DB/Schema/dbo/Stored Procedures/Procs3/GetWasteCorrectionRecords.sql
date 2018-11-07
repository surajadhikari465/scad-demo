CREATE Procedure [dbo].[GetWasteCorrectionRecords]
    @Store_No int, 
    @SubTeam_No int, 
    @DateStamp datetime, 
    @WasteType varchar(3),
	@Identifier varchar(13)
AS

-- ***********************************************************************************************
-- Procedure: GetWasteCorrectionRecords()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ShrinkCorrectionsDAO
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/21/2011	MD   	    	Removed hard-coded value treatment and applied coding standards
-- 03/21/2011   MD      1406    The Quantity and Weight do not need to be multiplied by (-1)
--								removed the multiplication
-- 03/20/2013   MZ      4602    returned local time instead of server time for Insert_Date. 
--                              This value is used for display only.
-- ***********************************************************************************************

-- This is used by ADO.NET - don''t SET NOCOUNT ON
DECLARE @CentralTimeZoneOffset int	
SELECT  @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

SELECT 
	Identifier,
	Item_Description,
	Insert_Date       = DATEADD(HOUR, @CentralTimeZoneOffset, IH.Insert_Date),
	@DateStamp  as DateStamp,
	@WasteType as WasteType, 
	(Quantity + Weight) as Quantity,
	Users.FullName as 'Created By'
FROM dbo.ItemHistory IH (nolock)
	INNER JOIN Item (nolock) ON Item.Item_Key = IH.Item_Key
	INNER JOIN ItemIdentifier II (nolock) ON II.Item_Key = Item.Item_Key AND Default_Identifier = 1
	INNER JOIN SubTeam S (nolock) ON S.SubTeam_No = Item.SubTeam_No
	INNER JOIN Users (nolock) ON Users.User_ID = IH.CreatedBy
	LEFT JOIN InventoryAdjustmentCode IAC on IAC.InventoryAdjustmentCode_ID = IH.InventoryAdjustmentCode_ID
WHERE IH.Store_No = @Store_No AND IH.SubTeam_No = @SubTeam_No
	AND IH.DateStamp = @DateStamp 
	AND IH.Adjustment_ID = 1
	AND IAC.Abbreviation = @WasteType
	AND ii.Identifier = @Identifier
	AND IH.CorrectionRecordFlag is null
Order by Insert_Date Desc
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrectionRecords] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrectionRecords] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrectionRecords] TO [IRMAReportsRole]
    AS [dbo];

