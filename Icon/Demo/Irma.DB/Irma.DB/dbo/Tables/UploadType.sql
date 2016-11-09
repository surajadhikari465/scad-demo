CREATE TABLE [dbo].[UploadType] (
    [UploadType_Code] VARCHAR (50)  NOT NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [Description]     VARCHAR (255) NULL,
    [IsActive]        BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Spreadsheet] PRIMARY KEY CLUSTERED ([UploadType_Code] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadType] TO [IRMAReportsRole]
    AS [dbo];

