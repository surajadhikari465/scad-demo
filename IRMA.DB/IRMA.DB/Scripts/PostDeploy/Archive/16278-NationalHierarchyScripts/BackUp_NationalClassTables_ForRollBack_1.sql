IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemCatBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].[NatItemCatBackUp]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemClassBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].[NatItemClassBackUp]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemfamilyBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [NatItemfamilyBackUp]

CREATE TABLE [dbo].[NatItemCatBackUp](
	[NatCatID] [int] NOT NULL,
	[NatCatName] [varchar](65) NULL,
	[NatFamilyID] [int] NOT NULL,
	[LastUpdateTimestamp] [datetime] NULL

 CONSTRAINT [PK_NatItemCatBackUp] PRIMARY KEY CLUSTERED 
(
	[NatCatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[NatItemClassBackUp](
	[ClassID] [int] NOT NULL,
	[ClassName] [varchar](65) NULL,
	[NatCatID] [int] NOT NULL,
	[LastUpdateTimestamp] [datetime] NULL,
 CONSTRAINT [PK_NatItemClassBackUp] PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[NatItemfamilyBackUp](
	[NatFamilyID] [int]  NOT NULL,
	[NatFamilyName] [varchar](65) NULL,
	[NatSubTeam_No] [int] NULL,
	[SubTeam_No] [int] NULL,
	[LastUpdateTimestamp] [datetime] NULL

 CONSTRAINT [PK_NatItemFamilyBackUp] PRIMARY KEY CLUSTERED 
(
	[NatFamilyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [NatItemCatBackUp]
SELECT * FROM  NatItemCat

INSERT INTO NatItemfamilyBackUp
SELECT * FROM  NatItemfamily

INSERT INTO NatItemClassBackUp
SELECT * FROM  NatItemClass