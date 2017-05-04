CREATE PROCEDURE [dbo].[GetAllControlGroups]
AS
BEGIN
   	select distinct OrderInvoice_ControlGroup_ID from Orderinvoice_ControlGroup
    order by OrderInvoice_ControlGroup_ID asc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroups] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroups] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllControlGroups] TO [IRMAReportsRole]
    AS [dbo];

