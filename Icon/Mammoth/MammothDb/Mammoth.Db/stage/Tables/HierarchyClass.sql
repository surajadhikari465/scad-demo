CREATE TABLE [stage].[HierarchyClass] (
    [HierarchyClassID]       INT            NOT NULL,
    [HierarchyID]            INT            NULL,
    [HierarchyClassName]          NVARCHAR (255) NULL,
    [HierarchyClassParentID] INT            NULL,
    [Timestamp]              DATETIME       NULL,
	[TransactionId]			 UNIQUEIDENTIFIER NOT NULL
);