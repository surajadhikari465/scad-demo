CREATE TABLE [dbo].[POSChangeType] (
    [POSChangeTypeKey] INT           IDENTITY (1, 1) NOT NULL,
    [POSDataTypeKey]   INT           NOT NULL,
    [ChangeTypeDesc]   VARCHAR (100) NULL,
    CONSTRAINT [PK_POSChangeType_POSChangeTypeKey] PRIMARY KEY CLUSTERED ([POSChangeTypeKey] ASC),
    CONSTRAINT [FK_POSChangeType_POSDataTypes] FOREIGN KEY ([POSDataTypeKey]) REFERENCES [dbo].[POSDataTypes] ([POSDataTypeKey])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangeType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangeType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangeType] TO [IRMAReportsRole]
    AS [dbo];

