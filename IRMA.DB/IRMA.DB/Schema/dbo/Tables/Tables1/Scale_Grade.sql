CREATE TABLE [dbo].[Scale_Grade] (
    [Scale_Grade_ID] INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]    VARCHAR (50) NULL,
    [Zone1]          SMALLINT     NULL,
    [Zone2]          SMALLINT     NULL,
    [Zone3]          SMALLINT     NULL,
    [Zone4]          SMALLINT     NULL,
    [Zone5]          SMALLINT     NULL,
    [Zone6]          SMALLINT     NULL,
    [Zone7]          SMALLINT     NULL,
    [Zone8]          SMALLINT     NULL,
    [Zone9]          SMALLINT     NULL,
    [Zone10]         SMALLINT     NULL,
    [User_ID]        INT          NULL,
    CONSTRAINT [PK_Scale_Grade] PRIMARY KEY CLUSTERED ([Scale_Grade_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Grade] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Grade] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Grade] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Grade] TO [IRMAReportsRole]
    AS [dbo];

