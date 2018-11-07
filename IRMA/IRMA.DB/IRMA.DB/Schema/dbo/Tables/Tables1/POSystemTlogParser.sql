CREATE TABLE [dbo].[POSystemTlogParser] (
    [POSSystemId] INT          NULL,
    [TlogParser]  VARCHAR (20) NULL,
    FOREIGN KEY ([POSSystemId]) REFERENCES [dbo].[POSSystemTypes] ([POSSystemId])
);

