CREATE TABLE [dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [uniqueid]       VARCHAR (255) NOT NULL,
    [filename]       VARCHAR (255) NOT NULL,
    [filetype]       VARCHAR (4)   NOT NULL,
    [orderheader_id] INT           NOT NULL,
    [identifier]     VARCHAR (13)  NOT NULL,
    [value]          INT           NULL,
    [ts]             DATETIME      DEFAULT (getdate()) NOT NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedOrdersValidationWorkspace] TO [IRMAReportsRole]
    AS [dbo];

