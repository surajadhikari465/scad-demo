CREATE TABLE [dbo].[OrderHeaderHistory] (
    [OrderHeader_ID]            INT            NOT NULL,
    [InvoiceNumber]             VARCHAR (20)   NULL,
    [OrderHeaderDesc]           VARCHAR (4000) NULL,
    [Vendor_ID]                 INT            NOT NULL,
    [PurchaseLocation_ID]       INT            NOT NULL,
    [ReceiveLocation_ID]        INT            NOT NULL,
    [CreatedBy]                 INT            NOT NULL,
    [OrderDate]                 SMALLDATETIME  NOT NULL,
    [CloseDate]                 DATETIME       NULL,
    [OriginalCloseDate]         DATETIME       NULL,
    [SystemGenerated]           BIT            NOT NULL,
    [Sent]                      BIT            NOT NULL,
    [Fax_Order]                 BIT            NOT NULL,
    [Expected_Date]             SMALLDATETIME  NULL,
    [SentDate]                  SMALLDATETIME  NULL,
    [QuantityDiscount]          DECIMAL (9, 2) NOT NULL,
    [DiscountType]              INT            NOT NULL,
    [Transfer_SubTeam]          INT            NULL,
    [Transfer_To_SubTeam]       INT            NULL,
    [Return_Order]              BIT            NOT NULL,
    [User_ID]                   INT            NULL,
    [Temperature]               TINYINT        NULL,
    [Accounting_In_DateStamp]   SMALLDATETIME  NULL,
    [Accounting_In_UserID]      INT            NULL,
    [InvoiceDate]               SMALLDATETIME  NULL,
    [ApprovedDate]              SMALLDATETIME  NULL,
    [ApprovedBy]                INT            NULL,
    [UploadedDate]              SMALLDATETIME  NULL,
    [RecvLogDate]               DATETIME       NULL,
    [RecvLog_No]                INT            NULL,
    [RecvLogUser_ID]            INT            NULL,
    [VendorDoc_ID]              VARCHAR (16)   NULL,
    [VendorDocDate]             SMALLDATETIME  NULL,
    [WarehouseSent]             BIT            NOT NULL,
    [WarehouseSentDate]         SMALLDATETIME  NULL,
    [Host_Name]                 VARCHAR (20)   NULL,
    [InsertDate]                DATETIME       CONSTRAINT [DF_OrderHeaderHistory_InsertDate] DEFAULT (getdate()) NOT NULL,
    [OrderType_ID]              TINYINT        NULL,
    [ProductType_ID]            TINYINT        NULL,
    [FromQueue]                 BIT            CONSTRAINT [DF_OrderHeaderHistory_FromQueue] DEFAULT ((0)) NOT NULL,
    [SentToFaxDate]             SMALLDATETIME  NULL,
    [ClosedBy]                  INT            NULL,
    [MatchingValidationCode]    INT            NULL,
    [MatchingUser_ID]           INT            NULL,
    [MatchingDate]              DATETIME       NULL,
    [Freight3Party_OrderCost]   SMALLMONEY     NULL,
    [WarehouseCancelled]        DATETIME       NULL,
    [PayByAgreedCost]           BIT            CONSTRAINT [DF_OrderHeaderHistory_PayByAgreedCost] DEFAULT ((0)) NOT NULL,
    [OrderRefreshCostSource_ID] INT            NULL
);


GO
CREATE NONCLUSTERED INDEX [idxOrderHeaderHistoryOrderHeader_ID]
    ON [dbo].[OrderHeaderHistory]([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeaderHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeaderHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderHeaderHistory] TO [IRMAReportsRole]
    AS [dbo];

