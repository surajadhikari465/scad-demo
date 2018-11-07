CREATE TABLE [dbo].[VendorHistory] (
    [Vendor_ID]                    INT           NOT NULL,
    [Vendor_Key]                   VARCHAR (10)  NULL,
    [CompanyName]                  VARCHAR (50)  NOT NULL,
    [Address_Line_1]               VARCHAR (50)  NULL,
    [Address_Line_2]               VARCHAR (50)  NULL,
    [City]                         VARCHAR (30)  NULL,
    [State]                        VARCHAR (2)   NULL,
    [Zip_Code]                     VARCHAR (10)  NULL,
    [Country]                      VARCHAR (10)  NULL,
    [Phone]                        VARCHAR (20)  NULL,
    [Fax]                          VARCHAR (20)  NULL,
    [PayTo_CompanyName]            VARCHAR (50)  NULL,
    [PayTo_Attention]              VARCHAR (50)  NULL,
    [PayTo_Address_Line_1]         VARCHAR (50)  NULL,
    [PayTo_Address_Line_2]         VARCHAR (50)  NULL,
    [PayTo_City]                   VARCHAR (30)  NULL,
    [PayTo_State]                  VARCHAR (2)   NULL,
    [PayTo_Zip_Code]               VARCHAR (10)  NULL,
    [PayTo_Country]                VARCHAR (10)  NULL,
    [PayTo_Phone]                  VARCHAR (20)  NULL,
    [PayTo_Fax]                    VARCHAR (20)  NULL,
    [Comment]                      VARCHAR (255) NULL,
    [Customer]                     BIT           NOT NULL,
    [InternalCustomer]             BIT           NOT NULL,
    [ActiveVendor]                 BIT           NOT NULL,
    [Store_no]                     INT           NULL,
    [Order_By_Distribution]        BIT           NOT NULL,
    [Electronic_Transfer]          BIT           NOT NULL,
    [User_ID]                      INT           NULL,
    [Phone_Ext]                    VARCHAR (5)   NULL,
    [PayTo_Phone_Ext]              VARCHAR (5)   NULL,
    [PS_Vendor_ID]                 VARCHAR (10)  NULL,
    [PS_Location_Code]             VARCHAR (10)  NULL,
    [PS_Address_Sequence]          VARCHAR (2)   NULL,
    [WFM]                          BIT           NOT NULL,
    [FTP_Addr]                     VARCHAR (255) NULL,
    [FTP_Path]                     VARCHAR (255) NULL,
    [FTP_User]                     VARCHAR (255) NULL,
    [FTP_Password]                 VARCHAR (255) NULL,
    [Non_Product_Vendor]           TINYINT       NOT NULL,
    [Default_GLNumber]             VARCHAR (10)  NULL,
    [Email]                        VARCHAR (50)  NULL,
    [EFT]                          BIT           NOT NULL,
    [InStoreManufacturedProducts]  BIT           NOT NULL,
    [EXEWarehouseVendSent]         BIT           NOT NULL,
    [EXEWarehouseCustSent]         BIT           NOT NULL,
    [County]                       VARCHAR (20)  NULL,
    [PayTo_County]                 VARCHAR (20)  NULL,
    [AddVendor]                    BIT           NOT NULL,
    [Po_Note]                      VARCHAR (150) NULL,
    [Receiving_Authorization_Note] VARCHAR (150) NULL,
    [Other_Name]                   VARCHAR (35)  NULL,
    [PS_Export_Vendor_ID]          VARCHAR (10)  NULL,
    [File_Type]                    VARCHAR (15)  NULL,
    [CaseDistHandlingCharge]       SMALLMONEY    NULL,
    [EInvoicing]                   BIT           NOT NULL,
    [POTransmissionTypeID]         INT           NULL,
    [CurrencyID]                   INT           NULL,
    [LeadTimeDays]                 INT           NOT NULL,
    [LeadTimeDayOfWeek]            TINYINT       NULL,
    [ChangedByUserID]              INT           NOT NULL,
    [ChangeDate]                   DATETIME      CONSTRAINT [DF_VendorHistory_ChangeDate] DEFAULT (getdate()) NOT NULL,
    [AccountingContactEmail]       VARCHAR (50)  NULL,
    [PaymentTermID]                INT           NULL,
    [EinvoiceRequired]             BIT           DEFAULT ((0)) NOT NULL,
    [AllowReceiveAll]              BIT           DEFAULT ((0)) NOT NULL,
    [ShortpayProhibited]           BIT           DEFAULT ((0)) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorHistory] TO [IRMAReportsRole]
    AS [dbo];

