CREATE TABLE dbo.ItemGroupMember 
( 
    ItemId int NOT NULL, 
    ItemGroupId int NOT NULL, 
    IsPrimary bit NOT NULL CONSTRAINT DF_ItemGroupMember_IsPrimary DEFAULT ((0)), 
    LastModifiedBy nvarchar(255) NOT NULL, 
    SysStartTimeUtc datetime2(7) GENERATED ALWAYS AS ROW START, 
    SysEndTimeUtc datetime2(7) GENERATED ALWAYS AS ROW END, 
    PERIOD FOR SYSTEM_TIME (SysStartTimeUtc, SysEndTimeUtc), 
    CONSTRAINT PK_ItemGroupMember PRIMARY KEY CLUSTERED (ItemId, ItemGroupId), 
    CONSTRAINT FK_ItemGroupMember_ItemId FOREIGN KEY (ItemId) REFERENCES dbo.Item (ItemId), 
    CONSTRAINT FK_ItemGroupMember_ItemGroupId FOREIGN KEY (ItemGroupId) REFERENCES dbo.ItemGroup (ItemGroupId) 
) 
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemGroupMemberHistory)); 