SELECT * INTO #tmpNatITemCat FROM NatItemCat
SELECT * INTO #tmpNatITemFamily FROM NatItemfamily
SELECT * INTO #tmpNatITemClass FROM NatItemClass

--Create Back uo of table
--SELECT * INTO NatItemCatBackUp FROM NatItemCat
--SELECT * INTO NatItemfamilyBackUp FROM NatItemfamily
--SELECT * INTO NatItemClassBackUp FROM NatItemClass


-- disable all constraints
ALTER TABLE NatItemCat NOCHECK CONSTRAINT ALL
ALTER TABLE NatItemfamily NOCHECK CONSTRAINT ALL
ALTER TABLE NatItemClass NOCHECK CONSTRAINT ALL

-- disable change_tracking
ALTER TABLE [dbo].[NatItemFamily] Disable Change_tracking;
ALTER TABLE [dbo].[NatItemClass] Disable Change_tracking;
ALTER TABLE [dbo].[NatItemCat] Disable Change_tracking;

--Delete all data
DELETE FROM NatItemCat
DELETE FROM NatItemfamily

-- Drop Constraint
ALTER TABLE [dbo].[NatItemFamily] DROP  CONSTRAINT [PK_NatItemFamily]
--ALTER TABLE [dbo].[NatItemClass] DROP  CONSTRAINT [PK_NatItemClass]
ALTER TABLE [dbo].[NatItemCat] DROP  CONSTRAINT [PK_NatItemCat] 

-- Drop Columns
ALTER TABLE NatItemCat
DROP COLUMN NatCatID

ALTER TABLE NatItemCat
ADD NatCatID INT NOT NULL  IDENTITY(1,1) 

ALTER TABLE NatItemCat
DROP COLUMN NatCatName

ALTER TABLE NatItemCat
ADD NatCatName  Varchar(65) NULL

ALTER TABLE NatItemCat
DROP COLUMN NatFamilyID

ALTER TABLE NatItemCat
ADD NatFamilyID  INT NOT NULL

ALTER TABLE NatItemCat
DROP COLUMN LastUpdateTimestamp

ALTER TABLE NatItemCat
ADD LastUpdateTimestamp  datetime NULL


ALTER TABLE NatItemfamily
DROP COLUMN NatFamilyID

--Add Columns
ALTER TABLE NatItemfamily
ADD NatFamilyID  INT NOT NULL IDENTITY(1,1)  

ALTER TABLE NatItemfamily
DROP COLUMN NatFamilyName

ALTER TABLE NatItemfamily
ADD NatFamilyName Varchar(65) NULL

ALTER TABLE NatItemfamily
DROP COLUMN NatSubTeam_No

ALTER TABLE NatItemfamily
ADD NatSubTeam_No int NULL

ALTER TABLE NatItemfamily
DROP COLUMN SubTeam_No

ALTER TABLE NatItemfamily
ADD SubTeam_No int NULL

ALTER TABLE NatItemfamily
DROP COLUMN LastUpdateTimestamp

ALTER TABLE NatItemfamily
ADD LastUpdateTimestamp datetime NULL


Set Identity_insert NatItemCat ON
INSERT INTO [dbo].[NatItemCat]
           (NatCatID,
           [NatCatName]
           ,[NatFamilyID]
           ,[LastUpdateTimestamp])
           
SELECT NatCatID,[NatCatName],[NatFamilyID],[LastUpdateTimestamp] FROM #tmpNatITemCat
 
Set Identity_insert NatItemCat OFF

Set Identity_insert NatItemfamily ON
INSERT INTO [dbo].[NatItemFamily]
           (NatFamilyID,
			[NatFamilyName]
           ,[NatSubTeam_No]
           ,[SubTeam_No]
           ,[LastUpdateTimestamp])
 SELECT NatFamilyID,NatFamilyName,NatSubTeam_No,SubTeam_No,LastUpdateTimestamp FROM #tmpNatITemFamily
Set Identity_insert NatItemfamily OFF

--enable Contraints
ALTER TABLE NatItemCat CHECK CONSTRAINT ALL
ALTER TABLE NatItemfamily CHECK CONSTRAINT ALL
ALTER TABLE NatItemClass CHECK CONSTRAINT ALL

--Alter Table
ALTER TABLE [dbo].[NatItemCat] ADD  CONSTRAINT [PK_NatItemCat] PRIMARY KEY CLUSTERED 
(
	[NatCatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
--ALTER TABLE [dbo].[NatItemClass] ADD  CONSTRAINT [PK_NatItemClass] PRIMARY KEY CLUSTERED 
--(
--	[ClassID] ASC
--)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--GO
ALTER TABLE [dbo].[NatItemFamily] ADD  CONSTRAINT [PK_NatItemFamily] PRIMARY KEY CLUSTERED 
(
	[NatFamilyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


ALTER TABLE [dbo].[NatItemFamily] Enable Change_tracking;
ALTER TABLE [dbo].[NatItemClass] Enable Change_tracking;
ALTER TABLE [dbo].[NatItemCat] Enable Change_tracking;

--Update Item


DROP TABLE #tmpNatITemCat
DROP TABLE #tmpNatITemFamily
DROP TABLE #tmpNatITemClass