-- **************************************************************************
-- Procedure: Reporting_GetPostPaymentDiscrepancyResolution
--    Author: trey d'amico
--      Date: 09/14/2011
--
-- Description: introduced in 4.3 to be used with the [Paid by Agreed Cost Post Payment Discrepancy Resolution.rdl] report.
-- (invoice amount) - (uploaded cost)
-- 
-- Modification History:
-- Date			Init	TFS		Comment
-- 12/30/2011	BAS		3744	Updated to use new column OrderInvoice.InvoiceTotalCost
--								instead of the aggregation of three fields
-- 04/27/2012	TD		6057	mapped to ReasonCodeDetail rather than ResolutionCode
--								tabbed the select statement for better readability
-- 09/13/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************

CREATE PROCEDURE [dbo].[Reporting_GetPostPaymentDiscrepancyResolution]
   @StartDate						DATETIME,
   @EndDate							DATETIME,
   @CostAdjustmentReasonOrderLevel	nvarchar(max),
   @CostAdjustmentReasonItemLevel	nvarchar(max),
   @ReceivingExceptionCode			nvarchar(max),
   @POAdminResolutionCode			nvarchar(max),
   @VendorID						nvarchar(max),
   @StoreNo							nvarchar(max)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    SET nocount ON;

    DECLARE @SACType_Id INT

    SELECT @SACType_Id = sactype_id
    FROM   dbo.einvoicing_sactypes (nolock)
    WHERE  sactype = 'Not Allocated'

    SELECT DISTINCT [Vendor]								=	v.companyname,
                    [PS Vendor ID]							=	v.ps_vendor_id,
                    [PS Export ID]							=	v.ps_export_vendor_id,
                    [Store Name]							=	vstore.companyname,
                    [Store BU Number]						=	s.businessunit_id,
                    [Invoice Number]						=	oh.invoicenumber,
                    [Invoice Date]							=	oh.invoicedate,
                    [PO Number]								=	oh.orderheader_id,
                    [Order Date]							=	oh.orderdate,
                    [Close Date]							=	oh.closedate,
                    [Received Date]							=	oh.recvlogdate,
                    [Invoice Amount]						=	Isnull(oiv.InvoiceTotalCost, 0),
                    [Uploaded Cost]							=	( CASE
																	  WHEN oh.uploadeddate IS NOT NULL THEN oh.apuploadedcost
																	  ELSE 0
																	END ),
                    [Difference]							=	( Isnull(oiv.InvoiceTotalCost, 0) ) - ((	CASE
																											WHEN oh.uploadeddate IS NOT NULL THEN oh.apuploadedcost
																											ELSE 0
																										END )),
                    [Item Identifier]						=	ii.identifier,
                    [Item Vin]								=	iv.item_id,
                    [Order UOM]								=	ou.unit_abbreviation,
                    [Order UOM Cost]						=	Isnull(oi.COST, 0),
                    [Invoice UOM]							=	ei.case_uom,
                    [Invoice UOM cost]						=	ei.unit_cost,
                    [Shipped Quantity]						=	Isnull(oi.einvoicequantity, 0),
                    [Received Quantity]						=	Isnull(oi.quantityreceived, 0),
                    [Quantity Difference]					=	( Isnull(oi.einvoicequantity, 0) - Isnull(oi.quantityreceived, 0) ),
                    [Received Ext Cost]						=	( Isnull(oi.receiveditemcost, 0) + Isnull(oi.receiveditemfreight, 0) + Isnull(oi.receiveditemhandling, 0) ),
                    [Invoice Ext Cost]						=	oi.invoiceextendedcost,
                    [Cost Adjustment Reason Order Level]	=	carol.reasoncodedesc,
                    [Receiving Exception Code]				=	rec.reasoncodedesc,
                    [Cost Adjustment Reason Item Level]		=	caril.reasoncodedesc,
                    [PO Admin Resolution Code]				=	resi.ReasonCodeDesc,
                    [PO Admin Resolution Notes]				=	oi.adminnotes
    FROM   dbo.orderheader oh (nolock)
           INNER JOIN dbo.vendor v (nolock)
             ON v.vendor_id = oh.vendor_id
           INNER JOIN dbo.vendor vstore (nolock)
             ON vstore.vendor_id = oh.receivelocation_id
           LEFT JOIN dbo.subteam ttsubteam (nolock)
             ON ttsubteam.subteam_no = oh.transfer_to_subteam
           LEFT JOIN dbo.orderitem oi (nolock)
             ON oi.orderheader_id = oh.orderheader_id
           LEFT JOIN dbo.orderinvoice oiv (nolock)
             ON oiv.orderheader_id = oh.orderheader_id
           LEFT JOIN dbo.orderinvoicecharges oic (nolock)
             ON oic.orderheader_id = oh.orderheader_id
                AND oic.sactype_id = @SACType_Id
           LEFT JOIN dbo.reasoncodedetail carol (nolock)
             ON carol.reasoncodedetailid = oh.reasoncodedetailid -- cost adjustment reason orderheader level
           LEFT JOIN dbo.reasoncodedetail caril (nolock)
             ON caril.reasoncodedetailid = oi.reasoncodedetailid -- cost adjustment reason orderitem level
           LEFT JOIN dbo.reasoncodedetail rec (nolock)
             ON rec.reasoncodedetailid = oi.receivingdiscrepancyreasoncodeid -- receiving exception code orderitem level
           LEFT JOIN dbo.ReasonCodeDetail resi (nolock)
             ON resi.ReasonCodeDetailID = oi.resolutioncodeid -- PO Item Level Admin Resolution code 
           LEFT JOIN dbo.einvoicing_item ei (nolock)
             ON ei.einvoice_id = oh.einvoice_id
                AND ei.item_key = oi.item_key
           INNER JOIN dbo.itemidentifier ii (nolock)
             ON ii.item_key = oi.item_key
           INNER JOIN dbo.itemvendor iv (nolock)
             ON iv.item_key = oi.item_key
                AND iv.vendor_id = oh.vendor_id
           INNER JOIN dbo.store s (nolock)
             ON s.store_no = vstore.store_no
           INNER JOIN dbo.itemunit ou (nolock)
             ON ou.unit_id = oi.quantityunit
    WHERE  ( oh.return_order = 0 )
           AND ( v.einvoicing = 1 )
           AND ( oh.paybyagreedcost = 1 )
           AND ( oh.apuploadedcost IS NOT NULL )
           AND ( oh.closedate BETWEEN @StartDate AND @EndDate )
           AND ( v.vendor_id IN (SELECT key_value
                                 FROM   dbo.Fn_parse_list(@VendorID, ',')) )
           AND ( s.store_no IN (SELECT key_value
                                FROM   dbo.Fn_parse_list(@StoreNo, ',')) )
           AND ( ( carol.reasoncodedetailid IN (SELECT key_value
                                                FROM   dbo.Fn_parse_list(@CostAdjustmentReasonOrderLevel, ',')) )
                  OR ( caril.reasoncodedetailid IN (SELECT key_value
                                                    FROM   dbo.Fn_parse_list(@CostAdjustmentReasonItemLevel, ',')) )
                  OR ( rec.reasoncodedetailid IN (SELECT key_value
                                                  FROM   dbo.Fn_parse_list(@ReceivingExceptionCode, ',')) )
                  OR ( resi.ReasonCodeDetailID IN (SELECT key_value
                                                FROM   dbo.Fn_parse_list(@POAdminResolutionCode, ',')) ) )
    GROUP  BY v.companyname,
              v.ps_vendor_id,
              v.ps_export_vendor_id,
              vstore.companyname,
              s.businessunit_id,
              oh.invoicenumber,
              oh.invoicedate,
              oh.orderheader_id,
              oh.orderdate,
              oh.closedate,
              oh.recvlogdate,
              oiv.InvoiceTotalCost,
              oh.uploadeddate,
              oh.apuploadedcost,
              ii.identifier,
              iv.item_id,
              ou.unit_abbreviation,
              oi.COST,
              ei.case_uom,
              ei.unit_cost,
              oi.einvoicequantity,
              oi.quantityreceived,
              oi.receiveditemcost,
              oi.receiveditemfreight,
              oi.receiveditemhandling,
              oi.invoiceextendedcost,
              carol.reasoncodedesc,
              rec.reasoncodedesc,
              caril.reasoncodedesc,
              resi.ReasonCodeDesc,
              oi.AdminNotes
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPostPaymentDiscrepancyResolution] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPostPaymentDiscrepancyResolution] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPostPaymentDiscrepancyResolution] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetPostPaymentDiscrepancyResolution] TO [IRMAReportsRole]
    AS [dbo];

