CREATE TABLE dbo.ItemHistory
(
	ItemId INT NOT NULL,
	ItemTypeId INT NOT NULL,
	ItemAttributesJson NVARCHAR(MAX) NOT NULL,
	SysStartTimeUtc datetime2  NOT NULL,
	SysEndTimeUtc datetime2 NOT NULL,
	Inactive nvarchar(4000) NULL,
	ProductDescription nvarchar(4000) NULL,
	CustomerFriendlyDescription nvarchar(4000) NULL,
	POSDescription nvarchar(4000) NULL,
	FoodStamp nvarchar(4000) NULL
) ON [FG_History]
GO

CREATE CLUSTERED INDEX IX_ItemHistory_ItemId ON dbo.ItemHistory (ItemId ASC) ON [FG_History]
GO