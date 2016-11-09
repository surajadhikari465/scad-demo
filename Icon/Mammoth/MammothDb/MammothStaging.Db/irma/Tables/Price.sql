CREATE TABLE [irma].[Price] (
    [Region]               NCHAR (2)     NOT NULL,
    [Item_Key]             INT           NOT NULL,
    [Store_No]             INT           NOT NULL,
    [Multiple]             TINYINT       NOT NULL,
    [Price]                SMALLMONEY    NOT NULL,
    [MSRPPrice]            SMALLMONEY    NOT NULL,
    [Sale_Multiple]        TINYINT       NOT NULL,
    [Sale_Price]           SMALLMONEY    NOT NULL,
    [Sale_Start_Date]      SMALLDATETIME NULL,
    [Sale_End_Date]        SMALLDATETIME NULL,
    [Restricted_Hours]     BIT           NOT NULL,
    [Discountable]         BIT           NOT NULL,
    [IBM_Discount]         BIT           NOT NULL,
    [POSPrice]             SMALLMONEY    NULL,
    [POSSale_Price]        SMALLMONEY    NULL,
    [NotAuthorizedForSale] BIT           NULL,
    [CompFlag]             BIT           NULL,
    [SrCitizenDiscount]    BIT           NULL,
    [PriceChgTypeId]       TINYINT       NULL,
    [Age_Restrict]         BIT           NULL,
    [AgeCode]              INT           NULL,
    [LinkedItem]           INT           NULL,
    [ElectronicShelfTag]   BIT           NULL,
    [LocalItem]            BIT           NULL,
    CONSTRAINT [PK__Price] PRIMARY KEY CLUSTERED ([Region] ASC, [Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 100)
);



