CREATE TABLE [dbo].[Status]
(
	[StatusID] INT NOT NULL IDENTITY PRIMARY KEY, 
    [StatusCode] NVARCHAR(3) NOT NULL, 
    [StatusDescription] NVARCHAR(100) NOT NULL
)
