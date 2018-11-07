-- ================================================================================
-- Author      :	Sekhara
-- Create date :    12/27/2007
-- Description :	3WayMatchDetailSummaryReport
-- =================================================================================

CREATE PROCEDURE [dbo].[ThreeWayMatchDetailSummaryReport]
@ControlGroup_ID int,
@PoNumber_Id int,
@BeginDate DateTime,
@EndDate DateTime
AS
BEGIN
	SET NOCOUNT ON
	IF @ControlGroup_ID IS NULL
  	BEGIN
       -- Fetching the status directly from OrderHeader and OrderInvoice_Freight3Party Tables.
		  Select 
			OH.OrderHeader_ID as 'PO Num',
			OH.InvoiceNumber as 'Invoice ID',
			OH.Vendor_ID as 'VendorID',
			OH.MatchingValidationCode as 'Status',
            VCT.Description as 'StatusType',
            VC.Description as 'Reason'
			from OrderHeader OH
            INNER JOIN ValidationCode VC ON
            OH.MatchingValidationCode=VC.ValidationCode
            INNER JOIN ValidationCodeType VCT ON
            VC.ValidationCodeType=VCT.ValidationCodeType
			Where OH.OrderHeader_ID=ISNULL(@PoNumber_ID,OH.OrderHeader_ID)
			and OH.MatchingValidationCode not in (select validationcode from validationcode where validationcodetype=2)
			and (OH.MatchingDate >=ISNULL(@BeginDate,OH.MatchingDate) and OH.MatchingDate <=ISNULL(@EndDate,OH.MatchingDate))
	   union 
	       Select 
			OrderHeader_ID as 'PO Num',
			InvoiceNumber as 'Invoice ID',
			Vendor_ID as 'VendorID',
			MatchingValidationCode as 'Status',
            VCT.Description as 'StatusType',
            VC.Description as 'Reason'
			from OrderInvoice_Freight3Party 
			INNER JOIN ValidationCode VC ON
            OrderInvoice_Freight3Party.MatchingValidationCode=VC.ValidationCode
            INNER JOIN ValidationCodeType VCT ON
            VC.ValidationCodeType=VCT.ValidationCodeType
			Where OrderHeader_ID=ISNULL(@PoNumber_ID,OrderHeader_ID)
			and MatchingValidationCode not in (select validationcode from validationcode where validationcodetype=2)
			and (MatchingDate >=ISNULL(@BeginDate,MatchingDate) and MatchingDate <=ISNULL(@EndDate,MatchingDate))
	END
	ELSE
	BEGIN
      -- Fetching from ControlGroup and OrderHeader and OrderInvoice_Freight3Party.
		Select 
			CGI.OrderHeader_ID as 'PO Num',
			OH.InvoiceNumber as 'Invoice ID',
			OH.Vendor_ID as 'VendorID',
			OH.MatchingValidationCode as 'Status',
            VCT.Description as 'StatusType', 
            VC.Description as 'Reason'
			-- Selecting only the Valid Invoices from the controlGroupinvoices 
			from (select OrderInvoice_ControlGroup_ID,
			OrderHeader_ID,
			InvoiceNumber,
			Vendor_ID
			from OrderInvoice_ControlGroupInvoice OC
			INNER JOIN dbo.ValidationCode V ON
			OC.ValidationCode=V.ValidationCode 
			where V.ValidationCodeType <> 2) as CGI
			INNER JOIN dbo.OrderHeader OH ON
			(CGI.InvoiceNumber=OH.InvoiceNumber
			and CGI.Vendor_ID=OH.Vendor_ID)
			INNER JOIN dbo.validationCode VC ON
			--To Display only Approved Invoices.
			(OH.MatchingValidationCode=VC.ValidationCode and OH.MatchingValidationCode not in (select validationcode from validationcode where validationcodetype=2))
			INNER JOIN ValidationCodeType VCT ON
            VC.ValidationCodeType=VCT.ValidationCodeType
            where CGI.OrderInvoice_ControlGroup_ID=
			ISNULL(@ControlGroup_ID,CGI.OrderInvoice_ControlGroup_ID)
			and OH.OrderHeader_ID=ISNULL(@PoNumber_ID,OH.OrderHeader_ID)
			and (OH.MatchingDate >=ISNULL(@BeginDate,OH.MatchingDate) and OH.MatchingDate <=ISNULL(@EndDate,OH.MatchingDate))
	   UNION 
		  Select 
			CGI.OrderHeader_ID as 'PO Num',
			TF.InvoiceNumber as 'Invoice ID',
			TF.Vendor_ID as 'VendorID',
			TF.MatchingValidationCode as 'Status',
            VCT.Description as 'StatusType',
            VC.Description as 'Reason'
			-- Selecting only the Valid Invoices from the controlGroupinvoices Table
			from (select OrderInvoice_ControlGroup_ID,
			OrderHeader_ID,
			InvoiceNumber,
			Vendor_ID
			from OrderInvoice_ControlGroupInvoice OC
			INNER JOIN dbo.ValidationCode V ON
			OC.ValidationCode=V.ValidationCode 
			where V.ValidationCodeType <> 2) as CGI
			INNER JOIN dbo.OrderInvoice_Freight3Party TF ON
			(CGI.InvoiceNumber=TF.InvoiceNumber
			and CGI.Vendor_ID=TF.Vendor_ID)
			INNER JOIN dbo.validationCode VC ON
			(TF.MatchingValidationCode=VC.ValidationCode and TF.MatchingValidationCode not in (select validationcode from validationcode where validationcodetype=2))
			INNER JOIN ValidationCodeType VCT ON
            VC.ValidationCodeType=VCT.ValidationCodeType
            where CGI.OrderInvoice_ControlGroup_ID=
			ISNULL(@ControlGroup_ID,CGI.OrderInvoice_ControlGroup_ID)
			and TF.OrderHeader_ID=ISNULL(@PoNumber_ID,TF.OrderHeader_ID)
			and (TF.MatchingDate >=ISNULL(@BeginDate,TF.MatchingDate) and TF.MatchingDate <=ISNULL(@EndDate,TF.MatchingDate))
	  SET NOCOUNT OFF
	END
END
SET QUOTED_IDENTIFIER OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThreeWayMatchDetailSummaryReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThreeWayMatchDetailSummaryReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThreeWayMatchDetailSummaryReport] TO [IRMAReportsRole]
    AS [dbo];

