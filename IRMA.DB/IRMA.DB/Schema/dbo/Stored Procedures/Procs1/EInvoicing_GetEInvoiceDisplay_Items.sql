create PROCEDURE dbo.EInvoicing_GetEInvoiceDisplay_Items
	@EInvoiceId int	
AS
BEGIN
    SET NOCOUNT ON
    
    
    SELECT  ei.upc
           ,ei.vendor_item_num
           ,ei.descrip
           ,ei.brand
           ,ei.lot_num
           ,ei.case_uom
           ,ei.item_qtyper
           ,ei.item_uom
           ,ei.alt_ordering_qty
           ,ei.case_pack
           ,ei.qty_shipped
           ,ei.unit_cost
           ,ei.ext_cost
           ,ei.net_ext_cost
           ,ei.line_num
           ,ei.calc_net_ext_cost
           ,( SELECT    ISNULL(SUM(CAST(eid.ElementValue AS DECIMAL(18,4))
                                   * ChargeOrAllowance),0)
              FROM      EInvoicing_ItemData eid
                        INNER JOIN EInvoicing_Config ec ON eid.ElementName = ec.ElementName
                        INNER JOIN EInvoicing_SACTypes es ON ec.SacCodeType = es.SACType_Id
              WHERE     eid.EInvoice_Id = ei.EInvoice_id
                        AND eid.ItemId = ei.line_num
                        AND elementvalue IS NOT NULL
                        AND ec.IsSacCode = 1
                        AND es.SACType = 'Line Item'
            ) AS LineItemChargeOrAllowance
           ,ei.line_num
    FROM    EInvoicing_Item ei
    WHERE   ei.EInvoice_id = @EInvoiceId
    ORDER BY ei.line_num
  

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Items] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Items] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Items] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Items] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Items] TO [IRMAReportsRole]
    AS [dbo];

