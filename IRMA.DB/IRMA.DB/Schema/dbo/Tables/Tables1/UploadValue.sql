CREATE TABLE [dbo].[UploadValue] (
    [UploadValue_ID]     INT            IDENTITY (1, 1) NOT NULL,
    [UploadAttribute_ID] INT            NOT NULL,
    [UploadRow_ID]       INT            NOT NULL,
    [Value]              VARCHAR (4500) NULL,
    CONSTRAINT [PK_UploadValue] PRIMARY KEY CLUSTERED ([UploadValue_ID] ASC),
    CONSTRAINT [FK_UploadValue_UploadRow] FOREIGN KEY ([UploadRow_ID]) REFERENCES [dbo].[UploadRow] ([UploadRow_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxUploadValueRowAttribute]
    ON [dbo].[UploadValue]([UploadRow_ID] ASC, [UploadAttribute_ID] ASC)
    INCLUDE([UploadValue_ID], [Value]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadValue] TO [IRMAReportsRole]
    AS [dbo];

