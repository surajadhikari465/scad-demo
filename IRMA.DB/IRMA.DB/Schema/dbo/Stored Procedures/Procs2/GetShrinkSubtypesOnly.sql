-- =============================================
-- Author:      Ed McNab
-- Create date: 2/5/2018
-- Description: Reads from the ShrinkSubType Table
-- Modification History:
--
-- =============================================
CREATE PROCEDURE dbo.GetShrinkSubtypesOnly
AS
BEGIN
    SET NOCOUNT ON
    
     SELECT ShrinkSubType_ID
           ,ReasonCodeDescription
           ,InventoryAdjustmentCode_ID
    FROM dbo.ShrinkSubType;
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE ON OBJECT:: dbo.GetShrinkSubtypesOnly TO IRMAAdminRole AS dbo;

GO
GRANT EXECUTE ON OBJECT:: dbo.GetShrinkSubtypesOnly TO IRMAClientRole AS dbo;

GO
GRANT EXECUTE ON OBJECT:: dbo.GetShrinkSubtypesOnly TO IRMASchedJobsRole AS dbo;

GO
GRANT EXECUTE ON OBJECT:: dbo.GetShrinkSubtypesOnly TO IRMAReportsRole AS dbo;

