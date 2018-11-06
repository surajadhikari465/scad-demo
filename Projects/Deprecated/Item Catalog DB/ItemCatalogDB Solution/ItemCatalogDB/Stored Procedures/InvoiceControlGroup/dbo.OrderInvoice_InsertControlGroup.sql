IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[OrderInvoice_InsertControlGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[OrderInvoice_InsertControlGroup]
GO

-- Insert a new record into the [OrderInvoice_ControlGroup] table.
-- The control group is always created in the OPEN status.
CREATE PROCEDURE dbo.OrderInvoice_InsertControlGroup
	@ExpectedGrossAmt money,
	@ExpectedInvoiceCount int,
	@UpdateUser_ID int,
	@OrderInvoice_ControlGroup_ID int OUTPUT
AS
BEGIN
	INSERT INTO [dbo].[OrderInvoice_ControlGroup] (
		ExpectedGrossAmt,
		ExpectedInvoiceCount,
		OrderInvoice_ControlGroupStatus_ID,
		UpdateTime,
		UpdateUser_ID
	) VALUES (
		@ExpectedGrossAmt,
		@ExpectedInvoiceCount,
		1,	-- OPEN Status
		GetDate(),
		@UpdateUser_ID
	)
	
	-- Grab the key for the new record so that it can be returned.
	SELECT @OrderInvoice_ControlGroup_ID = SCOPE_IDENTITY()
END
GO
 