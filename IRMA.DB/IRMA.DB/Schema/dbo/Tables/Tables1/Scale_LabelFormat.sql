CREATE TABLE [dbo].[Scale_LabelFormat] (
    [Scale_LabelFormat_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]          VARCHAR (50) NULL,
    CONSTRAINT [PK_Scale_LabelFormat] PRIMARY KEY CLUSTERED ([Scale_LabelFormat_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelFormat] TO [IRMAReportsRole]
    AS [dbo];

