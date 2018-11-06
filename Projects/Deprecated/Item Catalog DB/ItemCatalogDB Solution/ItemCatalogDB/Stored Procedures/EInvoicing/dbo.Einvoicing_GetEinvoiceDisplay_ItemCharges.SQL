 

IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   NAME = 'EInvoicing_GetEInvoiceDisplay_ItemCharges' ) 
    DROP PROCEDURE EInvoicing_GetEInvoiceDisplay_ItemCharges
  
  GO
  
CREATE PROCEDURE dbo.EInvoicing_GetEInvoiceDisplay_ItemCharges
    @EInvoiceId INT
   ,@ItemId INT
AS 
    BEGIN
        SET NOCOUNT ON  
	
	
        
        SELECT  ISNULL(ec.Label,eid.ElementName) AS ChargeName
               ,SUM(CAST(eid.ElementValue AS DECIMAL(18,4))
                    * ChargeOrAllowance) AS ChargeValue
        FROM    EInvoicing_ItemData eid
                INNER JOIN EInvoicing_Config ec ON eid.ElementName = ec.ElementName
        WHERE   eid.EInvoice_Id = @EinvoiceId
                AND eid.ItemId = @ItemId
                AND eid.ElementValue IS NOT NULL
                AND ec.ExcludeFromCalculations = 0
        GROUP BY eid.ElementName
               ,ec.Label 
	
        SET NOCOUNT OFF
    END
GO
