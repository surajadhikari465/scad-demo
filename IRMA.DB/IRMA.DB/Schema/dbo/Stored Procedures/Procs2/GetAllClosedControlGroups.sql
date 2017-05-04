CREATE PROCEDURE [dbo].[GetAllClosedControlGroups]
AS
BEGIN
	select distinct OrderInvoice_ControlGroup_ID from Orderinvoice_ControlGroup
    where OrderInVoice_ControlGroupStatus_ID=2 -- All closed controlGroups
    order by OrderInvoice_ControlGroup_ID asc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllClosedControlGroups] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllClosedControlGroups] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllClosedControlGroups] TO [IRMAReportsRole]
    AS [dbo];

