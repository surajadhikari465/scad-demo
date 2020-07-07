USE ItemCatalog
GO

UPDATE RetentionPolicy 
SET DaysToKeep = 30
WHERE [Table] IN ('AppLog','MessageArchiveEvent')