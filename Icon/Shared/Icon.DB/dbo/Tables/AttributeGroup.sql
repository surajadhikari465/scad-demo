CREATE TABLE dbo.AttributeGroup
(
    AttributeGroupId INT IDENTITY (1, 1) NOT NULL,
	AttributeGroupName NVARCHAR(25),
	CONSTRAINT [PK_AttributeGroup] PRIMARY KEY CLUSTERED ([AttributeGroupId] ASC)
)