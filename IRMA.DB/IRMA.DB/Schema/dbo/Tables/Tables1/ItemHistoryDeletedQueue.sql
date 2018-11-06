CREATE TABLE [dbo].[ItemHistoryDeletedQueue] (
    [Store_No]      INT      NULL,
    [Item_Key]      INT      NULL,
    [DateStamp]     DATETIME NULL,
    [SubTeam_No]    INT      NULL,
    [ItemHistoryID] INT      NULL,
    [Adjustment_ID] INT      NULL,
    [ID]            INT      IDENTITY (1, 1) NOT NULL
);

