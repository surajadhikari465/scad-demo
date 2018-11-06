CREATE PROCEDURE dbo.KitchenCaseTransferRpt
    @BeginDate varchar(10),
    @EndDate varchar(18),
    @Facility int,
    @SubTeam_No int

AS 

-- **************************************************************************
-- Procedure: KitchenCaseTransferRpt
--    Author: Amudha Sethuraman
--      Date: 12.10.2012
--
-- Description:
-- This procedure is used to populate the data for Kitchen Case Transfer Report.
-- According to Bug 8451, this report was causing slowdown in IRMA and took for ever.
-- So this is a new attempt to make things faster. The earlier stored proc did not have 
-- NOLOCK, joints were inefficient and unnecessary rollup tables were joined (this total calcultaion can be handled in SSRS).
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/10/2012	AS  	8451	Creation

-- **************************************************************************

BEGIN
    SET NOCOUNT ON
   
    
	SET @EndDate = @EndDate + ' 23:59:59'
	
	SELECT Kit.CompanyName As KitchenName
	, OH.Transfer_To_SubTeam As SubTeam
	, I.Category_ID As Class
	, L4.ProdHierarchyLevel3_ID As Level3
	, I.ProdHierarchyLevel4_ID As Level4
	, REPLICATE('0',12-LEN(RTRIM(II.Identifier))) + RTRIM(II.Identifier) as SKU
	, I.Item_Key 
	, I.Item_Description 
	, (OI.Cost * OI.QuantityOrdered) As LandedCaseCost
	, Convert(Int, OI.Package_Desc1) As CasePack
	, Rec.Store_no As ReceiverStoreNo
	, Rec.CompanyName As ReceiverStoreName
	, Convert(Int, SUM(OI.QuantityOrdered)) As OrderedQuantity
FROM Vendor Kit (NOLOCK) 
JOIN OrderHeader OH (NOLOCK) ON OH.Vendor_ID = Kit.Vendor_ID 
JOIN OrderItem OI (NOLOCK) ON OH.OrderHeader_ID = OI.OrderHeader_ID  
JOIN Item I (NOLOCK) ON I.Item_Key = OI.Item_Key 
JOIN ItemIdentifier II (NOLOCK) ON I.Item_Key = II.Item_Key 
								AND II.Default_Identifier =	1
LEFT JOIN ProdHierarchyLevel4 L4 (NOLOCK) ON L4.ProdHierarchyLevel4_ID = I.ProdHierarchyLevel4_ID
JOIN Vendor Rec (NOLOCK) ON Rec.Vendor_ID = OH.ReceiveLocation_ID
WHERE OH.CloseDate BETWEEN @BeginDate AND @EndDate
  AND Kit.Vendor_ID = @Facility 
  AND OH.Transfer_To_SubTeam = ISNULL(@SubTeam_No, OH.Transfer_To_SubTeam)  
GROUP BY II.Identifier
	, I.Item_Key 
	, I.Item_Description 
	, Kit.CompanyName
	, OH.Transfer_To_SubTeam
	, I.Category_ID
	, L4.ProdHierarchyLevel3_ID
	, I.ProdHierarchyLevel4_ID
	, OI.Package_Desc1
	, Rec.Store_no
	, Rec.CompanyName
	, OI.Cost * OI.QuantityOrdered
ORDER BY 
	  REPLICATE('0',12-LEN(RTRIM(II.Identifier))) + RTRIM(II.Identifier)
	, Rec.CompanyName
		
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KitchenCaseTransferRpt] TO [IRMASLIMRole]
    AS [dbo];

