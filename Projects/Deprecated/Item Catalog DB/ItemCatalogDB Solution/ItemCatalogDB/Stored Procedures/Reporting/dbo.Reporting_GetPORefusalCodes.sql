/****** Object:  StoredProcedure [dbo].[Reporting_GetPORefusalCodes]    Script Date: 05/03/2012 10:02:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_GetPORefusalCodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_GetPORefusalCodes]
GO

/****** Object:  StoredProcedure [dbo].[Reporting_GetPORefusalCodes]    Script Date: 05/03/2012 10:02:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- **************************************************************************
-- Procedure: Reporting_GetPORefusalCodes
-- Author: trey d'amico
-- Date: 02/02/2012
--
-- Description: introduced in 4.4 to be used with the [PO Item Level Refusal Codes.rdl] report.
-- 02/08/2012	td		added 1 day to the enddate
-- 05/03/2012	td		made sql more readable
-- **************************************************************************
CREATE PROCEDURE [dbo].[Reporting_GetPORefusalCodes] 
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
SELECT [Team]						=	st.subteam_name,
       [PO Number]					=	oh.orderheader_id,
       [DVO or POET NUMBER]			=	oh.dvoorderid,
       [Receiver Name]				=	u.fullname,
       [Original Received Cost]		=	oh.originalreceivedcost,
       [Close Date]					=	oh.closedate,
       [Vendor]						=	v.companyname,
       [Store]						=	vendorstore.companyname,
       [Refuse Receiving Reason]	=	rrr.reasoncodedesc,
       [PO Notes]					=	oh.orderheaderdesc
FROM   dbo.orderheader oh (nolock)
       INNER JOIN dbo.users u (nolock)
         ON oh.closedby = u.user_id
       INNER JOIN dbo.subteam st (nolock)
         ON oh.transfer_to_subteam = st.subteam_no
       INNER JOIN dbo.vendor v (nolock)
         ON oh.vendor_id = v.vendor_id
       INNER JOIN dbo.vendor vendorstore (nolock)
         ON oh.receivelocation_id = vendorstore.vendor_id
       INNER JOIN dbo.reasoncodedetail rrr (nolock)
         ON oh.refusereceivingreasonid = rrr.reasoncodedetailid
WHERE  ( vendorstore.store_no IN (SELECT key_value
                                  FROM   dbo.Fn_parse_list(@StoreNo, ',')) )
       AND ( st.subteam_no IN (SELECT key_value
                               FROM   dbo.Fn_parse_list (@SubteamNo, ',')) )
       AND ( v.vendor_id IN (SELECT key_value
                             FROM   dbo.Fn_parse_list(@VendorID, ',')) )
       AND ( rrr.ReasonCodeDetailID IN (SELECT key_value
										FROM   dbo.Fn_parse_list(@ReceivingExceptionCode, ',')) )
       AND ( oh.closedate BETWEEN @StartDate AND DATEADD("day", 1, @EndDate) ) 

  END
GO


