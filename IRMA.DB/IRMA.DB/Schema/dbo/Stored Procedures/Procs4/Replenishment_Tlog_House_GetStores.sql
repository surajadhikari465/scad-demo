CREATE PROCEDURE dbo.Replenishment_Tlog_House_GetStores
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT store_no, store_name, storeabbr
	FROM Store 
	WHERE WFM_Store=1 and StoreAbbr is not null

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_GetStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_GetStores] TO [IRMAClientRole]
    AS [dbo];

