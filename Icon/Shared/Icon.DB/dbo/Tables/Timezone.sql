CREATE TABLE [dbo].[Timezone] 
(
	[timezoneID] INT  NOT NULL IDENTITY, 
	[timezoneCode] nvarchar(5) NOT NULL,
	[timezoneName] NVARCHAR(100)  NULL,
	[gmtOffset] INT  NULL,  
	[dstStart] DATE  NULL,  
	[dstEnd] DATE  NULL,
	[posTimeZoneName] NVARCHAR(100) NULL
)
GO
ALTER TABLE [dbo].[Timezone] ADD CONSTRAINT [Timezone_timezoneCode_UNQ] UNIQUE ([timezoneCode])
GO
ALTER TABLE [dbo].[Timezone] ADD CONSTRAINT [Timezone_PK] PRIMARY KEY CLUSTERED ([timezoneID])
GO