CREATE TABLE [dbo].[ShelfTag_UnitPriceCode] (
    [UnitPriceCodeID] INT       IDENTITY (1, 1) NOT NULL,
    [UnitPriceCode]   CHAR (1)  NOT NULL,
    [UnitPriceDesc]   CHAR (25) NOT NULL,
    [CreateDate]      DATETIME  CONSTRAINT [DF_ShelfTag_UnitPriceCode_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]      DATETIME  CONSTRAINT [DF_ShelfTag_UnitPriceCode_ModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ShelfTag_UnitPriceCode] PRIMARY KEY CLUSTERED ([UnitPriceCodeID] ASC)
);

