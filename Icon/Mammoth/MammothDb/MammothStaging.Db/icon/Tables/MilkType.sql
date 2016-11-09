CREATE TABLE [icon].[MilkType] (
    [MilkTypeId]  INT            NOT NULL,
    [Description] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_MilkType] PRIMARY KEY CLUSTERED ([MilkTypeId] ASC) WITH (FILLFACTOR = 100)
);

