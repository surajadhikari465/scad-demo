CREATE TABLE [dbo].[JDA_VendorSync] (
    [JDA_VendorSync_ID]            INT           IDENTITY (1, 1) NOT NULL,
    [ActionCode]                   CHAR (1)      NOT NULL,
    [ApplyDate]                    DATETIME      NOT NULL,
    [Vendor_ID]                    INT           NOT NULL,
    [CompanyName]                  VARCHAR (50)  NULL,
    [Address_Line_1]               VARCHAR (50)  NULL,
    [Address_Line_2]               VARCHAR (50)  NULL,
    [City]                         VARCHAR (30)  NULL,
    [State]                        VARCHAR (2)   NULL,
    [Zip_Code]                     VARCHAR (10)  NULL,
    [Country]                      VARCHAR (10)  NULL,
    [Phone]                        VARCHAR (20)  NULL,
    [Fax]                          VARCHAR (20)  NULL,
    [PS_Vendor_ID]                 VARCHAR (10)  NULL,
    [Non_Product_Vendor]           TINYINT       NULL,
    [Po_Note]                      VARCHAR (150) NULL,
    [Receiving_Authorization_Note] VARCHAR (150) NULL,
    [Other_Name]                   VARCHAR (35)  NULL,
    [SyncState]                    TINYINT       DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JDA_VendorSync] PRIMARY KEY CLUSTERED ([JDA_VendorSync_ID] ASC)
);

