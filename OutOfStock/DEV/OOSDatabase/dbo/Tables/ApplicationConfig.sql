CREATE TABLE [dbo].[ApplicationConfig] (
    [Key]   VARCHAR (50)  NOT NULL,
    [Value] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [pk_applicationconfig_key] PRIMARY KEY CLUSTERED ([Key] ASC) WITH (FILLFACTOR = 80)
);



