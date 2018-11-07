SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertVendorCostHistory3') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertVendorCostHistory3
GO


CREATE PROCEDURE dbo.InsertVendorCostHistory3
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @Item_Key int,
    @Vendor_ID int,
    @UnitCost smallmoney,
    @UnitFreight smallmoney,
    @Package_Desc1 decimal(9,4),
    @StartDate smalldatetime,
    @EndDate smalldatetime,
    @Promotional bit,
    @MSRP smallmoney,
    @FromVendor bit,
    @StoresAffected int OUTPUT
AS

BEGIN
    SET NOCOUNT ON
    DECLARE @StoreFreight table(Store_No int, UnitFreight money)
    DECLARE @CurrDate smalldatetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))    

    IF @UnitFreight IS null
      BEGIN
        INSERT INTO @StoreFreight
        SELECT VC.Store_No, VC.UnitFreight
        FROM dbo.fn_VendorCostStores(@Item_Key, @Vendor_ID, @CurrDate) VC
      END
    ELSE IF @UnitFreight = 0
      BEGIN
        SELECT @UnitFreight = null
      END

    INSERT INTO VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor)
    SELECT StoreItemVendorID, 
           @Promotional,
           Case WHEN VSF.FreightMarkUp is null
                  THEN @UnitCost + isnull(@UnitFreight, isnull(SF.UnitFreight, 0))
                  else @UnitCost * (1 + isnull(VSF.FreightMarkUp,0))
                end,
           CASE WHEN VSF.FreightMarkUp is null
                 THEN
                     CASE WHEN @UnitFreight is null
                            THEN
                              isnull(SF.UnitFreight,0)
                            ELSE
                              @UnitFreight
                          END
                ELSE
                    0
                END,
           @Package_Desc1,
           ISNULL(@StartDate, CONVERT(smalldatetime,CONVERT(varchar(255),GETDATE(),101))),
           ISNULL(@EndDate, '2079-06-06'),
           @MSRP,
           @FromVendor
    FROM StoreItemVendor SIV
    INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store 
        ON Store.Key_Value = SIV.Store_No AND SIV.Item_Key = @Item_Key AND SIV.Vendor_ID = @Vendor_ID
    LEFT JOIN 
        VendorStoreFreight VSF
        on VSF.Store_No = Store.Key_Value and VSF.Vendor_ID = SIV.Vendor_ID
    LEFT JOIN
        @StoreFreight SF
        on SF.Store_No = Store.Key_Value
    WHERE (DeleteDate IS NULL) OR (ISNULL(@EndDate, DeleteDate) < DeleteDate)
    
    SELECT scope_identity() as VCH_ID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


