CREATE TABLE [icon].[ScanCode] (
    [scanCodeID]     INT           NOT NULL,
    [itemID]         INT           NOT NULL,
    [scanCode]       NVARCHAR (13) NOT NULL,
    [scanCodeTypeID] INT           NOT NULL,
    [localeID]       INT           NULL,
    CONSTRAINT [PK_ScanCode] PRIMARY KEY CLUSTERED ([scanCodeID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [AK_ScanCode_ScanCode] UNIQUE NONCLUSTERED ([scanCode] ASC) WITH (FILLFACTOR = 100)
);

