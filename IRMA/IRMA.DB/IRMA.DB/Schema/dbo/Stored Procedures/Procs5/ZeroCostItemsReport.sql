-- ================================================================================
-- Author:		Sekhara
-- Create date: 10/11/2007
-- Description:	Exception Report for $0 AVERAGE Cost Items.
------------------------------------------------------------------------------------
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   StoreItemVendor.DiscontinueItem
-- =================================================================================

CREATE PROCEDURE [dbo].[ZeroCostItemsReport]
    @Store_No as INT,
    @SubTeam_No AS INT
AS 

BEGIN
    SET NOCOUNT ON
        SELECT 
              S.SubTeam_Name 
            , I.Item_Description 
            , II.Identifier 
            , ST.Store_name
            , ST.Store_No
            , S.SubTeam_No 
        FROM 
            Item I 
            INNER JOIN Price P (NOLOCK) 
                ON I.Item_Key = P.Item_Key  
            INNER JOIN Store ST (NOLOCK) 
                ON P.Store_No = ST.Store_No
            INNER JOIN ItemIdentifier II (NOLOCK)
                ON I.Item_Key = II.Item_Key AND II.Default_Identifier = 1 and II.IdentifierType='S'
            INNER JOIN SubTeam S(NOLOCK) 
                ON I.SubTeam_No = S.SubTeam_No
			INNER JOIN StoreItemVendor SIV(NOLOCK) 
                ON St.Store_No = SIV.Store_No AND P.Item_Key = SIV.Item_Key
        WHERE 
            SIV.DiscontinueItem = 0
            AND I.Deleted_Item = 0 
            AND I.Remove_Item = 0
            AND ISNULL(@Store_No, ST.Store_No) = ST.Store_No 
            AND ISNULL(@SubTeam_No, I.SubTeam_No) = I.SubTeam_No 
            AND ISNULL(dbo.fn_GetCurrentAvgCost (I.Item_Key, ST.Store_No, I.SubTeam_No, GETDATE()), 0) = 0
            AND (II.IdentifierType='S' or II.IdentifierType='s') 
        ORDER BY 
              S.SubTeam_Name
            , II.Identifier
         SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroCostItemsReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroCostItemsReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroCostItemsReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroCostItemsReport] TO [IRMAReportsRole]
    AS [dbo];

