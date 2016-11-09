CREATE TABLE [dbo].[POSAuditException] (
    [POSAuditExceptionKey]    INT          IDENTITY (1, 1) NOT NULL,
    [SessionID]               INT          NOT NULL,
    [Item_Key]                INT          NOT NULL,
    [Store_No]                INT          NOT NULL,
    [POSAuditExceptionTypeID] INT          NOT NULL,
    [Identifier]              VARCHAR (13) NOT NULL,
    CONSTRAINT [PK_POSAuditException] PRIMARY KEY CLUSTERED ([POSAuditExceptionKey] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSAuditException] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSAuditException] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditException] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSAuditException] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSAuditException] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSAuditException] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditException] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSAuditException] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSAuditException] TO [IRMAReportsRole]
    AS [dbo];

