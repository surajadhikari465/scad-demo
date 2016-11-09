CREATE TABLE [etl].[Traits] (
    [tID]          INT            IDENTITY (1, 1) NOT NULL,
    [TraitDesc]    NVARCHAR (255) NULL,
    [TraitGroupID] INT            NULL,
    CONSTRAINT [PK_Traits] PRIMARY KEY CLUSTERED ([tID] ASC) WITH (FILLFACTOR = 100)
);

