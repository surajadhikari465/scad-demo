CREATE TABLE [dbo].[JobErrorLog] (
    [Classname]     VARCHAR (50)   NOT NULL,
    [RunDate]       DATETIME       NOT NULL,
    [ServerName]    VARCHAR (50)   NULL,
    [ExceptionText] VARCHAR (2000) NULL
);

