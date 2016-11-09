CREATE TABLE [vim].[EventType] (
    [EventTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([EventTypeId] ASC) WITH (FILLFACTOR = 80)
);

