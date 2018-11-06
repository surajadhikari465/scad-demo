DELETE [dbo].[RetentionPolicy]
WHERE  [Table] = 'OrderExportQueue'

DELETE [dbo].[RetentionPolicy]
WHERE  [Table] = 'OrderExportDeletedQueue'

GO