SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.CheckVendorCostHistoryOverlap') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.CheckVendorCostHistoryOverlap
GO


CREATE PROCEDURE dbo.CheckVendorCostHistoryOverlap
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @Item_Key int,
    @Vendor_ID int,
    @StartDate smalldatetime,
    @EndDate smalldatetime
AS

BEGIN
    SET NOCOUNT ON

    SELECT COUNT(*)
    FROM StoreItemVendor SIV (nolock)
    INNER JOIN VendorCostHistory VCH (nolock)
        ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
    INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store 
        ON Store.Key_Value = SIV.Store_No AND SIV.Item_Key = @Item_Key AND SIV.Vendor_ID = @Vendor_ID
    WHERE (ISNULL(@StartDate, CONVERT(smalldatetime,CONVERT(varchar(255),GETDATE(),101))) >= VCH.StartDate
          OR
          ISNULL(@EndDate, '2079-06-06') <= VCH.EndDate)
    AND ((DeleteDate IS NULL) OR (ISNULL(@EndDate, DeleteDate) < DeleteDate))

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

