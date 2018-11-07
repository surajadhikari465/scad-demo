SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetPOSStores') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetPOSStores
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



