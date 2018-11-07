-- This stored procedure returns the records from the OrderInvoice_ControlGroup table
-- that matches the search criteria entered.  All search parameters are optional.
CREATE PROCEDURE dbo.OrderInvoice_GetControlGroupSearch
	@OrderInvoice_ControlGroup_ID int,
	@OrderInvoice_ControlGroupStatus_ID int
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
		-- control group id
		(@OrderInvoice_ControlGroup_ID IS NULL 
			OR (@OrderInvoice_ControlGroup_ID IS NOT NULL AND CG.OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID))
		-- control group status
		AND (@OrderInvoice_ControlGroupStatus_ID IS NULL 
			OR (@OrderInvoice_ControlGroupStatus_ID IS NOT NULL AND CG.OrderInvoice_ControlGroupStatus_ID = @OrderInvoice_ControlGroupStatus_ID))
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_GetControlGroupSearch] TO [IRMAClientRole]
    AS [dbo];

