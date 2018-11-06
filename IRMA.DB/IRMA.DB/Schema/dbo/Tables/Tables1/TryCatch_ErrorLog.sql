CREATE TABLE [dbo].[TryCatch_ErrorLog] (
    [ErrorLogID]     INT             IDENTITY (1, 1) NOT NULL,
    [ErrorDateTime]  DATETIME        DEFAULT (getdate()) NOT NULL,
    [AppName]        NVARCHAR (128)  NULL,
    [HostName]       NVARCHAR (100)  NULL,
    [CurrentUser]    [sysname]       NULL,
    [SystemUser]     NVARCHAR (100)  NULL,
    [ErrorNumber]    INT             NULL,
    [ErrorSeverity]  INT             NULL,
    [ErrorState]     INT             NULL,
    [ErrorProcedure] NVARCHAR (126)  NULL,
    [ErrorLine]      INT             NULL,
    [ErrorMessage]   NVARCHAR (4000) NULL,
    [AdditionalInfo] NVARCHAR (1000) NULL,
    CONSTRAINT [PK_TryCatch_ErrorLog_ErrorLogID] PRIMARY KEY CLUSTERED ([ErrorLogID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TryCatch_ErrorLog_ErrorDateTime]
    ON [dbo].[TryCatch_ErrorLog]([ErrorDateTime] ASC);

