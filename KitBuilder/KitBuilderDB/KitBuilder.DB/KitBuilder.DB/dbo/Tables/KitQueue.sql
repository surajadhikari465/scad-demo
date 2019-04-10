CREATE TABLE [dbo].[KitQueue] (
   [KitQueueId] [int] IDENTITY(1,1) NOT NULL,
   [KitId] [int] NOT NULL,
   [StoreId][int] NOT NULL,
   [VenueId][int] NOT NULL,
   [kitLocaleId][int] NOT NULL,
   [InsertDateUtc] [datetime2](7) NOT NULL,
   [Status]  NVARCHAR (3)   NOT NULL,
   [MessageTimestampUtc] [datetime2](7) NULL,
   CONSTRAINT [PK_KitQueue] PRIMARY KEY CLUSTERED ([KitQueueId] ASC)
	)
GO

ALTER TABLE [dbo].[KitQueue] ADD
CONSTRAINT Check_KitStatus CHECK ([Status] in ('U','P','F', 'UA'))
GO

ALTER TABLE [dbo].[KitQueue] ADD  CONSTRAINT [DF_KitQueue_InsertDate]  DEFAULT (sysdatetime()) FOR [InsertDateUtc]
GO

CREATE NONCLUSTERED INDEX [IdxKitQueue_Status] ON [dbo].[KitQueue]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO