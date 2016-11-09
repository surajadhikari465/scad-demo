﻿CREATE TABLE [app].[PLUCategory](
	[PluCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[PluCategoryName] [nvarchar](255) NOT NULL,
	[BeginRange] [nvarchar](11) NOT NULL,
	[EndRange] [nvarchar](11) NOT NULL,
 CONSTRAINT [PluCategory_PK] PRIMARY KEY CLUSTERED 
(
	[PluCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
