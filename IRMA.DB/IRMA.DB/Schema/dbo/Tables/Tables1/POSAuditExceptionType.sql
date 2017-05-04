CREATE TABLE [dbo].[POSAuditExceptionType] (
    [POSAuditExceptionTypeID]   INT          NOT NULL,
    [POSAuditExceptionTypeDesc] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_POSAuditExceptionType] PRIMARY KEY CLUSTERED ([POSAuditExceptionTypeID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditExceptionType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditExceptionType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditExceptionType] TO [IRMAReportsRole]
    AS [dbo];

