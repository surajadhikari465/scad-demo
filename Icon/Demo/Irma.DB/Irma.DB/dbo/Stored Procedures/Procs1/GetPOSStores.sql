CREATE PROCEDURE dbo.GetPOSStores
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON
    
    SELECT Store.Store_No, 
		ChangeDirectory AS HD_Directory
	FROM Store (nolock)
	LEFT JOIN
		StoreFTPConfig
		ON StoreFTPConfig.Store_No = Store.Store_No 
	WHERE FileWriterType = 'POS'
		AND (Mega_Store = 1 OR WFM_Store = 1) 
		AND IP_Address IS NOT NULL
		AND (Store.Store_No = ISNULL(@Store_No, Store.Store_No))
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSStores] TO [IRMAReportsRole]
    AS [dbo];

