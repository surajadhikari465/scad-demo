CREATE TABLE [dbo].[POSWriterFileConfig] (
    [POSFileWriterKey] INT           NOT NULL,
    [POSChangeTypeKey] INT           NOT NULL,
    [ColumnOrder]      INT           NOT NULL,
    [RowOrder]         INT           CONSTRAINT [DF_POSWriterFileConfig_RowOrder] DEFAULT ((1)) NOT NULL,
    [DataElement]      VARCHAR (100) NOT NULL,
    [FieldID]          VARCHAR (20)  NULL,
    [MaxFieldWidth]    INT           NULL,
    [TruncLeft]        BIT           CONSTRAINT [DF_POSWriterFileConfig_TruncLeft] DEFAULT ((0)) NULL,
    [DefaultValue]     VARCHAR (10)  NULL,
    [IsTaxFlag]        BIT           CONSTRAINT [DF_POSWriterFileConfig_IsTaxFlag] DEFAULT ((0)) NULL,
    [IsLiteral]        BIT           CONSTRAINT [DF_POSWriterFileConfig_IsLiteral] DEFAULT ((0)) NULL,
    [IsDecimalValue]   BIT           CONSTRAINT [DF_POSWriterFileConfig_IsDecimalValue] DEFAULT ((0)) NULL,
    [DecimalPrecision] INT           NULL,
    [IncludeDecimal]   BIT           CONSTRAINT [DF_POSWriterFileConfig_IncludeDecimal] DEFAULT ((0)) NULL,
    [PadLeft]          BIT           CONSTRAINT [DF_POSWriterFileConfig_PadLeft] DEFAULT ((0)) NULL,
    [FillChar]         VARCHAR (1)   NULL,
    [LeadingChars]     VARCHAR (10)  NULL,
    [TrailingChars]    VARCHAR (10)  NULL,
    [IsBoolean]        BIT           DEFAULT ((0)) NULL,
    [BooleanTrueChar]  VARCHAR (10)  NULL,
    [BooleanFalseChar] VARCHAR (10)  NULL,
    [FixedWidthField]  BIT           DEFAULT ((0)) NULL,
    [BitOrder]         TINYINT       CONSTRAINT [DF_POSWriterFileConfig_BitOrder] DEFAULT ((0)) NOT NULL,
    [IsPackedDecimal]  BIT           NULL,
    [IsBinaryInt]      BIT           NULL,
    [PackLength]       INT           NULL,
    CONSTRAINT [PK_POSWriterFileConfig] PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [POSChangeTypeKey] ASC, [ColumnOrder] ASC, [RowOrder] ASC, [BitOrder] ASC),
    CONSTRAINT [FK_POSWriterFileConfig_POSChangeTypeKey] FOREIGN KEY ([POSChangeTypeKey]) REFERENCES [dbo].[POSChangeType] ([POSChangeTypeKey]),
    CONSTRAINT [FK_POSWriterFileConfig_POSFileWriterKey] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterFileConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterFileConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterFileConfig] TO [IRMAReportsRole]
    AS [dbo];

