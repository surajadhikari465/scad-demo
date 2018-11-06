CREATE PROCEDURE [dbo].[Replenishment_POSAudit_GetNoOfExceptions]
	@Store_No INT
AS
BEGIN
    SELECT COUNT(*) 
    FROM   POSAuditException
    WHERE Store_No IN (SELECT DISTINCT
                            CASE
                                WHEN @Store_No = -1 THEN
                                    Store_No
                                ELSE
                                    @Store_No
                            END
                        FROM Store) 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSAudit_GetNoOfExceptions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSAudit_GetNoOfExceptions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSAudit_GetNoOfExceptions] TO [IRMASchedJobsRole]
    AS [dbo];

