CREATE FUNCTION [dbo].[fn_3WayMatchDetails]
(
    @ControlGroup_ID int,
	@PoNumber_Id int,
	@BeginDate DateTime,
	@EndDate DateTime
)

RETURNS @Table TABLE 
(
	OrderHeader_ID int,
	InvoiceNumber varchar(16),
    Vendor_ID int,
	MatchingValidationCode int
 )
AS
BEGIN 
IF @ControlGroup_ID IS NULL
		BEGIN
			 INSERT @Table   
			  -- Fetching the status directly from OrderHeader and OrderInvoice_Freight3Party Tables.
				  Select 
					OH.OrderHeader_ID, 
					OH.InvoiceNumber, 
					OH.Vendor_ID,
					OH.MatchingValidationCode
					from OrderHeader OH
					Where OH.OrderHeader_ID=ISNULL(@PoNumber_ID,OH.OrderHeader_ID)
					and (OH.MatchingDate >=ISNULL(@BeginDate,OH.MatchingDate) and OH.MatchingDate <=ISNULL(@EndDate,OH.MatchingDate))
			   union 
				   Select 
					OrderHeader_ID, 
					InvoiceNumber, 
					Vendor_ID, 
					MatchingValidationCode
					from OrderInvoice_Freight3Party
					Where OrderHeader_ID=ISNULL(@PoNumber_ID,OrderHeader_ID)
					and (MatchingDate >=ISNULL(@BeginDate,MatchingDate) and MatchingDate <=ISNULL(@EndDate,MatchingDate))
		END
ELSE
	BEGIN
		INSERT @Table           
		   Select 
				CGI.OrderHeader_ID,
				OH.InvoiceNumber,
				OH.Vendor_ID,
				OH.MatchingValidationCode
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
				where CGI.OrderInvoice_ControlGroup_ID=ISNULL(@ControlGroup_ID,CGI.OrderInvoice_ControlGroup_ID)
				and OH.OrderHeader_ID=ISNULL(@PoNumber_ID,OH.OrderHeader_ID)
				and (OH.MatchingDate >=ISNULL(@BeginDate,OH.MatchingDate) and OH.MatchingDate <=ISNULL(@EndDate,OH.MatchingDate))
		union 
				Select 
				CGI.OrderHeader_ID,
				TF.InvoiceNumber,
				TF.Vendor_ID,
				TF.MatchingValidationCode
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
				where CGI.OrderInvoice_ControlGroup_ID=ISNULL(@ControlGroup_ID,CGI.OrderInvoice_ControlGroup_ID)
				and TF.OrderHeader_ID=ISNULL(@PoNumber_ID,TF.OrderHeader_ID)
				and (TF.MatchingDate >=ISNULL(@BeginDate,TF.MatchingDate) and TF.MatchingDate <=ISNULL(@EndDate,TF.MatchingDate))
	  END
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_3WayMatchDetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_3WayMatchDetails] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_3WayMatchDetails] TO [IRMAReportsRole]
    AS [dbo];

