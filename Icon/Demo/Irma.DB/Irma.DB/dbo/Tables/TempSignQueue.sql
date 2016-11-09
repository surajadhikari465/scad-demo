CREATE TABLE [dbo].[TempSignQueue] (
    [SignQueue_ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Item_Key]          INT            NOT NULL,
    [Store_No]          INT            NOT NULL,
    [Sign_Description]  VARCHAR (60)   NOT NULL,
    [Ingredients]       VARCHAR (250)  NULL,
    [Identifier]        VARCHAR (13)   NOT NULL,
    [Mix_Match]         TINYINT        NOT NULL,
    [Sold_By_Weight]    BIT            NOT NULL,
    [Multiple]          TINYINT        NOT NULL,
    [Price]             SMALLMONEY     NOT NULL,
    [MSRPMultiple]      TINYINT        NOT NULL,
    [MSRPPrice]         SMALLMONEY     NOT NULL,
    [Case_Price]        SMALLMONEY     NOT NULL,
    [Sale_Multiple]     TINYINT        NOT NULL,
    [Sale_Price]        SMALLMONEY     NOT NULL,
    [Sale_Start_Date]   SMALLDATETIME  NULL,
    [Sale_End_Date]     SMALLDATETIME  NULL,
    [Sale_Mix_Match]    SMALLINT       NOT NULL,
    [Sale_Earned_Disc1] TINYINT        NOT NULL,
    [Sale_Earned_Disc2] TINYINT        NOT NULL,
    [Sale_Earned_Disc3] TINYINT        NOT NULL,
    [PricingMethod_ID]  INT            NULL,
    [SubTeam_No]        INT            NULL,
    [Insert_Type]       TINYINT        NOT NULL,
    [Origin_Name]       VARCHAR (25)   NULL,
    [Brand_Name]        VARCHAR (25)   NULL,
    [Retail_Unit_Abbr]  VARCHAR (5)    NULL,
    [Retail_Unit_Full]  VARCHAR (25)   NULL,
    [Package_Unit]      VARCHAR (5)    NULL,
    [Package_Desc1]     DECIMAL (9, 4) NULL,
    [Package_Desc2]     DECIMAL (9, 4) NULL,
    [New_Item]          TINYINT        NOT NULL,
    [Sign_Printed]      TINYINT        NOT NULL,
    [Price_Post_Type]   TINYINT        NOT NULL,
    [Promo_Post_Type]   TINYINT        NOT NULL,
    [Price_Change]      TINYINT        NOT NULL,
    [Item_Change]       TINYINT        NOT NULL,
    [Promo_Change]      TINYINT        NOT NULL,
    [Organic]           BIT            NOT NULL,
    [Insert_Date]       SMALLDATETIME  NOT NULL,
    [Insert_User_ID]    INT            NULL,
    [Sale_EDLP]         BIT            NULL,
    [Vendor_Id]         INT            NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TempSignQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempSignQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempSignQueue] TO [IRMAReportsRole]
    AS [dbo];

