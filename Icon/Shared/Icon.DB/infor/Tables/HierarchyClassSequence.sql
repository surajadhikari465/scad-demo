CREATE TABLE [infor].[HierarchyClassSequence]
(
	[HierarchyClassSequenceID] INT IDENTITY(1,1) NOT NULL,
	[HierarchyClassID] INT NOT NULL,
	[SequenceID] NUMERIC(22,0)  NOT NULL
)
GO
ALTER TABLE [infor].[HierarchyClassSequence] WITH CHECK ADD CONSTRAINT [HierarchyClassSequence_HierarchyClassID_FK] FOREIGN KEY (
[HierarchyClassID]
)
REFERENCES [dbo].[HierarchyClass] (
[HierarchyClassID]
)
GO
ALTER TABLE [infor].[HierarchyClassSequence] ADD CONSTRAINT [HierarchyClassSequenceID_PK] PRIMARY KEY CLUSTERED (
[HierarchyClassSequenceID]
)
GO
CREATE NONCLUSTERED INDEX IX_HierarchyClassSequence_HierarchyClassSequenceID on  [infor].[HierarchyClassSequence] (HierarchyClassSequenceID)
GO
