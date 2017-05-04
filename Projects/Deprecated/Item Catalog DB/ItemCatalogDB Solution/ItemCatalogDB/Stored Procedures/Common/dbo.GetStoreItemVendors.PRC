SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreItemVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreItemVendors]
GO


CREATE PROCEDURE dbo.GetStoreItemVendors
    @Item_Key int,
    @Store_No int
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
    
    SELECT Vendor.Vendor_ID, Vendor.CompanyName
    FROM Vendor (nolock)
        INNER JOIN
            StoreItemVendor SIV (nolock)
            ON SIV.Vendor_ID = Vendor.Vendor_ID 
    where SIV.Store_No = @Store_No AND SIV.Item_Key = @Item_Key
            AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


