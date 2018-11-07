CREATE PROCEDURE [dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC]
    @OrderId INT
   ,@StoreNo INT
AS
 /*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	Changed all reference to DVOOrderID to OrderExternalSourceOrderID

**********************************************************************************************/

    BEGIN
        SET NOCOUNT ON
    
        DECLARE @po_num VARCHAR(30)
-- If a DVO Order, use DVOOrderId instead.
--set @po_num= (select isnull(DVOOrderId, OrderHeader_Id) from OrderHeader where OrderHeader_Id = @OrderId)
        SET @po_num = ( SELECT  ISNULL(OrderExternalSourceOrderID,
                                       OrderHeader_Id)
                        FROM    OrderHeader
                        WHERE   OrderHeader_Id = @OrderId
                      )



    
--Get Item Detail SAC Charges
        SELECT  EC.Label
               ,EC.SacCodeType
               ,ST.Subteam_Name
               ,ST.GLPurchaseAcct
               ,EC.IsSacCode
               ,ES.SACType
               ,SUM(CAST(CASE WHEN ISNUMERIC(ei.elementvalue) = 1
                              THEN ei.elementValue
                              ELSE 0.0
                         END AS DECIMAL(11,4))) AS Total
        FROM    einvoicing_itemdata EI ( NOLOCK )
                JOIN Einvoicing_Header EH ( NOLOCK ) ON EH.einvoice_id = EI.einvoice_id
                JOIN Einvoicing_config AS EC ( NOLOCK ) ON EI.ElementName = EC.ElementName
                JOIN einvoicing_sactypes AS ES ( NOLOCK ) ON ES.SACType_ID = EC.SacCodeType
                LEFT JOIN Subteam AS ST ( NOLOCK ) ON EC.Subteam_no = ST.Subteam_No
                INNER JOIN Store AS S ( NOLOCK ) ON EH.Store_num = S.BusinessUnit_Id
                JOIN dbo.orderheader oh ( NOLOCK ) ON eh.invoice_num = oh.invoicenumber
                JOIN dbo.OrderInvoiceCharges OIC ( NOLOCK ) ON oic.OrderHeader_Id = oh.OrderHeader_Id
        WHERE   elementvalue IS NOT NULL
                AND eh.po_num = @PO_Num
                AND EI.ElementName = EC.ElementName
                AND EC.IsSacCode = 1
                AND s.Store_No = @StoreNo
        GROUP BY EC.Label
               ,EC.SacCodeType
               ,ST.Subteam_Name
               ,ST.GLPurchaseAcct
               ,EC.IsSacCode
               ,ES.SACType


        SET NOCOUNT OFF
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_ItemAllocatedSAC] TO [IRMAReportsRole]
    AS [dbo];

