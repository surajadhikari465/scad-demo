SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertVendorDealHistory') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertVendorDealHistory
GO


CREATE PROCEDURE dbo.InsertVendorDealHistory
    @Item_Key int,
    @Vendor_ID int,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @CaseQty int,
    @Package_Desc1 decimal(9,4),
    @CaseAmt smallmoney,
    @StartDate smalldatetime,
    @EndDate smalldatetime,
    @TypeCode char(1),
    @FromVendor bit,
    @CostPromoCode int,
    @NotStackable bit    
AS

BEGIN
    SET NOCOUNT ON
			
	INSERT INTO VendorDealHistory (StoreItemVendorID, CaseQty, Package_Desc1, CaseAmt, StartDate, EndDate, VendorDealTypeID, FromVendor, CostPromoCodeTypeID, NotStackable)
    SELECT StoreItemVendorID, @CaseQty, @Package_Desc1, @CaseAmt, @StartDate, @EndDate, 
			(SELECT VendorDealTypeID FROM VendorDealType WHERE Code = @TypeCode), @FromVendor,
			(SELECT CostPromoCodeTypeID FROM CostPromoCodeType WHERE CostPromoCode = @CostPromoCode),
			@NotStackable
    FROM StoreItemVendor SIV
    INNER JOIN
        fn_Parse_List(@StoreList, @StoreListSeparator) Store
        ON Store.Key_Value = SIV.Store_No 
			AND SIV.Item_Key = @Item_Key 
			AND SIV.Vendor_ID = @Vendor_ID
    WHERE @EndDate < ISNULL(DeleteDate, DATEADD(day, 1, @EndDate))

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


