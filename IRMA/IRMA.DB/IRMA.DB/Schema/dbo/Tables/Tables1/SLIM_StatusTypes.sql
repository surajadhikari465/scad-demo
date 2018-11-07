CREATE TABLE [dbo].[SLIM_StatusTypes] (
    [StatusId] INT           NOT NULL,
    [Status]   VARCHAR (255) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StatusTypes] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SLIM_StatusTypes] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SLIM_StatusTypes] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StatusTypes] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SLIM_StatusTypes] TO [IRMASLIMRole]
    AS [dbo];

