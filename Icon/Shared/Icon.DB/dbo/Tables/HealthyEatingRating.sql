Create table dbo.HealthyEatingRating(
HealthyEatingRatingId int identity not null,
Description [nvarchar](50) not null,
 CONSTRAINT [PK_HealthyEatingRating] PRIMARY KEY CLUSTERED 
(
	HealthyEatingRatingId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]