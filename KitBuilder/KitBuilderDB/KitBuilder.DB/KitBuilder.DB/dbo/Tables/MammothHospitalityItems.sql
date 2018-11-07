CREATE TABLE  [dbo].[MammothHospitalityItems](
		[ItemId] [int] NOT NULL,
		[ScanCode] [nvarchar](13) NOT NULL,
		[ProductDesc] [nvarchar](255) NULL,
		[CustomerFriendlyDesc] [nvarchar](255) NULL,
		[KitchenDesc] [nvarchar](255) NULL,
		[BrandName] [nvarchar](255) NULL,
		[InsertDateUtc] [datetime2](7) NOT NULL,
		[LastUpdatedDateUtc] [datetime2](7) NULL,
		[ImageUrl] [nvarchar](max) NULL,
    );