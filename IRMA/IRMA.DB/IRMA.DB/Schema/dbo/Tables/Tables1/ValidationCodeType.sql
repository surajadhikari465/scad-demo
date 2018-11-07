CREATE TABLE [dbo].[ValidationCodeType] (
    [ValidationCodeType] INT          IDENTITY (1, 1) NOT NULL,
    [Description]        VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ValidationCodeType_ValidationCodeType] PRIMARY KEY CLUSTERED ([ValidationCodeType] ASC)
);

