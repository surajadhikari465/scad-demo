SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvailPrimVend]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvailPrimVend]
GO


CREATE PROCEDURE dbo.GetAvailPrimVend
@VendorID int,
@Item_Key int,
@Store_no int
AS
BEGIN
    SET NOCOUNT ON

    SELECT SIV.Vendor_ID, CompanyName, count(distinct SIV.Item_Key) ItmCnt,
			S.Store_Name, II.Identifier
    FROM Vendor
        INNER JOIN
            StoreItemVendor SIV (NOLOCK)
            ON Vendor.Vendor_ID = SIV.Vendor_ID
        INNER JOIN
            (SELECT Item_Key, Store_No
             FROM StoreItemVendor (NOLOCK)
             WHERE vendor_ID = @VendorID and PrimaryVendor = 1 and Item_key = isnull(@Item_Key, Item_Key)) PrimLst
            ON PrimLst.Item_key = SIV.Item_Key and PrimLst.Store_no = SIV.Store_No
        INNER JOIN
			Store S (NOLOCK)
			ON SIV.Store_No = S.Store_No
		INNER JOIN
			ItemIdentifier II (NOLOCK)
			ON SIV.Item_Key = II.Item_Key
    WHERE SIV.Vendor_ID <> @VendorID and (SIV.DeleteDate is null or SIV.DeleteDate > getdate()) and SIV.Store_no = isnull(@Store_no, SIV.Store_no)
    GROUP BY SIV.Vendor_ID, CompanyName, Store_Name, Identifier
    ORDER BY Count(DISTINCT SIV.Item_Key) DESC
   
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

