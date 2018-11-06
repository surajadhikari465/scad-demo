CREATE TABLE [dbo].[CycleCountExternalLoad] (
    [Store_No]    INT             NOT NULL,
    [Item_Key]    INT             NOT NULL,
    [Quantity]    DECIMAL (18, 4) CONSTRAINT [DF_CycleCountExternalLoad_Quantity] DEFAULT ((0)) NOT NULL,
    [Weight]      DECIMAL (18, 4) CONSTRAINT [DF_CycleCountExternalLoad_Weight] DEFAULT ((0)) NOT NULL,
    [PackSize]    DECIMAL (9, 4)  NULL,
    [IsCaseCnt]   BIT             CONSTRAINT [DF_CycleCountExternalLoad_IsCaseCnt] DEFAULT ((0)) NOT NULL,
    [SubTeam_No]  INT             NOT NULL,
    [BOHFileDate] DATETIME        NULL
);


GO
CREATE NONCLUSTERED INDEX [idx_CycleCountExternalLoad_StoreNo_SubTeam_No]
    ON [dbo].[CycleCountExternalLoad]([Store_No] ASC, [SubTeam_No] ASC)
    INCLUDE([Item_Key]) WITH (FILLFACTOR = 80);


GO
GRANT ALTER
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO PUBLIC
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO PUBLIC
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO PUBLIC
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountExternalLoad] TO [IRMAReportsRole]
    AS [dbo];

