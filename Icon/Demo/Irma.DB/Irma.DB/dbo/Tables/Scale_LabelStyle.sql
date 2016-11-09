CREATE TABLE [dbo].[Scale_LabelStyle] (
    [Scale_LabelStyle_ID] INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]         VARCHAR (50) NULL,
    CONSTRAINT [PK_LabelStyle] PRIMARY KEY CLUSTERED ([Scale_LabelStyle_ID] ASC)
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelStyle] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelStyle] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelStyle] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelStyle] TO [IRMASLIMRole]
    AS [dbo];

