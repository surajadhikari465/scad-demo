CREATE PROCEDURE dbo.Administration_POSPush_GetPOSWriterFileConfigRowCount
    @FileWriterKey int,
    @ChangeTypeKey int 
AS
-- Returns  the number of rows configured in the POSWriterFileConfig details for a 
-- writer, by change type.

BEGIN
select max(roworder) as MaxRow
from 
POSWriterFileConfig   
where 
POSFileWriterKey = @FileWriterKey and
POSChangeTypeKey = @ChangeTypeKey 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigRowCount] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigRowCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigRowCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigRowCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterFileConfigRowCount] TO [IRMAReportsRole]
    AS [dbo];

