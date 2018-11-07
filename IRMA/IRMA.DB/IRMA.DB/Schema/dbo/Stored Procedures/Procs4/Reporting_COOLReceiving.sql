CREATE PROCEDURE [dbo].[Reporting_COOLReceiving]
@Identifier nchar(15),
@POID int
As


BEGIN
    SET NOCOUNT ON

IF @Identifier IS NOT NULL AND @POID IS NOT NULL
BEGIN
select POID = CAST(COOL.HostPOID AS INT), COOL.Product, S.Store_Name, COOL.ProductDescription, COOL.ProductSize, COOL.SupplierName, COOL.SupplierAddress1, 
COOL.SupplierAddress2, COOL.SupplierCity, COOL.SupplierCountry, ReceiptQuantity = CAST(COOL.ReceiptQuantity AS INT), COOL.LotNumber, COOL.InboundCarrierName
from  dbo.OrderItemCOOL4020Detail COOL 
JOIN dbo.OrderHeader OH on CAST(COOL.HostPOID AS INT) = OH.OrderHeader_ID
LEFT JOIN dbo.Store S ON CAST(COOL.ReceivingDC AS INT) =  ISNULL(S.BusinessUnit_ID, 0) % 100 
WHERE LTRIM(RTRIM(COOL.Product)) = @Identifier AND CAST(COOL.HostPOID AS INT) = @POID
GROUP BY CAST(COOL.HostPOID AS INT), COOL.Product, S.Store_Name, COOL.ProductDescription, COOL.ProductSize, COOL.SupplierName, COOL.SupplierAddress1, 
COOL.SupplierAddress2, COOL.SupplierCity, COOL.SupplierCountry, CAST(COOL.ReceiptQuantity AS INT), COOL.LotNumber, COOL.InboundCarrierName

END
ELSE
BEGIN
select POID = CAST(COOL.HostPOID AS INT), COOL.Product, S.Store_Name, COOL.ProductDescription, COOL.ProductSize, COOL.SupplierName, COOL.SupplierAddress1, 
COOL.SupplierAddress2, COOL.SupplierCity, COOL.SupplierCountry, ReceiptQuantity = CAST(COOL.ReceiptQuantity AS INT), COOL.LotNumber, COOL.InboundCarrierName
from  dbo.OrderItemCOOL4020Detail COOL 
JOIN dbo.OrderHeader OH on CAST(COOL.HostPOID AS INT) = OH.OrderHeader_ID
LEFT JOIN dbo.Store S ON CAST(COOL.ReceivingDC AS INT) =  ISNULL(S.BusinessUnit_ID, 0) % 100 
WHERE LTRIM(RTRIM(COOL.Product)) = @Identifier OR CAST(COOL.HostPOID AS INT) = @POID
GROUP BY CAST(COOL.HostPOID AS INT), COOL.Product, S.Store_Name, COOL.ProductDescription, COOL.ProductSize, COOL.SupplierName, COOL.SupplierAddress1, 
COOL.SupplierAddress2, COOL.SupplierCity, COOL.SupplierCountry, CAST(COOL.ReceiptQuantity AS INT), COOL.LotNumber, COOL.InboundCarrierName
END          
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_COOLReceiving] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_COOLReceiving] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_COOLReceiving] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_COOLReceiving] TO [IRMAReportsRole]
    AS [dbo];

