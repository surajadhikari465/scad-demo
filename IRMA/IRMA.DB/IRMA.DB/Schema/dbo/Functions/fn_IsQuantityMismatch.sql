CREATE FUNCTION [dbo].[fn_IsQuantityMismatch]
	(@OrderHeaderID int)
	 RETURNS varchar(5)
AS
 BEGIN  
        DECLARE @Return VARCHAR(10)
        DECLARE @ComparisonValue DECIMAL
        DECLARE @IsEinvoice BIT
        
        -- if an einvoice is present check for a mismatch. 
        -- otherwise return N/A
        
        SELECT  @ComparisonValue = SUM(CASE WHEN ( ISNULL(oi.eInvoiceQuantity,0.0)
                                                   - ISNULL(oi.QuantityReceived,
                                                            0.0) ) = 0.0 THEN 0.0
                                            ELSE 1
                                       END)
               ,@IsEinvoice = CASE WHEN oh.eInvoice_Id IS NULL THEN 0
                                   ELSE 1
                              END
        FROM    OrderHeader oh
                INNER JOIN OrderItem oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
        WHERE   oh.OrderHeader_ID = @OrderHeaderID
        GROUP BY oi.OrderHeader_ID, oh.eInvoice_Id
			
	
        IF @IsEinvoice = 0 
            SET @Return = 'N/A'
        ELSE 
            SET @Return = CASE WHEN @ComparisonValue <> 0.0 THEN 'Y'
                               ELSE 'N'
                          END
        RETURN @Return
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsQuantityMismatch] TO [IRMAClientRole]
    AS [dbo];

