CREATE PROCEDURE [dbo].[AlertBuyerAbout_ED_SS_OOS]
	@InvoiceId int
AS
   -- **************************************************************************
   -- Procedure: AlertBuyerAbout_ED_SS_OOS()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from EInvoicing_MatchEInvoiceToPurchaseOrder to
   -- trigger an email to the buyer based upon certain issues (ED, SS, OOS)
   --
   -- Modification History:
   -- Date        Init	TFS		Comment
   -- 04/20/2010  BBB	12498	Updated SP to be more readable; corrected issue
   --							with rowCount in SS logic; corrected issue with
   --							NULL value in OSS logic; corrected issue with header
   --							logic affected ED output; made all output generic
   --							regardless of item count; made ED logic match OSS/SS
   -- **************************************************************************
BEGIN  
 BEGIN TRY
	--**************************************************************************
	-- Declare internal variables
	--************************************************************************** 
	DECLARE @OrderHeader_Id					varchar(100)
	DECLARE @UnitID							int 
	DECLARE @PoundID						int
	DECLARE @BUYEREMAIL						varchar(250)
	DECLARE @IssuesShortShipText			varchar(1000)
	DECLARE @ISSUEOUTOFSTOCKText			varchar(1000)
	DECLARE @Expected_Date					smalldatetime
	DECLARE @InvoiceExpected_Date			smalldatetime
	DECLARE @IssuesExpectedDateDifferent	bit 
	DECLARE	@IssuesShortShip				bit
	DECLARE @IssuesOutOfStock				bit
	DECLARE @IssuesShortShipErrorMsg		varchar(1000)
	DECLARE @ERRORCOUNT						int
	DECLARE @ERRORMESSAGE					varchar(2000)
	DECLARE @IssuesExpectedDateText			varchar(1000)
 
	--**************************************************************************
	-- Set internal variables
	--************************************************************************** 
	SET @Expected_Date			= NULL
	SET @InvoiceExpected_Date	= NULL
	SET @ERRORCOUNT				= 0
	SELECT @UnitID				= Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'EA'      
	SELECT @PoundID				= Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB'  

	SET @OrderHeader_Id			= (SELECT TOP 1 OrderHeader_Id FROM ORDERHEADER WHERE eInvoice_Id = @InvoiceId AND Expected_Date IS NOT NULL)
 
 	--**************************************************************************
	-- Expected Date check (ED)
	--************************************************************************** 
	SELECT    
		@Expected_Date			= oh.Expected_Date,
		@InvoiceExpected_Date	= eh.est_delivery_date,
		@IssuesExpectedDateText = 'The requested date is ' + CONVERT(varchar(255),oh.Expected_Date) + '.' + CHAR(13) + CHAR(10),
		@IssuesExpectedDateText = isnull(@IssuesExpectedDateText,'') + 'The expected date from the vendor is ' + CONVERT(varchar(255),eh.est_delivery_date) + '. ' + CHAR(13) + CHAR(10),
		@BUYEREMAIL				= ISNULL(u.EMail, '')
    FROM
		OrderHeader						(nolock) oh	
		INNER JOIN	einvoicing_header	(nolock) eh	ON	oh.Einvoice_id	= eh.Einvoice_id
		INNER JOIN	Users				(nolock) u	ON	u.User_ID		= oh.CreatedBy
	WHERE 
		oh.OrderHeader_ID = @OrderHeader_Id
		AND Expected_Date IS NOT NULL

	-- Output generation
	IF @InvoiceExpected_Date IS NOT NULL AND @Expected_Date <> @InvoiceExpected_Date
		BEGIN
			SET @IssuesExpectedDateDifferent = 1
			SET @ERRORCOUNT					 = 1
		END
	ELSE
		BEGIN
			SET @IssuesExpectedDateDifferent = 0
		END

 	--**************************************************************************
	-- Short Ship check (SS)
	--************************************************************************** 
	SELECT    
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + 'Item = ' + ii.Identifier + ',   ',
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + ' Description = ' + Convert(varchar(255), i.Item_Description) + ',    ',
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + ' Quantity Shipped = ' + Convert(varchar(255),eInvoiceQuantity) + ' ' + eu.EDISysCode + ',  ',
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + ' Quantity Ordered = ' + Convert(varchar(255),QuantityOrdered)+ ' ' + eu.EDISysCode + '.',
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + CHAR(13) + CHAR(10),
		@BUYEREMAIL			 = ISNULL(u.EMail, '')
    FROM
		OrderItem					(nolock) oi
		INNER JOIN	ItemIdentifier	(nolock) ii	ON	ii.Item_Key				= oi.Item_Key 
												AND ii.Default_Identifier	= 1  
		INNER JOIN	OrderHeader		(nolock) oh	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID  
		INNER JOIN	Users			(nolock) u	ON	u.User_ID				= oh.CreatedBy
		INNER JOIN	Item			(nolock) i	ON	oi.Item_Key				= i.Item_Key  
		INNER JOIN	ItemVendor		(nolock) iv ON	oi.Item_Key				= iv.Item_Key 
												AND oh.Vendor_ID			= iv.Vendor_ID  
		INNER JOIN	ItemUnit		(nolock) pu ON	oi.Package_Unit_ID		= pu.Unit_ID  
		INNER JOIN	ItemUnit		(nolock) ou ON	oi.QuantityUnit			= ou.Unit_ID
		INNER JOIN	ItemUnit		(nolock) eu	ON	oi.QuantityUnit			= eu.Unit_ID
	WHERE 
		oi.OrderHeader_ID		= @OrderHeader_ID
		AND eInvoiceQuantity	< QuantityOrdered
		AND eInvoiceQuantity	> 0

	-- Output generation
	IF @@RowCount > 0
		BEGIN
			SET @IssuesShortShip	= 1
			SET @ERRORCOUNT			= @ERRORCOUNT + @@RowCount
		END
   ELSE
		SET @IssuesShortShip = 0

 	--**************************************************************************
	-- Out of Stock check (OOS)
	--************************************************************************** 
	SELECT    
		@ISSUEOUTOFSTOCKText = isnull(@ISSUEOUTOFSTOCKText,'') + 'Item = ' + ii.Identifier + ',   ',
		@ISSUEOUTOFSTOCKText = isnull(@ISSUEOUTOFSTOCKText,'') + ' Description = ' + Convert(varchar(255),i.Item_Description) + ',    ',
		@ISSUEOUTOFSTOCKText = isnull(@ISSUEOUTOFSTOCKText,'') + ' Quantity Shipped = ' + Convert(varchar(255), ISNULL(eInvoiceQuantity, 0)) + ',  ',
		@ISSUEOUTOFSTOCKText = isnull(@ISSUEOUTOFSTOCKText,'') + ' Quantity Ordered = ' + Convert(varchar(255),(
								CASE 
									WHEN i.CostedByWeight = 1 THEN 
										DBO.fn_CostConversion(QuantityOrdered,@PoundID,QuantityUnit,oi.Package_Desc1,oi.Package_Desc2,oi.Package_Unit_ID)
									ELSE
										DBO.fn_CostConversion(QuantityOrdered,@UnitID,QuantityUnit,oi.Package_Desc1,oi.Package_Desc2,oi.Package_Unit_ID)
								END)) + '.',
		@IssuesShortShipText = isnull(@IssuesShortShipText,'') + CHAR(13) + CHAR(10),
		@BUYEREMAIL			 = ISNULL(u.EMail, '')
    FROM 
		OrderItem					(nolock) oi
		INNER JOIN	ItemIdentifier	(nolock) ii	ON	ii.Item_Key				= oi.Item_Key 
												AND ii.Default_Identifier	= 1  
		INNER JOIN	OrderHeader		(nolock) oh	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID  
		INNER JOIN	Users			(nolock) u	ON	u.User_ID				= oh.CreatedBy
		INNER JOIN	Item			(nolock) i	ON	oi.Item_Key				= i.Item_Key  
		INNER JOIN	ItemVendor		(nolock) iv ON	oi.Item_Key				= iv.Item_Key 
												AND oh.Vendor_ID			= iv.Vendor_ID  
		INNER JOIN	ItemUnit		(nolock) pu ON	oi.Package_Unit_ID		= pu.Unit_ID  
		INNER JOIN	ItemUnit		(nolock) ou ON	oi.QuantityUnit			= ou.Unit_ID
		INNER JOIN	ItemUnit		(nolock) eu	ON	oi.QuantityUnit			= eu.Unit_ID
    WHERE
		oi.OrderHeader_ID				= @OrderHeader_ID
		AND ISNULL(eInvoiceQuantity, 0)	= 0

	-- Output generation
	IF @@RowCount > 0
		BEGIN
			SET @IssuesOutOfStock	= 1
			SET @ERRORCOUNT			= @ERRORCOUNT + @@RowCount
		END
	ELSE
		SET @IssuesOutOfStock		= 0
		
 	--**************************************************************************
	-- Generate output
	--************************************************************************** 
	-- Header Output
	IF @ERRORCOUNT >= 1
		SET @ERRORMESSAGE = 'The following issue(s) were found for order ' + @OrderHeader_Id + '. ' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)

	-- ED Output
	IF @IssuesExpectedDateDifferent = 1
		SET @ERRORMESSAGE = @ERRORMESSAGE + 'The expected delivery date is different. ' + CHAR(13) + CHAR(10) + @IssuesExpectedDateText + CHAR(13) + CHAR(10)

	-- SS Output
	IF @IssuesShortShip = 1
		SET @ERRORMESSAGE = @ERRORMESSAGE + 'Item(s) which will be short-shipped: '  + CHAR(13) + CHAR(10) + @IssuesShortShipText + CHAR(13) + CHAR(10)

	-- OSS Output
	IF @IssuesOutOfStock = 1
		SET @ERRORMESSAGE = @ERRORMESSAGE + 'Item(s) which are out of stock: ' + CHAR(13) + CHAR(10) + @ISSUEOUTOFSTOCKText  + CHAR(13) + CHAR(10)

	--**************************************************************************
	-- Send email
	--**************************************************************************       
	IF (@IssuesExpectedDateDifferent = 1 OR @IssuesShortShip = 1 OR @IssuesOutOfStock = 1) AND @BUYEREMAIL <> ''
		BEGIN
			DECLARE @SUBJECT VARCHAR(100)
			SET @SUBJECT = 'Discrepancy for Electronic Order: ' + @OrderHeader_Id
			EXEC msdb.dbo.sp_send_dbmail  
			@profile_name	= 'IRMA Support',   
			@recipients		= @BUYEREMAIL,
			@body			= @ERRORMESSAGE,      
			@subject		= @SUBJECT
		END

 END Try

 --**************************************************************************
 -- Catch error
 --************************************************************************** 
 BEGIN CATCH      
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        RAISERROR ('mysproc failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
 END CATCH   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AlertBuyerAbout_ED_SS_OOS] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AlertBuyerAbout_ED_SS_OOS] TO [IRMAClientRole]
    AS [dbo];

