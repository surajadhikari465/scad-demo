-- lock users out of the database 
-- while performing admin work

-- ItemCatalog

ALTER DATABASE ItemCatalog
SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE

GO
