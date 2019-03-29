﻿CREATE TABLE [dbo].[InstructionListQueue] (
   [InstructionListQueueID] [int] IDENTITY(1,1) NOT NULL,
   [KeyID] [int] NOT NULL,
   [InsertDateUtc] [datetime2](7) NOT NULL,
   [Status]  NVARCHAR (3)   NOT NULL,
   [MessageTimestampUtc] [datetime2](7) NOT NULL,
   CONSTRAINT [PK_InstructionListQueue] PRIMARY KEY CLUSTERED ([InstructionListQueueID] ASC)
	)
GO

ALTER TABLE [dbo].[InstructionListQueue] ADD
CONSTRAINT Check_Status CHECK ([Status] in ('U','P','F'))
GO

ALTER TABLE [dbo].[InstructionListQueue] ADD  CONSTRAINT [DF_InstructionListQueue_InsertDate]  DEFAULT (sysdatetime()) FOR [InsertDateUtc]
GO

ALTER TABLE [dbo].[InstructionListQueue] ADD  CONSTRAINT [DF_InstructionListQueue_MessageTimestampUtc]  DEFAULT (sysutcdatetime()) FOR [MessageTimestampUtc]
GO
CREATE NONCLUSTERED INDEX [IdxInstructionListQueue_Status] ON [dbo].[InstructionListQueue]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO