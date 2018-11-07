CREATE TABLE [ItemHistoryShrinkSubType]
 (
   [ItemHistoryShrinkSubType_ID] INT      IDENTITY (1, 1) NOT NULL,
   [ShrinkSubType_ID]			 INT      NOT NULL,
   [ItemHistoryID]				 INT      NOT NULL,
   [AddedDate]					 DATETIME NOT NULL,
   [ModifiedDate]				 DATETIME NULL,
   CONSTRAINT [ItemHistoryShrinkSubType_ID] PRIMARY KEY CLUSTERED ([ItemHistoryShrinkSubType_ID] ASC) WITH (FILLFACTOR = 80),
   CONSTRAINT [FK_ItemHistoryShrinkSubType_ShrinkSubType_ID] FOREIGN KEY ([ShrinkSubType_ID]) REFERENCES [dbo].[ShrinkSubType] ([ShrinkSubType_ID])
 );

GO
ALTER TABLE [dbo].[ItemHistoryShrinkSubType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO
GRANT SELECT, INSERT,UPDATE
    ON OBJECT::[dbo].[ItemHistoryShrinkSubType] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemHistoryShrinkSubType] TO [IRMASchedJobsRole]
    AS [dbo];

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistoryShrinkSubType] TO [IRMASchedJobsRole]
    AS [dbo];

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistoryShrinkSubType] TO [IRMA_Teradata]
    AS [dbo];