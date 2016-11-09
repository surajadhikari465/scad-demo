/****** Object:  Table [app].[IRMAItem]    Script Date: 8/19/2014 2:03:46 PM ******/
DROP TABLE [app].[IRMAItem]
GO

/****** Object:  Table [app].[IRMAItem]    Script Date: 8/19/2014 2:03:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [app].[IRMAItem](
	[irmaItemID] [int] IDENTITY(1,1) NOT NULL,
	[regioncode] [varchar](2) NOT NULL,
	[identifier] [varchar](13) NOT NULL,
	[defaultIdentifier] [bit] NOT NULL CONSTRAINT [DF_defaultIdentifier]  DEFAULT ((1)),
	[brandName] [varchar](25) NOT NULL,
	[itemDescription] [varchar](60) NOT NULL,
	[posDescription] [varchar](60) NOT NULL,
	[packageUnit] [int] NOT NULL,
	[foodStamp] [bit] NOT NULL,
	[posScaleTare] [decimal](18, 0) NOT NULL,
	[departmentSale] [bit] NOT NULL,
	[giftCard] [bit] NULL,
	[taxClassID] [int] NULL,
	[merchandiseClassID] [int] NULL,
	[insertDate] [datetime2](7) NOT NULL,	
 CONSTRAINT [PK_IRMAItem] PRIMARY KEY CLUSTERED 
(
	[irmaItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO
