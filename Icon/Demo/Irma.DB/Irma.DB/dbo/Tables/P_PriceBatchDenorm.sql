CREATE TABLE [dbo].[P_PriceBatchDenorm] (
    [PriceBatchDenormID]    INT            NULL,
    [InsertDate]            DATETIME       NULL,
    [Item_Key]              INT            NULL,
    [Identifier]            VARCHAR (13)   NULL,
    [On_Sale]               BIT            NULL,
    [POS_Description]       VARCHAR (26)   NULL,
    [Item_Description]      VARCHAR (60)   NULL,
    [Package_Unit_Abbr]     VARCHAR (5)    NULL,
    [Price_Change]          TINYINT        NULL,
    [Item_Change]           TINYINT        NULL,
    [IsScaleItem]           TINYINT        NULL,
    [Price]                 MONEY          NULL,
    [Sale_Price]            MONEY          NULL,
    [Sale_End_Date]         SMALLDATETIME  NULL,
    [Sale_Start_Date]       SMALLDATETIME  NULL,
    [Brand_Name]            VARCHAR (25)   NULL,
    [CaseSize]              DECIMAL (9, 4) NULL,
    [Sign_Description]      VARCHAR (60)   NULL,
    [BusinessUnit_ID]       INT            NULL,
    [Organic]               BIT            NULL,
    [ClassID]               INT            NULL,
    [PriceChgTypeDesc]      VARCHAR (3)    NULL,
    [ECommerce]             BIT            NULL,
    [TaxClassID]            INT            NULL,
    [DiscontinueItem]       BIT            NULL,
    [Check_Box_1]           BIT            NULL,
    [Check_Box_2]           BIT            NULL,
    [Check_Box_3]           BIT            NULL,
    [Check_Box_4]           BIT            NULL,
    [Check_Box_5]           BIT            NULL,
    [Check_Box_6]           BIT            NULL,
    [Check_Box_7]           BIT            NULL,
    [Check_Box_8]           BIT            NULL,
    [Check_Box_9]           BIT            NULL,
    [Check_Box_10]          BIT            NULL,
    [Check_Box_11]          BIT            NULL,
    [Check_Box_12]          BIT            NULL,
    [Check_Box_13]          BIT            NULL,
    [Check_Box_14]          BIT            NULL,
    [Check_Box_15]          BIT            NULL,
    [Check_Box_16]          BIT            NULL,
    [Check_Box_17]          BIT            NULL,
    [Check_Box_18]          BIT            NULL,
    [Check_Box_19]          BIT            NULL,
    [Check_Box_20]          BIT            NULL,
    [Text_1]                VARCHAR (50)   NULL,
    [Text_2]                VARCHAR (50)   NULL,
    [Text_3]                VARCHAR (50)   NULL,
    [Text_4]                VARCHAR (50)   NULL,
    [Text_5]                VARCHAR (50)   NULL,
    [Text_6]                VARCHAR (50)   NULL,
    [Text_7]                VARCHAR (50)   NULL,
    [Text_8]                VARCHAR (50)   NULL,
    [Text_9]                VARCHAR (50)   NULL,
    [Text_10]               VARCHAR (50)   NULL,
    [ADB_SUBJECT]           VARCHAR (255)  NULL,
    [ADB_SEQUENCE]          INT            IDENTITY (1, 1) NOT NULL,
    [ADB_SET_SEQUENCE]      INT            NULL,
    [ADB_TIMESTAMP]         DATETIME       DEFAULT (getdate()) NULL,
    [ADB_OPCODE]            INT            DEFAULT ((1)) NOT NULL,
    [ADB_UPDATE_ALL]        INT            NULL,
    [ADB_REF_OBJECT]        VARCHAR (64)   NULL,
    [ADB_L_DELIVERY_STATUS] CHAR (1)       DEFAULT ('N') NULL,
    [ADB_L_CMSEQUENCE]      DECIMAL (28)   NULL,
    [ADB_TRACKINGID]        VARCHAR (40)   NULL,
    [IsDeleted]             BIT            NULL,
    [IsAuthorized]          BIT            NULL,
    [Package_Desc2]         DECIMAL (9, 4) NULL,
    [NatCatID]              INT            NULL,
    [NatFamilyID]           INT            NULL,
    [Brand_ID]              INT            NULL,
    [New_Item]              TINYINT        NULL
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[P_PriceBatchDenorm] TO [WFM\ESB_ClickAndCollect]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[P_PriceBatchDenorm] TO [WFM\ESB_ClickAndCollect]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[P_PriceBatchDenorm] TO [WFM\ESB_ClickAndCollect]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[P_PriceBatchDenorm] TO [WFM\ESB_ClickAndCollect]
    AS [dbo];


GO



GO



GO



GO


