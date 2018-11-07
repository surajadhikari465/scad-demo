-- **************************************************************************
-- Procedure:	Reporting_GetPOAdminReasonCodesInSPOT
--    Author:	trey d'amico
--      Date:	03/07/2012
--
-- Description:	TFS 5464, introduced in 4.4.2 to be used with the [PO Admin Reason Codes in SPOT.rdl] report.
-- 
-- Modification History:
-- Date			Init	TFS		Comment
-- 04/16/201	td		5464	Added OrderHeaderID Parameter.
--								
-- **************************************************************************

CREATE PROCEDURE [dbo].[Reporting_GetPOAdminReasonCodesInSPOT] 
		@StoreNo		NVARCHAR(MAX),
        @VendorID		NVARCHAR(MAX),
        @SubteamNo		NVARCHAR(MAX),
        @PaymentType	INT,-- 0 = 'Pay By Invoice', 1 = 'Pay By PO'
        @Identifier		VARCHAR(15) = NULL,
        @OrderHeaderID	INT = NULL,
        @DiffLoBound	FLOAT = NULL,
        @DiffUpBound	FLOAT = NULL,
        @VIN			VARCHAR(15) = NULL,
        @TimeFrame		INT,  -- 1 = OrderDate, 2 = InvoiceDate, 3 = CloseDate, 4 = ApprovedDate
        @DateStart		SMALLDATETIME,
        @DateEnd		SMALLDATETIME,
        @ReasonCodeID	NVARCHAR(MAX)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	SET NOCOUNT ON;
SELECT @DateEnd = @DateEnd+' 23:59:59' --dateadd("day", 1, @DateEnd)

SELECT [CloseDate]				= oh.closedate,
       [Store]					= s.Store_Name,
       [Vendor]					= v.companyname,
       [PO Number]				= oi.orderheader_id,
       [Order Date]				= oh.orderdate,
       [Invoice Number]			= oh.invoicenumber,
       [Invoice Date]			= oh.invoicedate,
       [Payment Type]			= CASE WHEN oi.paymenttypeid = 1 THEN 'Pay By PO' ELSE 'Pay By Invoice' END,
       [Resolution Code]		= oiprc.reasoncodedesc,
       [PO Admin Notes]			= oi.adminnotes,
       [Discrepant Y/N]			= CASE WHEN oi.lineitemsuspended = 1 THEN 'Y' ELSE 'N' END,
       [Identifier]				= ii.identifier,
       [VIN]					= iv.item_id,
       [Description]			= i.item_description,
       [Brand]					= ib.brand_name,
       [SubTeam]				= st.subteam_name,
       [PO Cost]				= oi.COST,
       [EInvoice Unit Cost]		= oi.invoicecost,
       [PO Effective Cost]		= CASE WHEN oi.adjustedcost > 0 THEN oi.adjustedcost ELSE oi.markupcost END,
       [PO/Inv Cost Diff]		= CASE WHEN oi.adjustedcost > 0 THEN oi.invoicecost - oi.adjustedcost ELSE oi.invoicecost - oi.markupcost END,
       [PO Adjusted Cost]		= oi.adjustedcost,
       [Adjusted Cost Reason]	= CASE WHEN rci.reasoncodedesc IS NOT NULL THEN rci.reasoncodedesc ELSE isnull(rch.reasoncodedesc, '') END,
       [Ordered Quantity]		= oi.quantityordered,
       [Received Quantity]		= oi.quantityreceived,
       [eInvoice Quantity]		= oi.einvoicequantity,
       [Receiver Reason Code]	= isnull(rcr.reasoncodedesc, ''),
       [IRMA Current Cost]		= dbo.fn_getcurrentnetcost(oi.item_key, vs.store_no),
       [Cost Effective Date]	= vc.startdate,
       [Cost Insert Date]		= vc.insertdate,
       [Approved Date]			= oi.approveddate,
       [PO Cost UOM]			= iuc.unit_name,
       [eInvoice UOM]			= iue.unit_name,
       [Vendor Order UOM]		= iuv.unit_name,
       [PO Case Pack]			= vc.package_desc1,
       [Einvoice Case Pack]		= eii.case_pack
FROM   dbo.orderheader (nolock) oh
       INNER JOIN dbo.orderitem (nolock) oi
         ON oh.orderheader_id = oi.orderheader_id
       INNER JOIN dbo.item (nolock) i
         ON oi.item_key = i.item_key
       INNER JOIN dbo.itemidentifier (nolock) ii
         ON i.item_key = ii.item_key
            AND ii.default_identifier = 1
       INNER JOIN dbo.vendor (nolock) v
         ON oh.vendor_id = v.vendor_id
       INNER JOIN dbo.vendor (nolock) vs
         ON oh.purchaselocation_id = vs.vendor_id
       INNER JOIN dbo.store (nolock) s
         ON vs.store_no = s.store_no
       INNER JOIN dbo.subteam (nolock) st
         ON i.subteam_no = st.subteam_no
       INNER JOIN dbo.itembrand (nolock) ib
         ON i.brand_id = ib.brand_id
       INNER JOIN dbo.itemunit (nolock) iuv
         ON i.vendor_unit_id = iuv.unit_id
       INNER JOIN dbo.itemunit (nolock) iuc
         ON oi.costunit = iuc.unit_id
       INNER JOIN dbo.itemvendor (nolock) iv
         ON i.item_key = iv.item_key
            AND v.vendor_id = iv.vendor_id
       LEFT JOIN dbo.vendorcosthistory (nolock) vc
         ON oi.vendorcosthistoryid = vc.vendorcosthistoryid
       LEFT JOIN dbo.einvoicing_item (nolock) eii
         ON oi.item_key = eii.item_key
            AND oh.einvoice_id = eii.einvoice_id
       LEFT JOIN dbo.itemunit (nolock) iue
         ON eii.case_uom = iue.unit_abbreviation
       LEFT JOIN dbo.reasoncodedetail (nolock) rch
         ON oh.reasoncodedetailid = rch.reasoncodedetailid
       LEFT JOIN dbo.reasoncodedetail (nolock) rci
         ON oi.reasoncodedetailid = rci.reasoncodedetailid
       LEFT JOIN dbo.reasoncodedetail (nolock) rcr
         ON oi.receivingdiscrepancyreasoncodeid = rcr.reasoncodedetailid
       LEFT JOIN dbo.reasoncodedetail (nolock) oiprc
         ON oi.resolutioncodeid = oiprc.reasoncodedetailid 
WHERE  ( vs.store_no IN (SELECT key_value
                         FROM   dbo.Fn_parse_list(@StoreNo, ',')) )
       AND ( v.vendor_id IN (SELECT key_value
                             FROM   dbo.Fn_parse_list(@VendorID, ',')) )
       AND ( oh.transfer_to_subteam IN (SELECT key_value
                                        FROM   dbo.Fn_parse_list (@SubteamNo, ',')) )
       AND ( ( @TimeFrame IS NULL
               AND ( ( oh.orderdate BETWEEN @DateStart AND @DateEnd )
                      OR ( oh.invoicedate BETWEEN @DateStart AND @DateEnd )
                      OR ( oh.closedate BETWEEN @DateStart AND @DateEnd )
                      OR ( oh.approveddate BETWEEN @DateStart AND @DateEnd ) ) )
              OR ( @TimeFrame = 1 AND oh.orderdate BETWEEN @DateStart AND @DateEnd )
              OR ( @TimeFrame = 2 AND oh.invoicedate BETWEEN @DateStart AND @DateEnd )
              OR ( @TimeFrame = 3 AND oh.closedate BETWEEN @DateStart AND @DateEnd )
              OR ( @TimeFrame = 4 AND oh.approveddate BETWEEN @DateStart AND @DateEnd ) )
       AND ( @Identifier IS NULL
              OR ( @Identifier IS NOT NULL AND ii.identifier = @Identifier ) )
       AND ( @OrderHeaderID IS NULL
              OR ( @OrderHeaderID IS NOT NULL AND oh.OrderHeader_ID = @OrderHeaderID ) )
       AND ( @VIN IS NULL
              OR ( @VIN IS NOT NULL AND iv.item_id = @VIN ) )
       AND ( @PaymentType IS NULL
              OR ( @PaymentType IS NOT NULL AND oi.paymenttypeid = @PaymentType ) )
       AND ( ( @DiffLoBound IS NULL AND @DiffUpBound IS NULL )
              OR ( ( @DiffLoBound IS NULL AND @DiffUpBound IS NOT NULL )
                   AND ( ( ( oi.adjustedcost > 0 )
                           AND ( oi.invoicecost - oi.adjustedcost BETWEEN -Abs(@DiffUpBound) AND Abs(@DiffUpBound) ) )
                          OR ( ( oi.adjustedcost <= 0 )
                               AND ( oi.invoicecost - oi.markupcost BETWEEN -Abs(@DiffUpBound) AND Abs(@DiffUpBound) ) ) ) )
              OR ( ( @DiffLoBound IS NOT NULL AND @DiffUpBound IS NULL )
                   AND ( ( ( oi.adjustedcost > 0 )
                           AND ( oi.invoicecost - oi.adjustedcost BETWEEN -Abs(@DiffLoBound) AND Abs(@DiffLoBound) ) )
                          OR ( ( oi.adjustedcost <= 0 )
                               AND ( oi.invoicecost - oi.markupcost BETWEEN -Abs(@DiffLoBound) AND Abs(@DiffLoBound) ) ) ) )
              OR ( ( @DiffLoBound IS NOT NULL AND @DiffUpBound IS NOT NULL )
                   AND ( ( ( oi.adjustedcost > 0 )
                           AND ( oi.invoicecost - oi.adjustedcost BETWEEN @DiffLoBound AND @DiffUpBound ) )
                          OR ( ( oi.adjustedcost <= 0 )
                               AND ( oi.invoicecost - oi.markupcost BETWEEN @DiffLoBound AND @DiffUpBound ) ) ) ) )
       AND ( ( rch.reasoncodedetailid IN (SELECT key_value
                                          FROM   dbo.Fn_parse_list (@ReasonCodeID, ',')) )
              OR ( rci.reasoncodedetailid IN (SELECT key_value
                                              FROM   dbo.Fn_parse_list (@ReasonCodeID, ',')) )
              OR ( rcr.reasoncodedetailid IN (SELECT key_value
                                              FROM   dbo.Fn_parse_list (@ReasonCodeID, ',')) )
              OR ( oiprc.reasoncodedetailid IN (SELECT key_value
                                                FROM   dbo.Fn_parse_list (@ReasonCodeID, ',')) ) ) 
ORDER  BY oh.OrderHeader_ID, oi.orderitem_id
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPOAdminReasonCodesInSPOT] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPOAdminReasonCodesInSPOT] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPOAdminReasonCodesInSPOT] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPOAdminReasonCodesInSPOT] TO [IRMAReportsRole]
    AS [dbo];

