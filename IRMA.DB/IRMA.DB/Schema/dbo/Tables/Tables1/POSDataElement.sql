CREATE TABLE [dbo].[POSDataElement] (
    [POSDataTypeKey] INT           NOT NULL,
    [DataElement]    VARCHAR (100) NOT NULL,
    [Description]    VARCHAR (130) NULL,
    [IsBoolean]      BIT           NULL,
    CONSTRAINT [PK_POSDataElement] PRIMARY KEY CLUSTERED ([DataElement] ASC, [POSDataTypeKey] ASC),
    CONSTRAINT [FK_POSDataElement_POSDataTypes] FOREIGN KEY ([POSDataTypeKey]) REFERENCES [dbo].[POSDataTypes] ([POSDataTypeKey])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataElement] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataElement] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSDataElement] TO [IRMAReportsRole]
    AS [dbo];

