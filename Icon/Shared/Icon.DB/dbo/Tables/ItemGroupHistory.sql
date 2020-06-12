CREATE TABLE dbo.ItemGroupHistory 
( 
    ItemGroupId int NOT NULL, 
    ItemGroupTypeId int NOT NULL, 
    ItemGroupAttributesJson nvarchar(max) NOT NULL, 
    LastModifiedBy nvarchar(255) NOT NULL, 
    SysStartTimeUtc datetime2(7)  NOT NULL, 
    SysEndTimeUtc datetime2(7)  NOT NULL
) 
ON [FG_History]
GO
CREATE CLUSTERED INDEX IX_ItemGroupHistory_ItemGroupId ON dbo.ItemGroupHistory (ItemGroupId ASC) ON [FG_History]
GO