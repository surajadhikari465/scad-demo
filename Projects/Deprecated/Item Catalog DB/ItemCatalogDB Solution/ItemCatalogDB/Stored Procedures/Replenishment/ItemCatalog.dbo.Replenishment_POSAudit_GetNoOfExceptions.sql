IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSAudit_GetNoOfExceptions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_POSAudit_GetNoOfExceptions]
GO

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