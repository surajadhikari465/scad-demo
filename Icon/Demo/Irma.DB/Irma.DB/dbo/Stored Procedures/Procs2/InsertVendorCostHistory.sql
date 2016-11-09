create  PROCEDURE dbo.InsertVendorCostHistory
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
    @CostUnit_ID int,
    @FreightUnit_ID int,
	@Currency int = null,
	-- added to the end of so it can be optional
    -- only used by the JDA to IRMA loaded cost sync
    -- to let the JDA to IRMA sync trigger know
    -- the cost came from JDA and does not need
    -- to be syned back
    @IsFromJDASync bit = null
AS

BEGIN
    SET NOCOUNT ON
    
	SELECT @IsFromJDASync = IsNull(@IsFromJDASync, 0)

    INSERT INTO VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor, CostUnit_ID, FreightUnit_ID, IsFromJDASync, Currency)
    SELECT StoreItemVendorID, 
           @Promotional,
           @UnitCost,
           @UnitFreight,
           @Package_Desc1,
           ISNULL(@StartDate, CONVERT(smalldatetime,CONVERT(varchar(255),GETDATE(),101))),
           ISNULL(@EndDate, '2079-06-06'),
           @MSRP,
           @FromVendor,
           @CostUnit_ID,
           @FreightUnit_ID,
           @IsFromJDASync,
		   @Currency
    FROM StoreItemVendor SIV
    INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store 
        ON Store.Key_Value = SIV.Store_No AND SIV.Item_Key = @Item_Key AND SIV.Vendor_ID = @Vendor_ID
    WHERE (DeleteDate IS NULL) OR (ISNULL(@EndDate, DeleteDate) < DeleteDate)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistory] TO [IRMASLIMRole]
    AS [dbo];

