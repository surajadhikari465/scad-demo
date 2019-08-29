﻿CREATE TABLE [app].[MessageQueueHierarchy] (
    [MessageQueueId]         INT            IDENTITY (1, 1) NOT NULL,
    [MessageTypeId]          INT            NOT NULL,
    [MessageStatusId]        INT            NOT NULL,
    [MessageHistoryId]       INT            NULL,
    [MessageActionId]        INT            NOT NULL,
    [InsertDate]             DATETIME2 (7)  CONSTRAINT [DF_MessageQueueHierarchy_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    [HierarchyId]            INT            NOT NULL,
    [HierarchyName]          NVARCHAR (255) NOT NULL,
    [HierarchyLevelName]     NVARCHAR (255) NOT NULL,
    [ItemsAttached]          BIT            NOT NULL,
    [HierarchyClassId]       NVARCHAR (32)  NOT NULL,
    [HierarchyClassName]     NVARCHAR (255) NOT NULL,
    [HierarchyLevel]         INT            NOT NULL,
    [HierarchyParentClassId] INT            NULL,
    [InProcessBy]            INT            NULL,
    [ProcessedDate]          DATETIME2 (7)  NULL,
    NationalClassCode        NVARCHAR(255)  NULL,
	[BrandAbbreviation]		 NVARCHAR(255)  NULL,
	[ZipCode]				 NVARCHAR(255)  NULL,
	[Designation]			 NVARCHAR(255)  NULL,
	[Locality]				 NVARCHAR(255)  NULL,

    CONSTRAINT [PK_MessageQueueHierarchy] PRIMARY KEY CLUSTERED ([MessageQueueId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_MessageQueueHierarchy_MessageActionId] FOREIGN KEY ([MessageActionId]) REFERENCES [app].[MessageAction] ([MessageActionId]),
    CONSTRAINT [FK_MessageQueueHierarchy_MessageHistoryId] FOREIGN KEY ([MessageHistoryId]) REFERENCES [app].[MessageHistory] ([MessageHistoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MessageQueueHierarchy_MessageStatusId] FOREIGN KEY ([MessageStatusId]) REFERENCES [app].[MessageStatus] ([MessageStatusId]),
    CONSTRAINT [FK_MessageQueueHierarchy_MessageTypeId] FOREIGN KEY ([MessageTypeId]) REFERENCES [app].[MessageType] ([MessageTypeId])
);