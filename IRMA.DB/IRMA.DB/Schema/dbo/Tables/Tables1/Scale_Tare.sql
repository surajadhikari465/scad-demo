CREATE TABLE [dbo].[Scale_Tare] (
    [Scale_Tare_ID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]   VARCHAR (50)   NULL,
    [Zone1]         DECIMAL (4, 3) NULL,
    [Zone2]         DECIMAL (4, 3) NULL,
    [Zone3]         DECIMAL (4, 3) NULL,
    [Zone4]         DECIMAL (4, 3) NULL,
    [Zone5]         DECIMAL (4, 3) NULL,
    [Zone6]         DECIMAL (4, 3) NULL,
    [Zone7]         DECIMAL (4, 3) NULL,
    [Zone8]         DECIMAL (4, 3) NULL,
    [Zone9]         DECIMAL (4, 3) NULL,
    [Zone10]        DECIMAL (4, 3) NULL,
    CONSTRAINT [PK_Scale_Tare] PRIMARY KEY CLUSTERED ([Scale_Tare_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Tare] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Tare] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Tare] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Tare] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Tare] TO [IRMASLIMRole]
    AS [dbo];

