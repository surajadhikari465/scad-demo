CREATE TABLE [dbo].[Scale_EatBy] (
    [Scale_EatBy_ID] INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]    VARCHAR (50) NULL,
    CONSTRAINT [PK_EatBy] PRIMARY KEY CLUSTERED ([Scale_EatBy_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_EatBy] TO [IRMAReportsRole]
    AS [dbo];

