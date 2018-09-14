CREATE TABLE [dbo].[LinkGroup]
(
	[LinkGroupId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [GroupName] NVARCHAR(100) NOT NULL, 
    [GroupDescription] NVARCHAR(500) NOT NULL,
    [InsertDate] DATETIME2 NOT NULL DEFAULT getdate(),
	[UpdatedDate] DATETIME2 NOT NULL DEFAULT getDate()
)