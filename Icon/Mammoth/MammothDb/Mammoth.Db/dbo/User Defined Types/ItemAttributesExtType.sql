CREATE TYPE dbo.ItemAttributesExtType AS TABLE
(
    [ItemID]          INT            NOT NULL,
    [AttributeCode]   NVARCHAR(3)    NOT NULL,
    [AttributeValue]  NVARCHAR (255) NULL
);
GO

GRANT EXEC ON type::dbo.ItemAttributesExtType TO MammothRole
GO