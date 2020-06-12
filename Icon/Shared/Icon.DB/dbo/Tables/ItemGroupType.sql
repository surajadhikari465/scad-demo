CREATE TABLE dbo.ItemGroupType 
( 
    ItemGroupTypeId int NOT NULL identity(1,1) CONSTRAINT PK_ItemGroupType PRIMARY KEY CLUSTERED, 
    ItemGroupTypeName nvarchar(255) 
) 