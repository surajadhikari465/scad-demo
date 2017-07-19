CREATE TABLE [infor].[ItemSequence]
(
	[ItemSequenceID] INT IDENTITY(1,1) NOT NULL,
	[ItemID] INT NOT NULL,
	[SequenceID] NUMERIC(22,0)  NOT NULL
)
GO
ALTER TABLE [infor].[ItemSequence] WITH CHECK ADD CONSTRAINT [ItemSequence_ItemID_FK] FOREIGN KEY (
[ItemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [infor].[ItemSequence] ADD CONSTRAINT [ItemSequenceID_PK] PRIMARY KEY CLUSTERED (
[ItemSequenceID]
)
GO
CREATE NONCLUSTERED INDEX IX_ItemSequence_ItemID on  [infor].[ItemSequence] (ItemID)
GO
