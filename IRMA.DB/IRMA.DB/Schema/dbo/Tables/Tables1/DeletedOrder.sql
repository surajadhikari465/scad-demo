CREATE TABLE [dbo].[DeletedOrder] (
    [DeletedOrder_ID]              INT             IDENTITY (1, 1) NOT NULL,
    [OrderHeader_ID]               INT             NOT NULL,
    [User_ID]                      INT             NOT NULL,
    [DeleteDate]                   SMALLDATETIME   CONSTRAINT [DF_DeletedOrder_DeleteDate] DEFAULT (getdate()) NOT NULL,
    [Vendor_ID]                    INT             NOT NULL,
    [ReceiveLocation_ID]           INT             NOT NULL,
    [CreatedBy]                    INT             NOT NULL,
    [OrderDate]                    SMALLDATETIME   NOT NULL,
    [SentDate]                     SMALLDATETIME   NULL,
    [Transfer_SubTeam]             INT             NULL,
    [Transfer_To_SubTeam]          INT             NULL,
    [OrderType_ID]                 TINYINT         NULL,
    [ProductType_ID]               TINYINT         NULL,
    [InvoiceNumber]                VARCHAR (20)    NULL,
    [OrderHeaderDesc]              VARCHAR (4000)  NULL,
    [PurchaseLocation_ID]          INT             NULL,
    [CloseDate]                    DATETIME        NULL,
    [OriginalCloseDate]            DATETIME        NULL,
    [SystemGenerated]              BIT             NULL,
    [Sent]                         BIT             NULL,
    [Fax_Order]                    BIT             NULL,
    [Expected_Date]                SMALLDATETIME   NULL,
    [QuantityDiscount]             DECIMAL (18, 4) NULL,
    [DiscountType]                 INT             NULL,
    [Return_Order]                 BIT             NULL,
    [OrderHeader_User_ID]          INT             NULL,
    [Temperature]                  DECIMAL (9, 2)  NULL,
    [Accounting_In_DateStamp]      SMALLDATETIME   NULL,
    [Accounting_In_UserID]         INT             NULL,
    [InvoiceDate]                  SMALLDATETIME   NULL,
    [ApprovedDate]                 SMALLDATETIME   NULL,
    [ApprovedBy]                   INT             NULL,
    [UploadedDate]                 SMALLDATETIME   NULL,
    [RecvLogDate]                  DATETIME        NULL,
    [RecvLog_No]                   INT             NULL,
    [RecvLogUser_ID]               INT             NULL,
    [VendorDoc_ID]                 VARCHAR (16)    NULL,
    [VendorDocDate]                SMALLDATETIME   NULL,
    [WarehouseSent]                BIT             NULL,
    [WarehouseSentDate]            SMALLDATETIME   NULL,
    [SentToFaxDate]                SMALLDATETIME   NULL,
    [FromQueue]                    BIT             NULL,
    [ClosedBy]                     INT             NULL,
    [IsDropShipment]               BIT             NULL,
    [OrderExternalSourceID]        INT             NULL,
    [OrderExternalSourceOrderID]   INT             NULL,
    [MatchingValidationCode]       INT             NULL,
    [MatchingUser_ID]              INT             NULL,
    [MatchingDate]                 DATETIME        NULL,
    [Freight3Party_OrderCost]      SMALLMONEY      NULL,
    [DVOOrderID]                   VARCHAR (10)    NULL,
    [eInvoice_Id]                  INT             NULL,
    [Electronic_Order]             BIT             NULL,
    [PayByAgreedCost]              BIT             NULL,
    [Email_Order]                  BIT             NULL,
    [SentToEmailDate]              SMALLDATETIME   NULL,
    [OverrideTransmissionMethod]   BIT             NULL,
    [SupplyTransferToSubTeam]      INT             NULL,
    [AccountingUploadDate]         DATETIME        NULL,
    [SentToElectronicDate]         SMALLDATETIME   NULL,
    [InvoiceDiscrepancy]           BIT             NULL,
    [InvoiceDiscrepancySentDate]   SMALLDATETIME   NULL,
    [InvoiceProcessingDiscrepancy] BIT             NULL,
    [WarehouseCancelled]           DATETIME        NULL,
    [PurchaseAccountsTotal]        MONEY           NULL,
    [CurrencyID]                   INT             NULL,
    [APUploadedCost]               MONEY           NULL,
    [QtyShippedProvided]           BIT             NULL,
    [POCostDate]                   DATETIME        NULL,
    [AdminNotes]                   VARCHAR (5000)  NULL,
    [ResolutionCodeID]             INT             NULL,
    [InReview]                     BIT             NULL,
    [InReviewUser]                 INT             NULL,
    [ReasonCodeDetailID]           INT             NULL,
    [RefuseReceivingReasonID]      INT             NULL,
    [OrderedCost]                  MONEY           NULL,
    [OriginalReceivedCost]         MONEY           NULL,
    [TotalPaidCost]                MONEY           NULL,
    [AdjustedReceivedCost]         MONEY           NULL,
    [DeletedReason]                INT             NULL,
    [DSDOrder]                     BIT             NULL,
    [PartialShipment]              BIT             NULL,
    [ReturnOrderHeader_ID]         INT             NULL,
    [InvoiceTotalCost]             MONEY           NULL,
    [InvoiceCost]                  MONEY           NULL,
    [InvoiceFreight]               MONEY           NULL,
    [TotalRefused]                 MONEY           NULL,
    [CopyDeletedPOBy]              INT             NULL,
    [CopyDeletedPODate]            DATETIME        NULL,
    [CopiedOrderHeader_ID]         INT             NULL,
    CONSTRAINT [PK_DeletedOrder] PRIMARY KEY CLUSTERED ([DeletedOrder_ID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([OrderExternalSourceID]) REFERENCES [dbo].[OrderExternalSource] ([ID]),
    CONSTRAINT [FK__DeletedOrder__Creat] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__DeletedOrder__Purch] FOREIGN KEY ([PurchaseLocation_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK__DeletedOrder__Recei] FOREIGN KEY ([ReceiveLocation_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK__DeletedOrder__RecvL] FOREIGN KEY ([RecvLogUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__DeletedOrder__User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__DeletedOrder__Vendo] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID]),
    CONSTRAINT [FK_Currency_DeletedOrder] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[Currency] ([CurrencyID]),
    CONSTRAINT [FK_DeletedOrder_ClosedBy] FOREIGN KEY ([ClosedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_DeletedOrder_MatchingUser_ID] FOREIGN KEY ([MatchingUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_DeletedOrder_MatchingValidationCode] FOREIGN KEY ([MatchingValidationCode]) REFERENCES [dbo].[ValidationCode] ([ValidationCode]),
    CONSTRAINT [FK_DeletedOrder_ReasonCodeDetail] FOREIGN KEY ([ReasonCodeDetailID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_DeletedOrder_SubTeam] FOREIGN KEY ([Transfer_SubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_DeletedOrder_SubTeam1] FOREIGN KEY ([Transfer_To_SubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam] FOREIGN KEY ([SupplyTransferToSubTeam]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_DeletedOrder_Users] FOREIGN KEY ([Accounting_In_UserID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_DeletedOrder_Users1] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_DeletedOrderItem_RefuseReceivingReasonID] FOREIGN KEY ([RefuseReceivingReasonID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
);


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Creat];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Purch];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Recei];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__RecvL];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__User];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Vendo];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_ClosedBy];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_SubTeam];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_SubTeam1];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_Users];


GO
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_Users1];


GO
CREATE NONCLUSTERED INDEX [idxApprovedCloseDate]
    ON [dbo].[DeletedOrder]([CloseDate] ASC, [ApprovedDate] ASC)
    INCLUDE([OrderHeader_ID], [InvoiceNumber], [Vendor_ID], [PurchaseLocation_ID], [OrderDate], [Transfer_To_SubTeam], [InvoiceDate], [eInvoice_Id], [ResolutionCodeID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxVendorOrder_ID]
    ON [dbo].[DeletedOrder]([InvoiceNumber] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID]
    ON [dbo].[DeletedOrder]([Vendor_ID] ASC, [OrderHeader_ID] ASC)
    INCLUDE([InvoiceNumber]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_OH_OHID_ExternalOrderId]
    ON [dbo].[DeletedOrder]([OrderHeader_ID] ASC, [OrderExternalSourceOrderID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [OH_ExtSrcID_ExtSrcOrdID]
    ON [dbo].[DeletedOrder]([OrderExternalSourceID] ASC, [OrderExternalSourceOrderID] ASC) WITH (FILLFACTOR = 90);


GO
GRANT SELECT
    ON OBJECT::[dbo].[DeletedOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DeletedOrder] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DeletedOrder] TO [IRMAReportsRole]
    AS [dbo];

