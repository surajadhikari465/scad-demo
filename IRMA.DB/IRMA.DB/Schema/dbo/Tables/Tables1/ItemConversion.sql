CREATE TABLE [dbo].[ItemConversion] (
    [FromUnit_ID]      INT            NOT NULL,
    [ToUnit_ID]        INT            NOT NULL,
    [ConversionSymbol] CHAR (1)       NOT NULL,
    [ConversionFactor] DECIMAL (9, 4) CONSTRAINT [DF__ItemConve__Conve__0FCC5F4C] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemConversion_FromUnit_ToUnit] PRIMARY KEY NONCLUSTERED ([FromUnit_ID] ASC, [ToUnit_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemConversion_1__33] FOREIGN KEY ([FromUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemConversion_2__33] FOREIGN KEY ([ToUnit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemConversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemConversion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemConversion] TO [IRMAReportsRole]
    AS [dbo];

