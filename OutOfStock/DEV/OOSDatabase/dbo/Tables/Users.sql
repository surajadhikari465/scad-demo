CREATE TABLE [dbo].[Users] (
    [Username]       VARCHAR (100) NOT NULL,
    [LastLogin]      DATETIME2 (7) CONSTRAINT [df_users_lastlogin] DEFAULT (getdate()) NOT NULL,
    [Region]         VARCHAR (5)   NULL,
    [RegionOverride] VARCHAR (5)   NULL,
    [Store]          VARCHAR (5)   NULL,
    [StoreOverride]  VARCHAR (5)   NULL,
    CONSTRAINT [pk_users_username] PRIMARY KEY CLUSTERED ([Username] ASC) WITH (FILLFACTOR = 80)
);

