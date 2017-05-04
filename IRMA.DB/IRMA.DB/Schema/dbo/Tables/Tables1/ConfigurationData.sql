CREATE TABLE [dbo].[ConfigurationData] (
    [ConfigKey]   VARCHAR (50) NOT NULL,
    [ConfigValue] SQL_VARIANT  NULL,
    CONSTRAINT [PK_ConfigurationData] PRIMARY KEY CLUSTERED ([ConfigKey] ASC)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ConfigurationData] TO [IRMAReportsRole]
    AS [dbo];

