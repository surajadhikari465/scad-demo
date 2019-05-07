CREATE TABLE [dbo].[ApplicationRoles]
(
	[Role] nvarchar(25) NOT NULL PRIMARY KEY, 
	[ADGroups] nvarchar(255) not null
)
