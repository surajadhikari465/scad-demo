CREATE TABLE [dbo].[OnHandHistory] (
    [Store_No]   INT             NOT NULL,
    [SubTeam_No] INT             NOT NULL,
    [Item_Key]   INT             NOT NULL,
    [Date_Key]   SMALLDATETIME   NOT NULL,
    [Quantity]   DECIMAL (18, 4) CONSTRAINT [DF_OnHandHistory_Quantity] DEFAULT ((0)) NOT NULL,
    [Weight]     DECIMAL (18, 4) CONSTRAINT [DF_OnHandHistory_Weight] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OnHandHistory] PRIMARY KEY NONCLUSTERED ([Store_No] ASC, [SubTeam_No] ASC, [Item_Key] ASC, [Date_Key] ASC) WITH (FILLFACTOR = 90)
);

