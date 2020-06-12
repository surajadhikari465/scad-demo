CREATE TABLE dbo.ItemGroup 
( 
    ItemGroupId int NOT NULL identity(1000000, 1) CONSTRAINT PK_ItemGroupId PRIMARY KEY CLUSTERED, 
    ItemGroupTypeId int NOT NULL, 
    ItemGroupAttributesJson nvarchar(max) NOT NULL CONSTRAINT DF_ItemGroup_ItemGroupAttributesJson DEFAULT ('{}'), 
    LastModifiedBy nvarchar(255) NOT NULL, 
    SysStartTimeUtc datetime2(7) GENERATED ALWAYS AS ROW START, 
    SysEndTimeUtc datetime2(7) GENERATED ALWAYS AS ROW END, 
    PERIOD FOR SYSTEM_TIME (SysStartTimeUtc, SysEndTimeUtc), 
    CONSTRAINT FK_ItemGroup_ItemGroupTypeId FOREIGN KEY (ItemGroupTypeId) REFERENCES dbo.ItemGroupType (ItemGroupTypeId), 
    CONSTRAINT CK_ItemGroupAttributesJson_IsJson CHECK (ISJSON(ItemGroupAttributesJson) > 0) 
) 
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemGroupHistory)); 