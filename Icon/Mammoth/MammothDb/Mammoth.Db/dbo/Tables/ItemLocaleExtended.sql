CREATE TABLE [dbo].[ItemLocaleExtended] (
    [Region]         NVARCHAR (2)   NOT NULL,
    [ScanCode]       NVARCHAR (13)  NOT NULL,
    [BusinessUnitId] INT            NOT NULL,
    [AttributeId]    INT            NOT NULL,
    [AttributeValue] NVARCHAR (255) NOT NULL
);

