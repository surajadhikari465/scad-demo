CREATE TABLE [dbo].[RawScans] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [createdOn] DATETIME      CONSTRAINT [df_RawScanbs_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [Message]   VARCHAR (MAX) NOT NULL,
    [Status]    VARCHAR (10)  CONSTRAINT [df_RawScans_Status] DEFAULT ('new') NOT NULL,
    [ElapsedMS] BIGINT        NULL,
    CONSTRAINT [pk_RawScans] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_RawScans_Status]
    ON [dbo].[RawScans]([Status] ASC) WITH (FILLFACTOR = 80);

