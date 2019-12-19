CREATE TABLE dbo.PickListData

(
	PickListId INT IDENTITY (1, 1) NOT NULL,
	AttributeId INT,
	PickListValue NVARCHAR(50),
	CONSTRAINT [FK_PickListData_Attributes_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]) ON DELETE CASCADE,  
    CONSTRAINT [PK_PickListData] PRIMARY KEY CLUSTERED (PickListId ASC)
)