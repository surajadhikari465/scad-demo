CREATE TABLE [dbo].[ODBCErrorLog] (
    [SystemTime]       DATETIME       NOT NULL,
    [ODBCStart]        DATETIME       NOT NULL,
    [ODBCEnd]          DATETIME       NOT NULL,
    [ErrorNumber]      INT            NOT NULL,
    [ErrorDescription] VARCHAR (2048) NULL,
    [ODBCCall]         VARCHAR (2048) NULL,
    [UserName]         VARCHAR (255)  NULL,
    [Computer]         VARCHAR (255)  NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ODBCErrorLog] TO [IRMASLIMRole]
    AS [dbo];

