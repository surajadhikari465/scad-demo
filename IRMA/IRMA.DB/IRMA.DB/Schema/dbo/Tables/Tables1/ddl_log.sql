CREATE TABLE [dbo].[ddl_log] (
    [EventType]    VARCHAR (100)  NULL,
    [PostTime]     VARCHAR (25)   NULL,
    [SPID]         VARCHAR (10)   NULL,
    [ServerName]   VARCHAR (50)   NULL,
    [LoginName]    VARCHAR (50)   NULL,
    [UserName]     VARCHAR (50)   NULL,
    [DatabaseName] VARCHAR (50)   NULL,
    [SchemaName]   VARCHAR (50)   NULL,
    [ObjectName]   VARCHAR (50)   NULL,
    [ObjectType]   VARCHAR (25)   NULL,
    [TSQLCommand]  VARCHAR (2000) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ddl_log] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ddl_log] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ddl_log] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ddl_log] TO [IRMAReportsRole]
    AS [dbo];

GO
GRANT INSERT
		 ON OBJECT::[dbo].[ddl_log] TO [IRMAClientRole]
		 AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ddl_log] TO [IconInterface]
    AS [dbo];

GO
GRANT INSERT
		 ON OBJECT::[dbo].[ddl_log] TO [IconInterface]
		 AS [dbo];