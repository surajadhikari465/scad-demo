CREATE TABLE [icon].[LocaleTrait] (
    [traitID]    INT            NOT NULL,
    [localeID]   INT            NOT NULL,
    [uomID]      INT            NULL,
    [traitValue] NVARCHAR (255) NULL,
    CONSTRAINT [LocaleTrait_PK] PRIMARY KEY CLUSTERED ([traitID] ASC, [localeID] ASC) WITH (FILLFACTOR = 100)
);

