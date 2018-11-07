/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.1.8
 * Time: 12/30/2008 10:35:06 AM
 ************************************************************/

/****** Object:  StoredProcedure [dbo].[EInvoicing_GetOrderHeaderIDForDSDOrder]    Script Date: 08/14/2008 10:53:58 ******/
IF EXISTS (
       SELECT *
       FROM   sys.objects
       WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[EInvoicing_GetOrderHeaderIDForDSDOrder]')
              AND TYPE IN (N'P', N'PC')
   )
    DROP PROCEDURE [dbo].[EInvoicing_GetOrderHeaderIDForDSDOrder]
GO
/****** Object:  StoredProcedure [dbo].[EInvoicing_GetOrderHeaderIDForDSDOrder]    Script Date: 08/14/2008 10:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.EInvoicing_GetOrderHeaderIDForDSDOrder
	@Invoice_Num	VARCHAR(255),
	@PO_Num			VARCHAR(255) = '',
	@Vendor_ID		VARCHAR(255)
AS
-- **************************************************************************
-- Procedure: EInvoicing_GetOrderHeaderIDForDSDOrder
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from the eInvoicing DAO to handle reparsing of DSD / GSS Orders.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 05/01/2013	DN   	11993	SP created to assign correct PO number to the suspended EInvoice po_num field
-- **************************************************************************
BEGIN
	DECLARE @OrderHeader_ID			AS INT
	DECLARE @OrderHeader_ID_Found	AS BIT = 0
	DECLARE @NumberOfMatches		AS INT = 0
	DECLARE @Result TABLE
	(
	OrderHeader_ID		INT,
	InvoiceMatch		BIT,
	POMatch				BIT,
	ErrorFound			BIT,
	Message		VARCHAR(255)
	)

	-- Verify any duplicate invoice numbers used by the vendor

	SELECT @NumberOfMatches = COUNT(ei.Invoice_Num)
	FROM EInvoicing_Invoices ei INNER JOIN Vendor v
	ON ei.PSVendor_Id = v.PS_Vendor_ID 
	WHERE	ei.Invoice_Num = @Invoice_Num AND 
			ei.PSVendor_Id = @Vendor_ID
			
	
	IF @NumberOfMatches > 1
		BEGIN
			SET @OrderHeader_ID = 0
			SET @OrderHeader_ID_Found = 1
			INSERT INTO @Result VALUES (@OrderHeader_ID, 0, 0, 1,'Duplicate invoice number found.')
		END	

	-- Check to see if there is a match on Invoice # between OrderHeader and EInvoicing_Invoices tables.
	IF @OrderHeader_ID_Found = 0
		BEGIN
			IF NOT EXISTS (SELECT oh.OrderHeader_ID FROM OrderHeader oh INNER JOIN EInvoicing_Invoices ei ON oh.InvoiceNumber = ei.Invoice_Num WHERE ei.Invoice_Num = @Invoice_Num AND oh.DSDOrder = 1)
				SET @OrderHeader_ID = 0
			ELSE
				BEGIN
					INSERT INTO @Result
					SELECT	oh.OrderHeader_ID,
							1,
							0,
							0,
							'' AS Message
					FROM OrderHeader oh INNER JOIN EInvoicing_Invoices ei 
					ON oh.InvoiceNumber = ei.Invoice_Num 
					WHERE	ei.Invoice_Num = @Invoice_Num AND 
							ei.PSVendor_Id = @Vendor_ID AND
							oh.DSDOrder = 1

					SET @OrderHeader_ID_Found = 1
				END
		END

		-- Verify any duplicate PO numbers used by the vendor
		if @OrderHeader_ID_Found = 0
			BEGIN
				SELECT @NumberOfMatches = COUNT(ei.po_num)
				FROM EInvoicing_Invoices ei INNER JOIN Vendor v
				ON ei.PSVendor_Id = v.PS_Vendor_ID 
				WHERE	ei.po_num = @PO_Num AND 
						ei.PSVendor_Id = @Vendor_ID
			
	
			IF @NumberOfMatches > 1
				BEGIN
					SET @OrderHeader_ID = 0
					SET @OrderHeader_ID_Found = 1
					INSERT INTO @Result VALUES (@OrderHeader_ID, 0, 0, 1, 'Duplicate PO number found.')
				END	
			END

	-- If there is no match on Invoice # (@OrderHeader_ID = 0), then check to see if there is a match using PO number.

	IF @OrderHeader_ID_Found = 0
		BEGIN
			IF NOT EXISTS (SELECT oh.OrderHeader_ID FROM OrderHeader oh INNER JOIN EInvoicing_Invoices ei ON RTRIM(CONVERT(VARCHAR(255),oh.OrderHeader_ID)) = RTRIM(ei.po_num) WHERE RTRIM(CONVERT(VARCHAR(255),oh.OrderHeader_ID)) = RTRIM(@PO_Num) AND oh.DSDOrder = 1)
				SET @OrderHeader_ID = 0
			ELSE
				BEGIN
					INSERT INTO @Result
					SELECT	oh.OrderHeader_ID,
							0,
							1,
							0,
							'' AS Message
					FROM OrderHeader oh INNER JOIN EInvoicing_Invoices ei 
					ON RTRIM(CONVERT(VARCHAR(255),oh.OrderHeader_ID)) = RTRIM(ei.po_num) 
					WHERE	oh.OrderHeader_ID = CONVERT(INT, @PO_Num) AND 
							ei.PSVendor_Id = @Vendor_ID AND
							oh.DSDOrder = 1

					SET @OrderHeader_ID_Found = 1
				END

		END

	-- If nothing matches, return OrderHeader_ID = 0 and a message "No matching PO found"
	IF @OrderHeader_ID_Found = 0
		INSERT INTO @Result VALUES (@OrderHeader_ID, 0, 0, 0, 'No matching PO found')

	SELECT * FROM @Result
END
GO