CREATE TABLE [dbo].[OrderInvoice_ControlGroupStatus] (
    [OrderInvoice_ControlGroupStatus_ID]   INT          NOT NULL,
    [OrderInvoice_ControlGroupStatus_Desc] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OrderInvoice_ControlGroupStatus] PRIMARY KEY CLUSTERED ([OrderInvoice_ControlGroupStatus_ID] ASC)
);

