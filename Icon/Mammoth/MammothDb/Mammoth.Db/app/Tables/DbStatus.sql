CREATE TABLE [app].[DbStatus]
(
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusFlagName] [varchar](64) NOT NULL,
	[StatusFlagValue] [bit] NOT NULL,
	[LastUpdateDate] [datetime] DEFAULT (getdate()) NOT NULL,
	CONSTRAINT [PK_DbStatus] PRIMARY KEY CLUSTERED ([StatusId] ASC)
)