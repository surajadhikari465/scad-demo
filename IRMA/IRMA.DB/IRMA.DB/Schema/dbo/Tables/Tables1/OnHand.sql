CREATE TABLE [dbo].[OnHand] (
    [Item_Key]   INT             NOT NULL,
    [Store_No]   INT             NOT NULL,
    [SubTeam_No] INT             NOT NULL,
    [Quantity]   DECIMAL (18, 4) NOT NULL,
    [Weight]     DECIMAL (18, 4) NOT NULL,
    [LastReset]  DATETIME        NULL,
    CONSTRAINT [PK_OnHand] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_OnHandStoreSubTeam]
    ON [dbo].[OnHand]([Store_No] ASC, [SubTeam_No] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OnHand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OnHand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OnHand] TO [IRMAReportsRole]
    AS [dbo];

