SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderBeforeClose]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderBeforeClose]
GO

CREATE PROCEDURE dbo.UpdateOrderBeforeClose
	@OrderHeader_ID		int,
	@InvoiceNumber		varchar(20),
	@InvoiceDate		date,
	@InvoiceCost		smallmoney,
	@VendorDoc_ID		varchar(16),
	@VendorDocDate		date,
	@SubTeam_No			int,
	@PartialShipment	bit = 0

AS

-- **************************************************************************************************
-- Procedure: UpdateOrderBeforeClose
--    Author: Benjamin Sims
--      Date: 11/5/2012
--
-- Description: This stored procedure is called by the Service Library
--				which is used by the WFM Mobile IRMA Plugin on the InvoiceData.vb form.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 11/5/2012	BS		2752	Created for handheld receiving via Service Library.
--								Replaces client calls of UpdateOrderStatus, DeleteOrderInvoice
--								& InsertOrderInvoice which are called in the client from OrderStatus.vb.
--								This wraps the Transaction inside the T-SQL instead of the VB.NET code.
--								This updates the fields needed for AP prior to closing the order via
--								dbo.UpdateOrderClosed. 
-- 2013-05-27	KM		12423	Check InvoiceNumber rather than InvoiceCost when determining whether to
--								update OrderInvoice;
-- **************************************************************************************************

BEGIN
	SET NOCOUNT ON
				
	BEGIN TRY
		BEGIN TRANSACTION
		
			-- Update OrderHeader
			UPDATE
				OrderHeader
			SET
				InvoiceNumber		= UPPER(@InvoiceNumber), 
				InvoiceDate			= @InvoiceDate,
				VendorDoc_ID		= @VendorDoc_ID,
				VendorDocDate		= @VendorDocDate,
				PartialShipment		= @PartialShipment
			WHERE
				OrderHeader_ID = @OrderHeader_ID
				AND (CloseDate IS NULL
					OR 
					(CloseDate IS NOT NULL AND ApprovedDate IS NULL))
			
			-- Delete OrderInvoice entry
			DELETE FROM	OrderInvoice WHERE OrderHeader_ID = @OrderHeader_ID

			-- Only add OrderInvoice entry if there is Invoice information
			IF @InvoiceNumber IS NOT NULL
				BEGIN
			
					-- Insert OrderInvoice
					INSERT INTO OrderInvoice 
					(
						OrderHeader_ID, 
						SubTeam_No, 
						InvoiceCost
					)
					VALUES 
					(
						@OrderHeader_ID, 
						@SubTeam_No, 
						@InvoiceCost
					)

				END

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		
		DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
		RAISERROR (' failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
	END CATCH

	SET NOCOUNT OFF
END
GO