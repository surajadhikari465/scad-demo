CREATE TABLE [mammoth].[ItemChangeEventType] (
    [EventTypeID]   INT            IDENTITY (1, 1) NOT NULL,
    [EventTypeName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EventTypeID] PRIMARY KEY CLUSTERED ([EventTypeID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[mammoth].[ItemChangeEventType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[ItemChangeEventType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[ItemChangeEventType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[mammoth].[ItemChangeEventType] TO [MammothRole]
    AS [dbo];

