CREATE TABLE [dbo].[tlog_stores] (
    [store]    CHAR (5) NULL,
    [store_no] INT      NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_stores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_stores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_stores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_stores] TO [IRMAReportsRole]
    AS [dbo];

