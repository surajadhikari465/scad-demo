CREATE TABLE dbo.ItemGroupMemberHistory 
( 
    ItemId int NOT NULL, 
    ItemGroupId int NOT NULL, 
    IsPrimary bit NOT NULL, 
    LastModifiedBy nvarchar(255) NOT NULL, 
    SysStartTimeUtc datetime2(7) NOT NULL, 
    SysEndTimeUtc datetime2(7)  NOT NULL
) 
ON [FG_History]
GO
CREATE CLUSTERED INDEX IX_ItemGroupMemberHistory_ItemIdItemGroupId ON dbo.ItemGroupMemberHistory (ItemId,ItemGroupId ASC) ON [FG_History]
GO