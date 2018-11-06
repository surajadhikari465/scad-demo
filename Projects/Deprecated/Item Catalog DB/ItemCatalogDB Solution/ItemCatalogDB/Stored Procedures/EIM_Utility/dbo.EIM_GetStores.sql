
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_GetStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_GetStores]
GO


CREATE PROCEDURE dbo.EIM_GetStores 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		Store.Store_No
		,Store_Name		
    FROM Store (nolock)
    WHERE Mega_Store = 1 OR WFM_Store = 1
    ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO



