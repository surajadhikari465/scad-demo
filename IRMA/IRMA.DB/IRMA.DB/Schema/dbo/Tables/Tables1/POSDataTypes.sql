CREATE TABLE [dbo].[POSDataTypes] (
    [POSDataTypeKey] INT          IDENTITY (1, 1) NOT NULL,
    [DataTypeDesc]   VARCHAR (20) NULL,
    CONSTRAINT [PK_POSDataTypes] PRIMARY KEY CLUSTERED ([POSDataTypeKey] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataTypes] TO [IRMAReportsRole]
    AS [dbo];

