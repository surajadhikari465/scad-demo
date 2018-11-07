CREATE PROCEDURE dbo.EInvoicing_getEInvoices
	@InvoiceStartDate	smalldatetime, 
	@InvoiceEndDate		smalldatetime,
	@ImportStartDate	smalldatetime, 
	@ImportEndDate		smalldatetime,
	@PONum				varchar(100),
	@InvoiceNum			varchar(100),
	@PSVendorID			varchar(10),
	@BusinessUnit		integer,
	@ErrorCodeId		integer, 
	@Status				varchar(100),
	@Archived			integer
AS
-- **************************************************************************
-- Procedure: EInvoicing_getEInvoices()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from the eInvoicing DAO to return a list eInvoices.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 02.22.12		BBB   	4174	modified PSVendorID var to varchar(10) to match data schema; 
--								modified WHERE to look at v.Vendor_PS_ID which is source of column in UI;
-- 2012-03-19	KM		5324	Modify WHERE to properly return archived results;
-- 2013-02-15	DN		10880	Added an extra field in the query OH_Einvoice_Id to help determine if there's
--								an eInvoice has already been matched to a PO. 
-- 2013-02-25	DN		11156	Modified the join condition
-- 2013-03-22   MZ      11640   Reversed the change made for TFS 10880 & 11156
-- **************************************************************************
BEGIN
	/*
		Declare local variables to prevent parameter sniffing: http://www.sqlpointers.com/2006/11/parameter-sniffing-stored-procedures.html
	*/
	declare	@_InvoiceStartDate	smalldatetime 
	declare @_InvoiceEndDate	smalldatetime
	declare @_ImportStartDate	smalldatetime
	declare @_ImportEndDate		smalldatetime
	declare @_PONum				varchar(100)
	declare @_InvoiceNum		varchar(100)
	declare @_PSVendorId		varchar(10)
	Declare @_BusinessUnit		integer
	declare @_ErrorCodeId		integer 
	declare @_Status			varchar(100)
	declare @_Archived			integer
		
	set	@_InvoiceStartDate	= @InvoiceStartDate
	set @_InvoiceEndDate	= @InvoiceEndDate
	set @_ImportStartDate	= @ImportStartDate
	set @_ImportEndDate		= @ImportEndDate
	set @_PONum				= @PONum
	set @_InvoiceNum		= @InvoiceNum
	set @_PSVendorID		= @PSVendorID
	set @_BusinessUnit		= @BusinessUnit
	set @_ErrorCodeId		= @ErrorCodeId
	set @_Status			= @Status
	set @_Archived			= @Archived
	
	DECLARE @Results TABLE
    (
        Invoice			VARCHAR(100),
        store_no		INT,
        Store			VARCHAR(100),
        PurchaseOrder	VARCHAR(100),
        InvoiceDate		smalldatetime,
        eInvoice_id		INT,
        Status			VARCHAR(100),
        ErrorCode_Id	INT,
        ImportDate		smalldatetime,
        VendorId		VARCHAR(10),
        Vendor			VARCHAR(100),
        ErrorMessage	VARCHAR(255),
        Archived		BIT,
        ArchivedDate	DATETIME
    )
	
	INSERT INTO @Results
	SELECT TOP 2000 
		Invoice			= Invoice_Num,
		ei.store_no,
		Store			= Store_Name,
		PurchaseOrder	= ei.po_num,
		ei.InvoiceDate,
		ei.eInvoice_id,
		Status			= ISNULL(ei.Status, 'Success'),
		ei.ErrorCode_Id,
		EI.ImportDate,
		VendorId		= ei.PSVendor_Id,
		Vendor			= ISNULL(v.CompanyName, 'Unavailable'),
		ec.ErrorMessage,
		ei.archived,
		ei.ArchivedDate
	FROM   
		EInvoicing_Invoices				(NOLOCK) ei
		LEFT JOIN EInvoicing_errorCodes	(NOLOCK) ec		ON ei.ErrorCode_id			= ec.ErrorCode_Id
		LEFT JOIN Store					(NOLOCK) s      ON ei.Store_no				= s.BusinessUnit_ID
		LEFT JOIN Vendor				(NOLOCK) v      ON ei.psvendor_id_padded	= v.PS_Export_Vendor_ID
	WHERE 
		ei.InvoiceDate BETWEEN @_InvoiceStartDate AND @_InvoiceEndDate
		
		AND (@_ImportStartDate IS NULL  
				OR ( @_ImportStartDate IS NOT NULL 
					AND ei.ImportDate BETWEEN @_ImportStartDate and @_ImportEndDate))
		
		AND (@_PONum IS NULL 
				OR (@_PONum IS NOT NULL	
					AND ei.po_num LIKE '%' + @_PONum + '%'))

		
		AND (@_InvoiceNum IS NULL 
				OR (@_InvoiceNum IS NOT NULL	
					AND ei.invoice_num LIKE '%' + @_InvoiceNum + '%'))

		AND (@_BusinessUnit IS NULL 
				OR (@_BusinessUnit IS NOT NULL 
						AND ei.Store_No = @_BusinessUnit))

		AND (@_PSVendorId IS NULL 
				OR (@_PSVendorId IS NOT NULL 
						AND v.PS_Vendor_ID = @_PSVendorId))

		AND (@_ErrorCodeId IS NULL
				OR (@_ErrorCodeId IS NOT NULL 
						AND ei.ErrorCode_id = @_ErrorCodeId))
		
		AND  (@_Archived IS NULL AND (ei.Archived = 0 OR ei.Archived IS NULL)
				OR (@_Archived IS NOT NULL 
						AND ei.Archived = @_Archived))

	ORDER BY
		ei.Einvoice_Id DESC
		   
		IF @_Status IS NULL
			SELECT * FROM @Results
		ELSE
			SELECT * FROM @Results WHERE Status = @_Status
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getEInvoices] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getEInvoices] TO [IRMAClientRole]
    AS [dbo];

