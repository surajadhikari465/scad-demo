CREATE TABLE [dbo].[LocaleAttributes_TS](
	[Region]			[nchar](2)		DEFAULT ('TS')		NOT NULL,
	[LocaleAttributeID] [int]			IDENTITY(1,1)		NOT NULL,
	[LocaleID]          [int]                               NOT NULL,
	[AttributeID]		[int]								NULL,
	[AttributeValue]	[nvarchar](255)						NULL,
	[AddedDate]			[datetime]		DEFAULT (getdate()) NOT NULL,
	[ModifiedDate]		[datetime]							NULL,
 CONSTRAINT	[PK_LocaleAttributes_TS] PRIMARY KEY CLUSTERED ([Region] ASC,[LocaleAttributeID] ASC)
 WITH (FILLFACTOR = 100) ON [FG_RM]) ON [FG_RM]
GO

