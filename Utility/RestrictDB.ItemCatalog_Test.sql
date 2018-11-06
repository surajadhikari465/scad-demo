-- Lock users out of the database while performing admin work.
ALTER DATABASE ItemCatalog_Test
SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
GO
