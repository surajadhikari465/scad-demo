﻿CREATE TABLE [dbo].[tempUser] (
    [User_ID]                    INT          IDENTITY (1, 1) NOT NULL,
    [UserName]                   VARCHAR (25) NOT NULL,
    [FullName]                   VARCHAR (50) NULL,
    [Printer]                    VARCHAR (50) NULL,
    [CoverPage]                  VARCHAR (30) NULL,
    [EMail]                      VARCHAR (50) NULL,
    [Pager_Email]                VARCHAR (50) NULL,
    [Fax_Number]                 VARCHAR (15) NULL,
    [Assistant_ID]               INT          NULL,
    [AccountEnabled]             BIT          NOT NULL,
    [SuperUser]                  BIT          NOT NULL,
    [SPE]                        BIT          NOT NULL,
    [PO_Accountant]              BIT          NOT NULL,
    [Accountant]                 BIT          NOT NULL,
    [Distributor]                BIT          NOT NULL,
    [Credit_Buyer]               BIT          NOT NULL,
    [Buyer]                      BIT          NOT NULL,
    [Frequent_Shopper]           BIT          NOT NULL,
    [Coordinator]                BIT          NOT NULL,
    [Item_Administrator]         BIT          NOT NULL,
    [Availability_Administrator] BIT          NOT NULL,
    [Vendor_Administrator]       BIT          NOT NULL,
    [Lock_Administrator]         BIT          NOT NULL,
    [Search_1]                   BIT          NOT NULL,
    [Search_2]                   BIT          NOT NULL,
    [Search_3]                   BIT          NOT NULL,
    [Search_4]                   BIT          NOT NULL,
    [Search_5]                   BIT          NOT NULL,
    [Search_6]                   BIT          NOT NULL,
    [Search_7]                   BIT          NOT NULL,
    [Search_8]                   TINYINT      NOT NULL,
    [Search_9]                   TINYINT      NOT NULL,
    [Search_10]                  TINYINT      NOT NULL,
    [Telxon_Store_Limit]         INT          NULL,
    [Telxon_Enabled]             BIT          NOT NULL,
    [Telxon_Waste]               BIT          NOT NULL,
    [Telxon_Cycle_Count]         BIT          NOT NULL,
    [Telxon_Distribution]        BIT          NOT NULL,
    [Telxon_Transfers]           BIT          NOT NULL,
    [Telxon_Price_Check]         BIT          NOT NULL,
    [Telxon_Superuser]           BIT          NOT NULL,
    [Telxon_Orders]              BIT          NOT NULL,
    [Telxon_Receiving]           BIT          NOT NULL,
    [Telxon_Reserved_2]          BIT          NOT NULL,
    [Telxon_Reserved_3]          BIT          NOT NULL,
    [Phone_Number]               VARCHAR (25) NULL,
    [Title]                      VARCHAR (60) NULL,
    [Support_Password]           VARCHAR (20) NULL,
    [Support_Administrator]      BIT          NOT NULL,
    [Support_Worker]             BIT          NOT NULL,
    [Maintenance_Password]       VARCHAR (20) NULL,
    [Maintenance_Administrator]  BIT          NOT NULL,
    [Maintenance_Worker]         BIT          NOT NULL,
    [RecvLog_Store_Limit]        INT          NULL,
    [Delete_Access]              BIT          NOT NULL,
    [Warehouse]                  BIT          NOT NULL,
    [Non_PO_Administrator]       BIT          NOT NULL,
    [PriceBatchProcessor]        BIT          NOT NULL,
    [Inventory_Administrator]    BIT          NOT NULL,
    [BatchBuildOnly]             BIT          NOT NULL
);

