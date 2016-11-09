Create table dbo.SeafoodFreshOrFrozen(
SeafoodFreshOrFrozenId int identity not null,
Description [nvarchar](50) not null,
 CONSTRAINT [PK_SeafoodFreshOrFrozen] PRIMARY KEY CLUSTERED 
(
	SeafoodFreshOrFrozenId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]