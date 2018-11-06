CREATE TABLE [dbo].[NutriFactsChgQueue] (
    [NutriFactsChgQueue_ID] INT      IDENTITY (1, 1) NOT NULL,
    [NutriFactsID]          INT      NOT NULL,
    [ActionCode]            CHAR (1) NOT NULL,
    [Store_No]              INT      NULL,
    CONSTRAINT [PK_NutriFactsChgQueue] PRIMARY KEY CLUSTERED ([NutriFactsChgQueue_ID] ASC),
    CONSTRAINT [FK_NutriFactsChgQueue_NutriFacts] FOREIGN KEY ([NutriFactsID]) REFERENCES [dbo].[NutriFacts] ([NutriFactsID])
);

