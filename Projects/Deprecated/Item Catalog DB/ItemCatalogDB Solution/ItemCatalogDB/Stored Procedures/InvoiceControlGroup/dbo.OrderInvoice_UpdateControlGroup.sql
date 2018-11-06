IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[OrderInvoice_UpdateControlGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[OrderInvoice_UpdateControlGroup]
GO

-- Update an existing record in the [OrderInvoice_ControlGroup] table.
-- The control group can only be updated when it is in the OPEN status.
CREATE PROCEDURE dbo.OrderInvoice_UpdateControlGroup
	@OrderInvoice_ControlGroup_ID int,
	@ExpectedGrossAmt money,
	@ExpectedInvoiceCount int,
	@UpdateUser_ID int
AS
BEGIN
	UPDATE [dbo].[OrderInvoice_ControlGroup] SET 
		ExpectedGrossAmt = @ExpectedGrossAmt,
		ExpectedInvoiceCount = @ExpectedInvoiceCount,
		UpdateTime = GetDate(),
		UpdateUser_ID = @UpdateUser_ID
	 WHERE 
		OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
		AND OrderInvoice_ControlGroupStatus_ID = 1 -- OPEN Status
END
GO
 