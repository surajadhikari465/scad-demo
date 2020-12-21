CREATE PROCEDURE [dbo].[EInvoicing_IsDuplicateEInvoice]
	@Invoice_Num	VARCHAR(30),
	@vendorid		VARCHAR(30),
	@PONum			VARCHAR(30),
	@ForceLoad		BIT 
AS
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	Changed all reference to DVOOrderID to OrderExternalSourceOrderID
TTL		031611	1605	Moved set of @PoNum_Clean variable to the top of the procedure.  It was only being set in the top IF-EXISTS block
						so when the ELSE (second) block was executed, the variable had a NULL value, resulting in the second piece never
						identifying the case where a different invoice# for same PO and vendor was being imported.
BBB		020112	4608	Added in additional check for OrderHeader in @MatchingOrderHeaderId where ExternalOrder IS NOT NULL, but PO # Match;
BBB		010413	9715	Changed datatype mapping when comparing OrderHeader and PONum from int/varchar to varchar/varchar
MZ      030113  9264    Changed to incude closed but without invoice POs for reparse
MZ      032513  9880    Removed the code to re-open POs for force load. If the POs are open, they won't be automatically re-closed in SP EInvoicing_MatchEInvoiceToPurchaseOrder.
MZ		050313  11992	Modified the code so that e-invoice record would be flagged with error right away if the associated PO has been uploaded.
BAS		052813	12174	Added an IF EXISTS in main ELSE block to check OrderHeader table to see if the @PONum_Clean matches an order for that vendor
						based on either OrderHeader_ID or OrderExternalSourceOrderID and that order already has an einvoiceID. 
						If an order is found then it will result in error code 99.
***********************************************************************************************/

BEGIN
-- set snapshot isolation level. to make sure (nolock) functionality is applied to all queries in SP and deadlocks are avoided.
SET TRANSACTION ISOLATION LEVEL SNAPSHOT


	DECLARE @count                  INT      
	DECLARE @matchingEinvoiceId     INT      
	DECLARE @matchingPONum          VARCHAR(255)      
	DECLARE @IsmatchingPOClosed     BIT      
	DECLARE @IsmatchingPOUploaded   BIT  
	DECLARE @matchingVendorId       VARCHAR(255)      
	DECLARE @MatchingOrderHeaderId  INT      
	DECLARE @ReturnValue            VARCHAR(100) 
	DECLARE @MatchingPOHasInvoice   BIT

	DECLARE @PONum_Clean varchar(30)  
	DECLARE @MatchingPONum_Clean varchar(255) 
	
	SET @PoNum_Clean = dbo.ToNumericString(@PONum);
	
	/*      
	ReturnValue:      
	ERROR -- Invoice has been suspended and flagged with an error code.      
	INSERT|OK -- New Invoice go ahead and import.      
	INSERT|<ErrorCode> -- Insert but flag as error.      
	UPDATE|<EInvoiceId>|OVERWRITEALL  -- Duplicate Invoice, but PO not closed. Update EInv and IMRA Inv Information.      
	UPDATE|<EInvoiceId>|OVERWRITEEINV -- Duplicate Invoice, but PO is already closed. Update EInv Data only. Leave IRMA invoice data alone.      
	*/ 
	
	-- does Invoice_Num already exist in the EInvoicing tables?      
	IF EXISTS (
		   SELECT EInvoice_Id
		   FROM   einvoicing_invoices
		   WHERE  Invoice_Num = @Invoice_Num
	   )
	BEGIN
		-- does it match more than one?       
		SET @count = (
				SELECT COUNT(einvoice_Id)
				FROM   EInvoicing_Invoices ei
				WHERE  ei.Invoice_Num = @Invoice_Num
			)      
		
		IF @count >= 1
		BEGIN
			-- only one. get the EId.       
			SET @MatchingEInvoiceId = (
					SELECT TOP 1 EInvoice_Id
					FROM   einvoicing_invoices
					WHERE  Invoice_Num = @Invoice_Num
					ORDER BY
						   ImportDate DESC
				) 
			
			-- get the PONum       
			SET @MatchingPONum = (
					SELECT TOP 1 po_num
					FROM   einvoicing_invoices
					WHERE  EInvoice_Id = @MatchingEInvoiceId
					ORDER BY
						   ImportDate DESC
				) 
			
			SET @MatchingPONum_clean  = dbo.ToNumericString(@MatchingPONum);

			-- get the Vendor       
			SET @MatchingVendorid = (
					SELECT TOP 1 PSVendor_Id
					FROM   einvoicing_invoices
					WHERE  EInvoice_Id = @MatchingEInvoiceId
					ORDER BY
						   ImportDate DESC
				)      
			
			SET @MatchingOrderHeaderId = (
					SELECT TOP 1 oh.OrderHeader_ID
					FROM   OrderHeader oh
					--WHERE  ISNULL(DVOOrderid, Orderheader_id) = @MatchingPONum
					WHERE  CONVERT(varchar(255), ISNULL(OrderExternalSourceOrderID, Orderheader_id)) = @MatchingPONum_clean
					OR (OrderExternalSourceOrderID IS NOT NULL AND CONVERT(varchar(255), oh.OrderHeader_ID) = @MatchingPONum_clean)
					ORDER BY
						   orderdate DESC
				)      
				
			SELECT @IsmatchingPOClosed  = (
							CASE 
								WHEN oh.CloseDate IS NULL THEN 0
								ELSE 1
							END),
				   @IsmatchingPOUploaded = (
							CASE 
								WHEN oh.UploadedDate IS NULL THEN 0
								ELSE 1
							END),							
				   @MatchingPOHasInvoice = (
							CASE 
								WHEN oi.OrderHeader_Id IS NULL THEN 0
								ELSE 1
							END)
			  FROM  OrderHeader  oh
		LEFT  JOIN  OrderInvoice oi  ON  oh.OrderHeader_Id = oi.OrderHeader_Id
			 WHERE  oh.OrderHeader_ID = @MatchingOrderHeaderId		

		-- if PONum and VendorId both match.      OR ponum is null but vendor matches based on invoice number (dsd vendors)
			IF ((@PoNum_Clean = @MatchingPONum_clean AND @vendorid = @matchingVendorId)) OR (@ponum = '' AND @vendorid = @matchingVendorId) 
			BEGIN
				IF @IsmatchingPOUploaded = 1 AND @ForceLoad = 1
					BEGIN
						set @ReturnValue = 'UPDATE|' + CAST(@MatchingEInvoiceId AS VARCHAR(100)) + '|107'
					END
				ELSE
				IF @IsmatchingPOUploaded = 1 AND @ForceLoad = 0
					BEGIN
						set @ReturnValue = 'INSERT|107'
					END
				ELSE			
				IF @IsmatchingPOClosed = 0 OR @PONum = ''
				BEGIN
					PRINT 
					'matching IRMA PO is NOT closed. overwrite invoice (close order screen) information. overwrite einv information'      
					SET @ReturnValue = 'UPDATE|' + CAST(@MatchingEInvoiceId AS VARCHAR(100)) 
						+ '|OVERWRITEALL'
				END
				ELSE
				BEGIN
					if @ForceLoad = 1 
					BEGIN
						-- if we are forceloading reopen the order and let it go through. 
						print 'ForceLoad Order: ' + cast(@MatchingOrderHeaderId as varchar(20))

							-- clear existing orderinvoice data.
							exec DeleteOrderInvoice @MatchingOrderHeaderId
							SET @ReturnValue = 'UPDATE|' + CAST(@MatchingEInvoiceId AS VARCHAR(100)) + '|OVERWRITEALL'
					END
					ELSE
					BEGIN
						-- closed but not has invoice
					    IF @MatchingPOHasInvoice = 0
					    BEGIN
							SET @ReturnValue = 'UPDATE|' + CAST(@MatchingEInvoiceId AS VARCHAR(100)) + '|OVERWRITEALL'
					    END
					    ELSE
						-- not a force load and PO has invoiced. mark w/ error.
						SET @ReturnValue = 'insert|102'
					END
					
				END
			END
			ELSE 
			-- if vendorid matches but PONum does not.      
			IF @vendorid = @matchingVendorId
			   AND @PoNum_Clean <> @MatchingPONum_clean
			BEGIN
				-- exception. same invoice number  same vendor different PO.      
				PRINT 
				'EXCEPTION -- An Einvoice has already been imported with this Invoice NUmber. Vendor Matches but PO Number does not.'      
				SET @ReturnValue = 'insert|97'
			END
			ELSE 
			-- vendor and PONum do not match. exception.      
			IF @vendorid <> @matchingVendorId
			   AND @PoNum_Clean <> @MatchingPONum_clean
			BEGIN
				PRINT 
				'EXCEPTION -- An EInvoice has already been imported with this Invoice Number. PO Number and Vendor do not match.'      
				SET @ReturnValue = 'insert|ok'
			END
			ELSE
			IF @vendorid <> @matchingVendorId
			AND @PoNum_Clean = @MatchingPONum_clean
			BEGIN
				-- An Invoice has already been importeed with this Invoice Number. The Vendors do not match but the PO is the same.
			SET @ReturnValue = 'insert|103'	
				
			END 
			ELSE
				IF @vendorid <> @matchingVendorId
				AND @PoNum_Clean <> @MatchingPONum_clean
				BEGIN
					SET @ReturnValue = 'insert|ok'
				END	
		END
	END
	ELSE
	BEGIN
		-- no invoice_num match exists.
		-- does an einvoice exist from the same vendor with the same PO?      
		IF (EXISTS (
				SELECT ei.EInvoice_Id
				FROM   EInvoicing_Invoices ei
				WHERE  po_num = @PoNum_Clean
						AND ei.PSVendor_Id = @vendorid
						AND @ponum <> '' --exclude invoices with no PO number. 
			) OR

			-- was an Einvoice already loaded to the PO and this Einvoice is using the ExternalSourceOrderID?
			-- this covers the scenario where the vendor sends a second einvoice with a different invoice number and using ExternalSourceOrderID as the po_num
			EXISTS (
				SELECT oh.OrderHeader_ID
				FROM 
					OrderHeader oh (nolock)
					INNER JOIN Vendor v (nolock) ON oh.Vendor_ID = v.Vendor_ID
				WHERE 
					(oh.OrderHeader_ID = @PONum_Clean OR oh.OrderExternalSourceOrderID = @PONum_Clean)
					AND oh.eInvoice_Id IS NOT NULL
					AND v.PS_Export_Vendor_ID = @vendorid
			)
		)
		BEGIN
			--yes      
			PRINT 
			'exception: an invoice from the same vendor for the same PO already exists'      
			SET @ReturnValue = 'insert|99'
		END
		ELSE
		BEGIN
			-- no invoice_num match exists.
			-- check for duplicate po num but different vendor.    
			IF EXISTS (
				   SELECT ei.einvoice_id
				   FROM   EInvoicing_Invoices ei
				   WHERE  po_num = @PoNum_Clean
				   AND @ponum <> '' --exclude invoices with no PO number. 
			   )
			BEGIN
				-- an invoice with this PO number has already been inserted.    
				SET @ReturnValue = 'insert|97'
			END
			ELSE
			BEGIN
				SET @ReturnValue = 'insert|ok'
			END
		END
	END      
	SELECT @ReturnValue AS ReturnValue
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_IsDuplicateEInvoice] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_IsDuplicateEInvoice] TO [IRMAClientRole]
    AS [dbo];

