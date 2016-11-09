CREATE TABLE [app].[VimMessageHistory] (
    [ID]					INT IDENTITY (1, 1) NOT NULL,
	[Level]					NVARCHAR (16)   NOT NULL,
	[Action]				NVARCHAR (255) NOT NULL,
	[Server]				NVARCHAR (255) NOT NULL,
	[InsertDate]			DATETIME2 (3) CONSTRAINT [VimMessageHistory_InsertDate_DF] DEFAULT (getdate()) NOT NULL,
	[Context]				NVARCHAR (MAX),
	[Error]					NVARCHAR (255), 
	[ErrorDetails]			NVARCHAR (4000),

    CONSTRAINT [VimMessageHistory_PK] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80),
);