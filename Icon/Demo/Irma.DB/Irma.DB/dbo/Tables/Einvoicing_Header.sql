CREATE TABLE [dbo].[Einvoicing_Header] (
    [Einvoice_id]       INT           NOT NULL,
    [version]           VARCHAR (255) NOT NULL,
    [vendor_id]         VARCHAR (255) NOT NULL,
    [usage_indicator]   VARCHAR (10)  NOT NULL,
    [file_type]         VARCHAR (10)  NOT NULL,
    [trans_dt]          DATETIME      NOT NULL,
    [store]             VARCHAR (255) NOT NULL,
    [store_num]         INT           NOT NULL,
    [store_dept]        VARCHAR (255) NULL,
    [street1]           VARCHAR (255) NULL,
    [building]          VARCHAR (255) NULL,
    [street2]           VARCHAR (255) NULL,
    [city]              VARCHAR (255) NULL,
    [area]              VARCHAR (255) NULL,
    [state]             VARCHAR (255) NULL,
    [postal]            VARCHAR (255) NULL,
    [country]           VARCHAR (255) NULL,
    [invoice_num]       VARCHAR (255) NOT NULL,
    [invoice_date]      DATETIME      NOT NULL,
    [cust_num]          VARCHAR (255) NULL,
    [po_num]            VARCHAR (255) NULL,
    [invoice_amt]       MONEY         NOT NULL,
    [currency]          VARCHAR (255) NULL,
    [shipvia]           VARCHAR (255) NULL,
    [order_date]        DATETIME      NOT NULL,
    [order_num]         VARCHAR (255) NULL,
    [est_delivery_date] DATETIME      NULL,
    [reference]         VARCHAR (255) NULL,
    [lineCount]         INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Einvoice_id] ASC)
);





GO
ALTER TABLE [dbo].[Einvoicing_Header] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_Einvoicing_Header_invoicenum]
    ON [dbo].[Einvoicing_Header]([invoice_num] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Einvoicing_Header_invoicedate]
    ON [dbo].[Einvoicing_Header]([invoice_date] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Einvoicing_Header] TO [iCONReportingRole]
    AS [dbo];

