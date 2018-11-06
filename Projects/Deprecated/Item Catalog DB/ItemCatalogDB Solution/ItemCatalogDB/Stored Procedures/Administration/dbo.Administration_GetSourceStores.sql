/****** Object:  StoredProcedure [dbo].[Administration_GetSourceStores]    Script Date: 05/19/2006 16:33:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetSourceStores]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_GetSourceStores]
GO

/****** Object:  StoredProcedure [dbo].[Administration_GetSourceStores]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_GetSourceStores] 
	@POSFileWriterKey int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT s.Store_Name,
		s.Store_No
    FROM Store s (NOLOCK) 
	JOIN dbo.storeposconfig spos (NOLOCK) ON spos.Store_No = s.store_no
    WHERE (Mega_Store = 1 OR WFM_Store = 1) 
		AND spos.POSFileWriterKey = @POSFileWriterKey
		AND dbo.fn_GetCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID) = 3 -- Regional   
    ORDER BY Store_Name, Store_No
    
    SET NOCOUNT OFF
END


GO