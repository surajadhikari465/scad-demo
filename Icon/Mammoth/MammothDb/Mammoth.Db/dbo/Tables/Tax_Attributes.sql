CREATE TABLE [dbo].[Tax_Attributes]
(
	[TaxAttributesID]	INT IDENTITY	(1, 1) NOT NULL, 
    [TaxHCID]			INT				NOT NULL,
	[TaxCode]			NVARCHAR(7)		NOT NULL,
	[AddedDate]         DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]      DATETIME		NULL,
	CONSTRAINT [PK_Tax_Attributes] PRIMARY KEY CLUSTERED ([TaxAttributesID] ASC) WITH (FILLFACTOR = 100)
)
