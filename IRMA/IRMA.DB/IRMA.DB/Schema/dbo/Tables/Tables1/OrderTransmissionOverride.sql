CREATE TABLE [dbo].[OrderTransmissionOverride] (
    [OrderHeader_ID] INT          NOT NULL,
    [Target]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OrderTransmissionOverride] PRIMARY KEY CLUSTERED ([OrderHeader_ID] ASC),
    CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderTransmissionOverride] TO [IRMAReportsRole]
    AS [dbo];

