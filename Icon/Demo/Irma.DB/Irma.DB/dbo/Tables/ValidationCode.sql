CREATE TABLE [dbo].[ValidationCode] (
    [ValidationCode]     INT            NOT NULL,
    [ValidationCodeType] INT            NOT NULL,
    [Description]        VARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_ValidationCode_ValidationCode] PRIMARY KEY CLUSTERED ([ValidationCode] ASC),
    CONSTRAINT [FK__ValidationCode_ValidationCodeType] FOREIGN KEY ([ValidationCodeType]) REFERENCES [dbo].[ValidationCodeType] ([ValidationCodeType])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidationCode] TO [IRMAReportsRole]
    AS [dbo];

