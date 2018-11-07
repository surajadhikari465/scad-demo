
CREATE PROCEDURE dbo.Replenishment_POSPush_GetIdentifierAdds
    @Date datetime,
    @AuditReport bit,
    @Store_No int
AS 
-- *************************************************************************************************************************************************************
-- Procedure: Replenishment_POSPush_GetIdentifierAdds()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Gets Identifier Adds info as part of POS Push routine
--
-- Modification History:
-- Date      Init	TFS/PBI			Comment
-- 20160718  Jamali PBI 16828		Renoved the cursor and using a cross-apply to make the direct call to the fn 
-- 20160926  Jamali PBI 18217		Removed the data set for the Tax flags
-- **************************************************************************************************************************************************************
SET TRANSACTION ISOLATION LEVEL SNAPSHOT
SET NOCOUNT ON

-- The AuditReport logic is making this query crawl.  
IF @AuditReport = 1
BEGIN
	EXEC Replenishment_GetIdentifierAddsForAudits @Date, @Store_No
END
ELSE
BEGIN
	-- list of items associated with the added identifiers and their details
	EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
		@NewItemVal = 1,
		@ItemChangeVal = 0,
		@RemoveItemVal = 0,
		@PIRUSHeaderActionVal = 'A ',
		@Deletes = 0,
		@IsPOSPush = 0,
		@IsScaleZoneData = 0,
		@POSDeAuthData = 0,	
		@ScaleDeAuthData = 0, 
		@ScaleAuthData = 0,
		@IdentifierAdds = 1,
		@IdentifierDeletes = 0,
		@Date = @Date,
		@AuditReport = @AuditReport,
		@Store_No = null, --@Store_No,
		@LegacyStoresOnly = 1
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAReportsRole]
    AS [dbo];

