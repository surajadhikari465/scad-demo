CREATE PROCEDURE [dbo].[DCPerformanceReport]
	(
		@StartDate datetime = NULL,
		@EndDate datetime = NULL,
		@Store_No int = NULL,
		@SubTeam_No int = NULL
	)

AS

DECLARE @Case int, @Unit int
SELECT @Case = Unit_ID FROM ItemUnit WHERE UnitSysCode = 'Case'
SELECT @Unit = Unit_ID FROM ItemUnit WHERE UnitSysCode = 'Unit'

DECLARE @Vendor_ID int

IF @Store_No IS NOT NULL
    SELECT @Vendor_ID = Vendor_ID FROM Vendor (nolock) WHERE Store_No = @Store_No

DECLARE @Received TABLE (Vendor_ID int, SubTeam_No int, CasesReceived decimal(18,4), UnitsReceived decimal(18,4))

INSERT INTO @Received
SELECT  ReceiveLocation_ID, OH.Transfer_To_SubTeam, 
	    SUM(dbo.fn_CostConversion(QuantityReceived, @Case, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID)),
		SUM(UnitsReceived)
FROM OrderHeader OH (nolock)
INNER JOIN 
	OrderItem OI (nolock)
	ON OH.OrderHeader_ID = OI.OrderHeader_ID
INNER JOIN
	Vendor RecvVend (nolock)
	ON RecvVend.Vendor_ID = OH.ReceiveLocation_ID
INNER JOIN
	Store (nolock)
	ON Store.Store_No = RecvVend.Store_No
WHERE (OH.ReceiveLocation_ID = ISNULL(@Vendor_ID, OH.ReceiveLocation_ID)
	AND OH.Transfer_To_SubTeam = ISNULL(@SubTeam_No, OH.Transfer_To_SubTeam)
	AND Return_Order = 0)
	AND (DateReceived >= @StartDate AND DateReceived < @EndDate)
	AND(Distribution_Center = 1 OR Manufacturer = 1)
GROUP BY ReceiveLocation_ID, OH.Transfer_To_SubTeam

DECLARE @Shipped TABLE (Vendor_ID int, SubTeam_No int, CasesOrdered decimal(18,4), UnitsOrdered decimal(18,4), CasesAllocated decimal(18,4), UnitsAllocated decimal(18,4), CasesShipped decimal(18,4), UnitsShipped decimal(18,4), CasesReturned decimal(18,4), UnitsReturned decimal(18,4), OrderCount int)

INSERT INTO @Shipped
SELECT 
	OH.Vendor_ID,
	Transfer_SubTeam,
	CasesOrdered = SUM(dbo.fn_CostConversion(QuantityOrdered, @Case, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID)	* CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END),
	UnitsOrdered = SUM(dbo.fn_CostConversion(QuantityOrdered, CASE WHEN CostedByWeight = 1 THEN OI.Package_Unit_ID ELSE @Unit END, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID) * CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END), 
	CasesAllocated = SUM(dbo.fn_CostConversion(QuantityAllocated, @Case, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID) * CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END), 
    UnitsAllocated = SUM(dbo.fn_CostConversion(QuantityAllocated, CASE WHEN CostedByWeight = 1 THEN OI.Package_Unit_ID ELSE @Unit END, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID) * CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END), 
	CasesShipped = SUM(dbo.fn_CostConversion(QuantityReceived, @Case, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID)	* CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END),
    UnitsShipped = SUM(UnitsReceived * CASE WHEN OH.Return_Order = 0 THEN  1 ELSE -1 END),
	CasesReturned = SUM(CASE WHEN OH.Return_Order = 1 THEN dbo.fn_CostConversion(QuantityReceived, @Case, QuantityUnit, OI.Package_Desc1, OI.Package_Desc2, OI.Package_Unit_ID) ELSE 0 END),
    UnitsReturned = SUM(CASE WHEN OH.Return_Order = 1 THEN UnitsReceived ELSE 0 END),
    COUNT(CASE WHEN OH.Return_Order = 0 THEN OH.OrderHeader_ID ELSE NULL END)
FROM OrderItem OI (nolock)
INNER JOIN 
	OrderHeader OH (nolock)
	ON OH.OrderHeader_ID = OI.OrderHeader_ID
INNER JOIN 
	Vendor V (nolock)
	ON V.Vendor_ID = OH.Vendor_ID
INNER JOIN
	Store VendStore (nolock)
	ON VendStore.Store_No = V.Store_No
INNER JOIN
	ItemUnit QU (nolock)
	ON QU.Unit_ID = OI.QuantityUnit
INNER JOIN
    Item (nolock)
    ON Item.Item_Key = OI.Item_Key
WHERE (VendStore.Distribution_Center = 1 OR VendStore.Manufacturer = 1)
	AND DateReceived >= @StartDate AND DateReceived < @EndDate
	AND OH.Vendor_ID = ISNULL(@Vendor_ID, OH.Vendor_ID)
	AND Transfer_SubTeam = ISNULL(@SubTeam_No, Transfer_SubTeam)
GROUP BY OH.Vendor_ID, Transfer_SubTeam

SELECT CompanyName, SubTeam_Name, CasesReceived, UnitsReceived, CasesOrdered, UnitsOrdered, CasesAllocated, UnitsAllocated, CasesShipped, UnitsShipped, CasesReturned, UnitsReturned, OrderCount As ShippedOrders
FROM Vendor (nolock)
INNER JOIN 
	Store (nolock) 
	ON Store.Store_No = Vendor.Store_No
INNER JOIN
	StoreSubTeam SST (nolock)
	ON SST.Store_No = Store.Store_No
INNER JOIN
	SubTeam (nolock)
	ON SubTeam.SubTeam_No = SST.SubTeam_No
LEFT JOIN
	@Received R
	ON R.Vendor_ID = Vendor.Vendor_ID AND R.SubTeam_No = SST.SubTeam_No
LEFT JOIN
	@Shipped S
	ON S.Vendor_ID = Vendor.Vendor_ID AND S.SubTeam_No = SST.SubTeam_No
WHERE (Distribution_Center = 1 OR Manufacturer = 1)
	AND S.Vendor_ID IS NOT NULL
	AND Vendor.Vendor_ID = ISNULL(@Vendor_ID, Vendor.Vendor_ID)
	AND SST.SubTeam_No = ISNULL(@SubTeam_No, SST.SubTeam_No)
ORDER BY CompanyName, SubTeam_Name