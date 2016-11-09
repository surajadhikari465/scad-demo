CREATE PROCEDURE [dbo].[EInvoicing_getPOsByPSVendor]
	@PSVendorId VARCHAR(50),
	@OriginalPO VARCHAR(50),
	@NewPO VARCHAR(50)
AS
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	Changed all reference to oh.DVOOrderID to oh.OrderExternalSourceOrderID
RDS		042010	12545	Removed <> criteria on OrderExternalSourceOrderID,
						Reformat SQL, Removed deprecated code
RDE		090511	1393	Removed Exclusion of Original PO. Wouldnt work if Einvoice supplied PO had characters in it.
MZ      030713  9264    Changed to incude closed but without invoice POs 
***********************************************************************************************/

BEGIN
	SELECT
		[PO]				=	oh.OrderHeader_Id,
	    [Source]			=	ISNULL(oex.[Description], 'IRMA'),
	    [Store_Name]		=	s.Store_Name,
	    [OrderDate]			=	oh.OrderDate,
	    [OrderHeaderDesc]	=	oh.OrderHeaderDesc
	FROM
	   Orderheader oh
	   INNER JOIN	Vendor v				ON  oh.vendor_id				= v.vendor_id
	   INNER JOIN	Vendor StoreVendor		ON  oh.PurchaseLocation_Id		= StoreVendor.vendor_id
	   INNER JOIN	Store s					ON  StoreVendor.store_no		= s.store_no
	   LEFT  JOIN	OrderExternalSource oex	ON	oh.OrderExternalSourceID	= oex.ID
	   LEFT  JOIN   OrderInvoice  oi        ON  oh.OrderHeader_Id           = oi.OrderHeader_Id
	WHERE 
		v.ps_export_Vendor_Id	= @PSVendorId -- only search the same vendor.
		AND oh.UploadedDate     IS NULL -- exclude Uploaded Orders
		AND oi.OrderHeader_Id   IS NULL -- exclude Orders with invoice
		AND oh.eInvoice_Id		IS NULL -- exclude Orders already associated w/ eInvoices.
		AND (oh.OrderHeader_ID	= @NewPO OR  oh.OrderExternalSourceOrderID = @NewPO)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getPOsByPSVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getPOsByPSVendor] TO [IRMAClientRole]
    AS [dbo];

