/****** Object:  StoredProcedure [dbo].[Administration_GetSubStores]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_GetSubStores] 
	@SourceStore_No int,
    @Subteam_No int, 
	@POSFileWriterKey int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT s.Store_Name,
		s.Store_No
    FROM Store s (NOLOCK) 
	INNER JOIN dbo.Zone z (NOLOCK) ON s.Zone_Id = z.Zone_Id
	JOIN dbo.StoreSubteam sst (NOLOCK) ON s.Store_No = sst.Store_No 
	JOIN dbo.StorePOSConfig spc (nolock) ON s.Store_No = spc.Store_No
    WHERE (s.Mega_Store = 1 OR s.WFM_Store = 1)
			AND spc.PosFileWriterKey = @POSFileWriterKey
		AND dbo.fn_GetCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID) = 3 -- Regional   
		AND s.Store_No <> @SourceStore_No AND sst.Subteam_No = @Subteam_No				
    ORDER BY s.Mega_Store DESC, s.WFM_Store DESC, Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSubStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetSubStores] TO [IRMAClientRole]
    AS [dbo];

