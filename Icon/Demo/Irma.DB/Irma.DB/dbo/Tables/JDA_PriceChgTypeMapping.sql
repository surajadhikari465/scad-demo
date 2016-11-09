CREATE TABLE [dbo].[JDA_PriceChgTypeMapping] (
    [PriceChgTypeID] INT      NOT NULL,
    [Priority]       SMALLINT NOT NULL,
    [JDA_ID]         CHAR (1) NOT NULL,
    [JDA_Priority]   SMALLINT NOT NULL,
    CONSTRAINT [PK_JDA_PriceChgTypeMapping] PRIMARY KEY CLUSTERED ([PriceChgTypeID] ASC)
);

