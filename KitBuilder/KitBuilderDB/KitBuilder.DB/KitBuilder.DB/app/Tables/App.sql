CREATE TABLE [app].[App]
(
	[AppId] INT Identity(1,1) NOT NULL,
	[AppName] varchar(255) not null,
	CONSTRAINT [PK_App] Primary Key clustered ([AppId])
)
