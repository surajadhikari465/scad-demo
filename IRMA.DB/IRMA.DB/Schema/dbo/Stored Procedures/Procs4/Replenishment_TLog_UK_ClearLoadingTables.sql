CREATE PROCEDURE dbo.[Replenishment_TLog_UK_ClearLoadingTables]

AS

BEGIN
	delete from tlog_uk_item
	delete from tlog_uk_payment
	delete from tlog_uk_discounts
	delete from tlog_uk_offers
	delete from tlog_uk_voids
	delete from tlog_uk_transaction
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_ClearLoadingTables] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_ClearLoadingTables] TO [IRMAClientRole]
    AS [dbo];

