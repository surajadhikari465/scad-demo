Create Table dbo.AnimalWelfareRating(
AnimalWelfareRatingId int identity not null,
Description [nvarchar](50) not null,
 CONSTRAINT [PK_AnimalWelfareRating] PRIMARY KEY CLUSTERED 
(
	AnimalWelfareRatingId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
