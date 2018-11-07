CREATE TABLE [dbo].[JDA_ItemUnitMapping] (
    [Unit_ID] INT         NOT NULL,
    [JDA_ID]  VARCHAR (6) NULL,
    CONSTRAINT [PK_JDA_ItemUnitMapping] PRIMARY KEY CLUSTERED ([Unit_ID] ASC)
);

