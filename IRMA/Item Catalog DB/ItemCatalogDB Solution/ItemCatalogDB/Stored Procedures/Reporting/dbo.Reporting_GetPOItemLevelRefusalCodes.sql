/****** Object:  StoredProcedure [dbo].[Reporting_GetPOItemLevelRefusalCodes]    Script Date: 02/06/2012 10:32:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_GetPOItemLevelRefusalCodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_GetPOItemLevelRefusalCodes]
GO

/****** Object:  StoredProcedure [dbo].[Reporting_GetPOItemLevelRefusalCodes]    Script Date: 02/06/2012 10:33:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- **************************************************************************
-- Procedure: Reporting_GetPOItemLevelRefusalCodes
-- Author: trey d'amico
-- Date: 02/02/2012
--
-- Description: introduced in 4.4 to be used with the [PO Item Level Refusal Codes.rdl] report.
-- 05/03/2012	td	added to db project. 
-- **************************************************************************
CREATE PROCEDURE [dbo].[Reporting_GetPOItemLevelRefusalCodes] 
	@TimeFrame              NVARCHAR(MAX),
	@StartDate              DATETIME,
	@EndDate                DATETIME,
	@StoreNo                NVARCHAR(MAX),
	@SubteamNo              NVARCHAR(MAX),
	@VendorID               NVARCHAR(MAX),
	@ReceivingExceptionCode NVARCHAR(MAX)
AS
  BEGIN
      SET nocount ON;

      -- Item Level Refusal Codes Report
SELECT [UPC]										=	ii.identifier,
       [Description]								=	item_description,
       [Uploaded Cost]								=	( CASE
															 WHEN oh.uploadeddate IS NOT NULL THEN oh.apuploadedcost
															 ELSE 0
														   END ),
       [Net Received Quantity]						=	oit.quantityreceived,
       [eInvoice Quantity]							=	oit.einvoicequantity,
       [Ordered Quantity]							=	oit.quantityordered,
       [PO Number]									=	oh.orderheader_id,
       [DVO or POET NUMBER]							=	oh.dvoorderid,
       [Invoice Number]								=	oh.invoicenumber,
       [Receiving Refusal Discrepancy Code]			=	rdrcd.reasoncodedesc,
       [Receiver Name]								=	u.fullname,
       [Team]										=	st.subteam_name,
       [Vendor]										=	v.companyname,
       [Store]										=	vendorstore.companyname,
       [PO Received Date]							=	oit.datereceived,
       [PO Closed Date]								=	oh.closedate,
       [PO Sent Date]								=	oh.sentdate,
       [PO Admin Notes]								=	oh.adminnotes,
       [PO Notes]									=	oh.orderheaderdesc
FROM   dbo.orderheader oh (nolock)
       INNER JOIN dbo.orderitem oit (nolock)
         ON oh.orderheader_id = oit.orderheader_id
       INNER JOIN dbo.item i (nolock)
         ON oit.item_key = i.item_key
       INNER JOIN dbo.itemidentifier ii (nolock)
         ON i.item_key = ii.item_key
       INNER JOIN dbo.users u (nolock)
         ON oh.closedby = u.user_id
       INNER JOIN dbo.subteam st (nolock)
         ON oh.transfer_to_subteam = st.subteam_no
       INNER JOIN dbo.vendor v (nolock)
         ON oh.vendor_id = v.vendor_id
       INNER JOIN dbo.vendor vendorstore (nolock)
         ON oh.receivelocation_id = vendorstore.vendor_id
       INNER JOIN dbo.reasoncodedetail rdrcd (nolock)
         ON oit.receivingdiscrepancyreasoncodeid = rdrcd.reasoncodedetailid
WHERE  ( vendorstore.store_no IN (SELECT key_value
                                  FROM   dbo.Fn_parse_list(@StoreNo, ',')) )
       AND ( st.subteam_no IN (SELECT key_value
                               FROM   dbo.Fn_parse_list (@SubteamNo, ',')) )
       AND ( v.vendor_id IN (SELECT key_value
                             FROM   dbo.Fn_parse_list(@VendorID, ',')) )
       AND ( rdrcd.reasoncodedetailid IN (SELECT key_value
                                          FROM   dbo.Fn_parse_list(@ReceivingExceptionCode, ',')) )
       AND ( ( ( @TimeFrame = 'datereceived' )
               AND ( oit.datereceived BETWEEN @StartDate AND @EndDate ) )
              OR ( ( @TimeFrame = 'closedate' )
                   AND ( oh.closedate BETWEEN @StartDate AND @EndDate ) )
              OR ( ( @TimeFrame = 'sentdate' )
                   AND ( oh.sentdate BETWEEN @StartDate AND @EndDate ) ) ) 

  END 
       

GO


