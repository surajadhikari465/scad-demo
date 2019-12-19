CREATE TABLE [infor].[ItemHistory]
(
	ItemHistoryId int IDENTITY(1,1) CONSTRAINT PK_infor_ItemHistory PRIMARY KEY CLUSTERED,
	ItemId int NOT NULL,
	JsonObject nvarchar(max) NOT NULL
) ON [FG_HISTORY]
GO

CREATE NONCLUSTERED INDEX IX_Infor_ItemHistory_ItemId ON infor.ItemHistory (ItemId ASC)
GO