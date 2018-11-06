
/*********************************************************************************************************************************************
CHANGE LOG
DEV				DATE					TASK				DESCRIPTION
----------------------------------------------------------------------------------------------
Jamali			2016-09-26				PBI18216			Remove the dataset for the Tax flags											
***********************************************************************************************************************************************/

CREATE PROCEDURE  dbo.Replenishment_POSPush_GetIdentifierRefreshes
    @Date datetime,
    @AuditReport bit,
    @Store_No int
AS 

-- Read the tax details for each item that will be returned in the second result set
BEGIN
SET TRANSACTION ISOLATION LEVEL SNAPSHOT
SET NOCOUNT ON

--list of items associated with the added identifiers and their details
EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
	@NewItemVal = 0,
	@ItemChangeVal = 0,
	@RemoveItemVal = 0,
	@PIRUSHeaderActionVal = 'A ',
	@Deletes = 0,
	@IsPOSPush = 0,
	@IsScaleZoneData = 0,
	@POSDeAuthData = 0,	
	@ScaleDeAuthData = 0, 
	@ScaleAuthData = 0,
	@IdentifierAdds = 0,
	@IdentifierDeletes = 0,
	@IdentifierRefreshes = 1,
	@Date = @Date,
	@AuditReport = @AuditReport,
	@Store_No = @Store_No,
	@LegacyStoresOnly = 1
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierRefreshes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierRefreshes] TO [IRMAClientRole]
    AS [dbo];

