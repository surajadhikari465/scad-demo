CREATE PROCEDURE [dbo].[GetAllControlGroupStatus]
AS
BEGIN
Select OrderInvoice_ControlGroupStatus_ID,
    OrderInvoice_ControlGroupStatus_Desc 
    from Orderinvoice_ControlGroupStatus
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroupStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroupStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroupStatus] TO [IRMAReportsRole]
    AS [dbo];

