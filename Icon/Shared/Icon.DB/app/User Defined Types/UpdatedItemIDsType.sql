﻿CREATE TYPE [app].[UpdatedItemIDsType] AS TABLE(
	[itemID] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[itemID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO


