CREATE TABLE [dbo].[NutriFactsChgQueueTmp] (
    [NutriFactsChgQueueTmp_ID] INT      IDENTITY (1, 1) NOT NULL,
    [NutriFactsChgQueue_ID]    INT      NOT NULL,
    [NutriFact_ID]             INT      NOT NULL,
    [ActionCode]               CHAR (1) NOT NULL,
    [Store_No]                 INT      NULL,
    CONSTRAINT [PK_NutriFactsChgQueueTmp] PRIMARY KEY CLUSTERED ([NutriFactsChgQueueTmp_ID] ASC)
);

