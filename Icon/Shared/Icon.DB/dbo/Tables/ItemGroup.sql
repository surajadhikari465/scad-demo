CREATE TABLE dbo.ItemGroup 
( 
    ItemGroupId int NOT NULL identity(1000000, 1) CONSTRAINT PK_ItemGroupId PRIMARY KEY CLUSTERED, 
    ItemGroupTypeId int NOT NULL, 
    ItemGroupAttributesJson nvarchar(max) NOT NULL CONSTRAINT DF_ItemGroup_ItemGroupAttributesJson DEFAULT ('{}'), 
    LastModifiedBy nvarchar(255) NOT NULL, 
    SysStartTimeUtc datetime2(7) GENERATED ALWAYS AS ROW START, 
    SysEndTimeUtc datetime2(7) GENERATED ALWAYS AS ROW END, 
	vSkuDescription  AS (json_value(ItemGroupAttributesJson,'$.SkuDescription')),
	vPriceLineDescription  AS (json_value(ItemGroupAttributesJson,'$.PriceLineDescription')),
	vPriceLineSize  AS (json_value(ItemGroupAttributesJson,'$.PriceLineSize')),
	vPriceLineUOM  AS (json_value(ItemGroupAttributesJson,'$.PriceLineUOM')),
    PERIOD FOR SYSTEM_TIME (SysStartTimeUtc, SysEndTimeUtc), 
    CONSTRAINT FK_ItemGroup_ItemGroupTypeId FOREIGN KEY (ItemGroupTypeId) REFERENCES dbo.ItemGroupType (ItemGroupTypeId), 
    CONSTRAINT CK_ItemGroupAttributesJson_IsJson CHECK (ISJSON(ItemGroupAttributesJson) > 0) 
) 
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemGroupHistory)); 
GO

CREATE INDEX idx_ItemGroupMember_json_vSKUDescription ON [dbo].[ItemGroup](vSKUDescription);
GO

CREATE INDEX idx_ItemGroupMember_json_vPriceLineDescription ON [dbo].[ItemGroup](vPriceLineDescription);
GO

CREATE INDEX idx_ItemGroupMember_json_vPriceLineSize ON [dbo].[ItemGroup](vPriceLineSize);
GO

CREATE INDEX idx_ItemGroupMember_json_vPriceLineUOM ON [dbo].[ItemGroup](vPriceLineUOM);
GO
