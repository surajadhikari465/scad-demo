CREATE TABLE [dbo].[Scale_LabelType] (
    [Scale_LabelType_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]        VARCHAR (50) NULL,
    [LinesPerLabel]      INT          NULL,
    [Characters]         INT          NULL,
    CONSTRAINT [PK_Scale_LabelType] PRIMARY KEY CLUSTERED ([Scale_LabelType_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_LabelType] TO [IRMASLIMRole]
    AS [dbo];

