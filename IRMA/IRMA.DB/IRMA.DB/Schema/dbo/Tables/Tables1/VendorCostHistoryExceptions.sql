CREATE TABLE [dbo].[VendorCostHistoryExceptions] (
    [VCAI_ExID]        INT            IDENTITY (1, 1) NOT NULL,
    [Vendor_ID]        VARCHAR (10)   NOT NULL,
    [Item_ID]          VARCHAR (15)   NULL,
    [Sku_Regional]     BIGINT         NOT NULL,
    [UPC]              BIGINT         NOT NULL,
    [Item_Description] VARCHAR (50)   NULL,
    [MSRP]             DECIMAL (9, 2) NULL,
    [OrigCase_Size]    INT            NULL,
    [UserCase_Size]    INT            NULL,
    [Case_Size]        INT            NOT NULL,
    [OrigUnit_Price]   DECIMAL (9, 3) NULL,
    [UserUnit_Price]   DECIMAL (9, 3) NULL,
    [Case_Price]       DECIMAL (9, 3) NOT NULL,
    [Status_flag]      VARCHAR (2)    NOT NULL,
    [UserStart_Date]   DATETIME       NULL,
    [Start_Date]       DATETIME       NOT NULL,
    [UserEnd_Date]     DATETIME       NULL,
    [End_Date]         DATETIME       NOT NULL,
    [Store_No]         INT            NULL,
    [SubTeam_No]       INT            NOT NULL,
    [ExStatus]         SMALLINT       NOT NULL,
    [ExRuleID]         INT            NULL,
    [ExDescription]    VARCHAR (50)   NULL,
    [ExSeverity]       TINYINT        NULL,
    [User_ID]          INT            NULL,
    [VCH_ID]           INT            NULL,
    [LastModified]     SMALLDATETIME  NULL,
    [InsertDate]       SMALLDATETIME  CONSTRAINT [DF_VendorCostHistoryExceptions_InsertDate] DEFAULT (getdate()) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorCostHistoryExceptions] TO [IRMAAVCIRole]
    AS [dbo];

