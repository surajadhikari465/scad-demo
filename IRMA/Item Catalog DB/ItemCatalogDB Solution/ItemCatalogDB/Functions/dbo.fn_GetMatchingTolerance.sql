IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetMatchingTolerance') 
    DROP FUNCTION fn_GetMatchingTolerance
GO

CREATE FUNCTION [dbo].[fn_GetMatchingTolerance] (
	@OrderHeader_ID INT
) RETURNS @InvoiceTable TABLE (  
Tolerance DECIMAL(5,2),   
Tolerance_Amount smallmoney  
)  
AS  
BEGIN  
 DECLARE @Tolerance DECIMAL(5,2),  
   @Tolerance_Amount smallmoney  
  
 -- The default three way matching tolerance for an order is configured in the   
 -- InvoiceMatchingTolerance table.  
 -- The tolerance for an order can be overriden at the store level, configured in  
 -- the InvoiceMatchingTolerance_StoreOverride table, or at the vendor level,   
 -- configured in the InvoiceMatchingTolerance_VendorOverride table. 
 -- The most specific tolerance level that exists for the order is returned.  
  IF EXISTS (SELECT * FROM dbo.InvoiceMatchingTolerance_VendorOverride VO   
 INNER JOIN dbo.OrderHeader OH ON    
   OH.Vendor_ID = VO.Vendor_ID    
   WHERE OH.OrderHeader_ID = @OrderHeader_ID)  
   BEGIN  
 SELECT @Tolerance = VO.Vendor_Tolerance, @Tolerance_Amount = VO.Vendor_Tolerance_Amount FROM dbo.OrderHeader OH    
  INNER JOIN dbo.InvoiceMatchingTolerance_VendorOverride VO ON    
   OH.Vendor_ID = VO.Vendor_ID    
  WHERE OH.OrderHeader_ID = @OrderHeader_ID    
  END  
 ELSE IF EXISTS (SELECT * FROM dbo.InvoiceMatchingTolerance_StoreOverride SO   
 INNER JOIN dbo.Vendor V ON   
 V.Store_No = SO.Store_No     
 INNER JOIN dbo.OrderHeader OH ON    
    OH.PurchaseLocation_ID = V.Vendor_ID   
   WHERE OH.OrderHeader_ID = @OrderHeader_ID)  
 BEGIN    
  SELECT @Tolerance = SO.Vendor_Tolerance, @Tolerance_Amount = SO.Vendor_Tolerance_Amount FROM dbo.OrderHeader OH    
    INNER JOIN dbo.Vendor V ON   
 V.Vendor_ID = OH.PurchaseLocation_ID   
 INNER JOIN dbo.InvoiceMatchingTolerance_StoreOverride SO  ON    
    V.Store_No = SO.Store_No   
   WHERE OH.OrderHeader_ID = @OrderHeader_ID    
 END    
  ELSE  
 BEGIN    
  SELECT @Tolerance = Vendor_Tolerance, @Tolerance_Amount = Vendor_Tolerance_Amount FROM dbo.InvoiceMatchingTolerance    
 END    
  
 INSERT INTO @InvoiceTable (Tolerance, Tolerance_Amount)  
 SELECT   @Tolerance, @Tolerance_Amount  
   
 RETURN  
END  


GO
 