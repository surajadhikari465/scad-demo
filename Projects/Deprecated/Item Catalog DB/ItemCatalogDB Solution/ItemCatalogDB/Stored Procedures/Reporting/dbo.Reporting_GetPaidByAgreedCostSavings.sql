/****** Object:  StoredProcedure [dbo].[Reporting_GetPaidByAgreedCostSavings]    Script Date: 09/14/2011 15:06:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_GetPaidByAgreedCostSavings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_GetPaidByAgreedCostSavings]
GO

/****** Object:  StoredProcedure [dbo].[Reporting_GetPaidByAgreedCostSavings]    Script Date: 09/14/2011 15:06:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- **************************************************************************
-- Procedure: Reporting_GetPaidByAgreedCostSavings
--    Author: trey d'amico
--      Date: 08/09/2011
--
-- Description: introduced in 4.3 to be used with the [Paid By Agreed Cost Savings.rdl] report.
-- (invoice amount) - (uploaded cost)
-- this query was stolen from the [GetOrderInfo] and [OrderInvoice_GetOrderInvoiceSACTotal] stored procedures.
-- please note SACTotal is SUM(oic.VALUE)

-- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/30/2011	BAS		3744	Updated to use new column OrderInvoice.InvoiceTotalCost
-- **************************************************************************

CREATE PROCEDURE [dbo].[Reporting_GetPaidByAgreedCostSavings]
   @StartDate DATETIME,
   @EndDate   DATETIME,
   @VendorID  nvarchar(max),
   @StoreNo   nvarchar(max),
   @SubteamNo nvarchar(max)
AS
BEGIN
    SET nocount ON;

DECLARE @SACType_Id INT

SELECT @SACType_Id = sactype_id
FROM   dbo.einvoicing_sactypes (nolock)
WHERE  sactype = 'Not Allocated'

SELECT [PONumber] = oh.orderheader_id,
       [Vendor] = v.companyname,
       [storecompanyname] = receivelocation.companyname,
       [subteam] = ttsubteam.subteam_name,
       [psvendorid] = v.ps_vendor_id,
       [psexportvendorid] = v.ps_export_vendor_id,
       [invoiceamount] = Isnull(oi.InvoiceTotalCost, 0),
       [uploadedcost] = oh.apuploadedcost
FROM   dbo.orderheader oh (nolock)
       INNER JOIN dbo.vendor v (nolock)
         ON v.vendor_id = oh.vendor_id
       INNER JOIN dbo.vendor receivelocation (nolock)
         ON receivelocation.vendor_id = oh.receivelocation_id
       LEFT JOIN dbo.subteam ttsubteam (nolock)
         ON ttsubteam.subteam_no = oh.transfer_to_subteam
       LEFT JOIN dbo.orderinvoice oi (nolock)
         ON oi.orderheader_id = oh.orderheader_id
       LEFT JOIN dbo.orderinvoicecharges oic (nolock)
         ON oic.orderheader_id = oh.orderheader_id
            AND oic.sactype_id = @SACType_Id
WHERE  ( oh.return_order = 0 )
       AND ( v.einvoicing = 1 )
       AND ( oh.paybyagreedcost = 1 )
       AND ( oh.apuploadedcost IS NOT NULL )
       AND ( oh.closedate BETWEEN @StartDate AND @EndDate )
       AND ( v.vendor_id IN (SELECT key_value
                             FROM   dbo.Fn_parse_list(@VendorID, ',')) )
       AND ( receivelocation.store_no IN (SELECT key_value
                                          FROM   dbo.Fn_parse_list(@StoreNo, ',')) )
       AND ( ttsubteam.subteam_no IN (SELECT key_value
                                      FROM   dbo.Fn_parse_list(@SubteamNo, ',')) )
GROUP  BY oh.orderheader_id,
          v.companyname,
          receivelocation.companyname,
          ttsubteam.subteam_name,
          v.ps_vendor_id,
          v.ps_export_vendor_id,
          oi.InvoiceTotalCost,
          oh.uploadeddate,
          oh.apuploadedcost
HAVING( Isnull(oi.InvoiceTotalCost, 0) - ( oh.apuploadedcost ) ) <> 0 

END
GO


