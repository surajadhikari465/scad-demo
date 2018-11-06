IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[OrderInvoice_GetControlGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[OrderInvoice_GetControlGroup]
GO

-- This stored procedure returns the record from the OrderInvoice_ControlGroup table
-- that matches the control group id.
CREATE PROCEDURE dbo.OrderInvoice_GetControlGroup
	@OrderInvoice_ControlGroup_ID int
AS
BEGIN
	SELECT 
		CG.OrderInvoice_ControlGroup_ID,
		CG.ExpectedGrossAmt,
		CG.ExpectedInvoiceCount,
		CG.OrderInvoice_ControlGroupStatus_ID,
		CGS.OrderInvoice_ControlGroupStatus_Desc,
		CG.UpdateTime,
		CG.UpdateUser_ID,
		Users.UserName
	FROM dbo.OrderInvoice_ControlGroup CG
	INNER JOIN dbo.OrderInvoice_ControlGroupStatus CGS ON
		CG.OrderInvoice_ControlGroupStatus_ID = CGS.OrderInvoice_ControlGroupStatus_ID
	INNER JOIN dbo.Users ON
		CG.UpdateUser_ID = Users.User_ID
	WHERE
		OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
END
GO
 