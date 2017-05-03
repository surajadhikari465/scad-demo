
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- **************************************************************************
-- Procedure: CheckForDuplicateReceivingDocumentInvoiceNumber
--    Author: Kyle Milner
--      Date: 2014-12-18
--
-- Description:
-- This procedure is called when creating a receiving document in the handheld.
-- It checks for duplicate invoice numbers for a given DSD vendor and returns any
-- duplicates that were found.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2014-12-18	KM				Creation;
--
-- **************************************************************************

CREATE PROCEDURE [dbo].[CheckForDuplicateReceivingDocumentInvoiceNumber]
	@InvoiceNumber	varchar(20),
	@VendorId		int
as
begin
	
	set nocount on;

	select
		InvoiceNumber
	from
		OrderHeader (nolock) oh
	where
		DSDOrder = 1 and
		Vendor_ID = @VendorId and
		InvoiceNumber = @InvoiceNumber
    
end
