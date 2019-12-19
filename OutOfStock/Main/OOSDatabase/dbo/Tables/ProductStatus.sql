CREATE TABLE [dbo].[ProductStatus] (
    [UPC]            VARCHAR (25)   NOT NULL,
    [Region]         VARCHAR (5)    NOT NULL,
    [VendorKey]      VARCHAR (10)   NULL,
    [Vin]            VARCHAR (25)   NULL,
    [Reason]         VARCHAR (500)  NULL,
    [StartDate]      DATETIME       CONSTRAINT [DF_ProductStatus_StartDate] DEFAULT (getdate()) NOT NULL,
    [ProductStatus]  VARCHAR (2000) NULL,
    [ExpirationDate] DATETIME       NULL,
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ProductStatus_1] PRIMARY KEY CLUSTERED ([UPC] ASC, [Region] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductStatus_Id]
    ON [dbo].[ProductStatus]([id] ASC);

