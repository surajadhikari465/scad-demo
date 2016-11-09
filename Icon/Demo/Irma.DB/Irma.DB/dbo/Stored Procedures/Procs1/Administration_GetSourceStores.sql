/****** Object:  StoredProcedure [dbo].[Administration_GetSourceStores]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_GetSourceStores] 
	@POSFileWriterKey int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT s.Store_Name,
		s.Store_No
    FROM dbo.Store s (NOLOCK) 
	JOIN dbo.storeposconfig spos (NOLOCK) ON spos.Store_No = s.store_no
    WHERE (Mega_Store = 1 OR WFM_Store = 1) 
		AND spos.POSFileWriterKey = @POSFileWriterKey
		AND dbo.fn_GetCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID) = 3 -- Regional   
    ORDER BY s.Store_Name, s.Store_No
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSourceStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSourceStores] TO [IRMAClientRole]
    AS [dbo];

