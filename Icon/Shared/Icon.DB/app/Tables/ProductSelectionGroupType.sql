CREATE TABLE [app].[ProductSelectionGroupType](
	[ProductSelectionGroupTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ProductSelectionGroupTypeName] [nvarchar](255) NULL,
 CONSTRAINT [PK__ProductSelectionGroupType] PRIMARY KEY CLUSTERED 
(
	[ProductSelectionGroupTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO