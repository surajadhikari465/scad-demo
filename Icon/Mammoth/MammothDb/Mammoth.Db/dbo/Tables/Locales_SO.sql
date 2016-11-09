CREATE TABLE [dbo].[Locales_SO] (
    [Region]         NCHAR (2)      DEFAULT ('SO') NOT NULL,
    [LocaleID]       INT            IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID] INT            NOT NULL,
    [StoreName]      NVARCHAR (255) NOT NULL,
    [StoreAbbrev]    NVARCHAR (5)   NOT NULL,
    [AddedDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]   DATETIME       NULL,
    CONSTRAINT [PK_Locales_SO] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_SO]
);
