CREATE TABLE [dbo].[ResolutionCodes] (
    [ResolutionCodeID] INT           IDENTITY (1, 1) NOT NULL,
    [Description]      VARCHAR (128) NOT NULL,
    [Default]          BIT           NOT NULL,
    [Active]           BIT           NOT NULL,
    CONSTRAINT [PK_ResolutionCode] PRIMARY KEY CLUSTERED ([ResolutionCodeID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ResolutionCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ResolutionCodes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ResolutionCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ResolutionCodes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ResolutionCodes] TO [IRMAReportsRole]
    AS [dbo];

