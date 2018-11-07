CREATE TABLE [dbo].[ShelfTag_UnitPriceCodeJurisdiction] (
    [UnitPriceCodeID]   INT      NOT NULL,
    [UnitPriceCode]     CHAR (1) NOT NULL,
    [TaxJurisdictionID] INT      NOT NULL,
    [CreateDate]        DATETIME CONSTRAINT [DF_ShelfTag_UnitPriceCodeJurisdiction_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]        DATETIME CONSTRAINT [DF_ShelfTag_UnitPriceCodeJurisdiction_ModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ShelfTag_UnitPriceCodeJurisdiction] PRIMARY KEY CLUSTERED ([UnitPriceCodeID] ASC, [UnitPriceCode] ASC, [TaxJurisdictionID] ASC)
);

