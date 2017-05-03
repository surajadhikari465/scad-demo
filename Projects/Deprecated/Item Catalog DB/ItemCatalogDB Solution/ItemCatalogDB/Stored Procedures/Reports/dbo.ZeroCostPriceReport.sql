SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ZeroCostPriceReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ZeroCostPriceReport]
GO

--exec ZeroCostPriceReport 4200, 103, 'Cost'
--exec ZeroCostPriceReport 4200, 101, 'Price'

CREATE PROCEDURE dbo.ZeroCostPriceReport
    @SubTeam_No AS INT, 
    @Store_No as INT, 
    @CostPrice as varchar(5) 
AS 

-- **************************************************************************
-- Procedure: ZeroCostPriceReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/12/2013  BAS	Update i.Discontinue_Item to account for schema change.
--					Renamed file to .sql.
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    IF @CostPrice = 'Cost'
       BEGIN
        SELECT 
              S.SubTeam_Name 
            , I.Item_Description 
            , II.Identifier 
            , II.Default_Identifier
            , ST.BusinessUnit_ID 
        FROM 
            Item I 
            INNER JOIN Price P (NOLOCK) 
                ON I.Item_Key = P.Item_Key  
            INNER JOIN Store ST (NOLOCK) 
                ON P.Store_No = ST.Store_No
            INNER JOIN ItemIdentifier II (NOLOCK)
                ON I.Item_Key = II.Item_Key AND II.Default_Identifier = 1
            INNER JOIN SubTeam S(NOLOCK) 
                ON I.SubTeam_No = S.SubTeam_No
			INNER JOIN StoreItemVendor SIV(NOLOCK) 
                ON St.Store_No = SIV.Store_No AND P.Item_Key = SIV.Item_Key
        WHERE 
            dbo.fn_GetDiscontinueStatus(I.Item_Key, ISNULL(@Store_No, ST.Store_No), NULL) = 0
            AND I.Deleted_Item = 0 
            AND I.Remove_Item = 0
            AND ISNULL(@Store_No, ST.Store_No) = ST.Store_No 
            AND ISNULL(@SubTeam_No, I.SubTeam_No) = I.SubTeam_No 
            AND ISNULL(dbo.fn_AvgCostHistory(I.Item_Key, ST.Store_No, I.SubTeam_No, GETDATE()), 0) = 0 
        ORDER BY 
              S.SubTeam_Name
            , II.Identifier
      END
    ELSE IF @CostPrice = 'Price'
       BEGIN
        SELECT 
              S.SubTeam_Name 
            , I.Item_Description 
            , II.Identifier 
            , II.Default_Identifier
            , ST.BusinessUnit_ID 
        FROM 
            Item I 
            INNER JOIN Price P (NOLOCK) 
                ON I.Item_Key = P.Item_Key  
            INNER JOIN Store ST (NOLOCK) 
                ON P.Store_No = ST.Store_No
            INNER JOIN ItemIdentifier II (NOLOCK)
                ON I.Item_Key = II.Item_Key AND II.Default_Identifier = 1
            INNER JOIN SubTeam S(NOLOCK) 
                ON I.SubTeam_No = S.SubTeam_No
			INNER JOIN StoreItemVendor SIV(NOLOCK) 
                ON St.Store_No = SIV.Store_No AND P.Item_Key = SIV.Item_Key		
        WHERE 
            dbo.fn_GetDiscontinueStatus(I.Item_Key, ISNULL(@Store_No, ST.Store_No), NULL) = 0
            AND I.Deleted_Item = 0 
            AND I.Remove_Item = 0
            AND P.Price = 0
            AND ISNULL(@Store_No, ST.Store_No) = ST.Store_No 
            AND ISNULL(@SubTeam_No, I.SubTeam_No) = I.SubTeam_No 
            AND (ST.Mega_Store = 1 OR ST.WFM_Store = 1)
        ORDER BY 
              S.SubTeam_Name
            , II.Identifier
      END

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

