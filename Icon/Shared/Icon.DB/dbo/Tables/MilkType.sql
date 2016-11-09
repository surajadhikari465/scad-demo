Create table dbo.MilkType(
MilkTypeId int identity not null,
Description [nvarchar](100) not null,
 CONSTRAINT [PK_MilkType] PRIMARY KEY CLUSTERED 
(
	MilkTypeId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
