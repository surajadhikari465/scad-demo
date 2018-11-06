CREATE TABLE [dbo].[Item_Temp] (
    [item_key]             INT            IDENTITY (1, 1) NOT NULL,
    [item_description]     VARCHAR (60)   NOT NULL,
    [subteam_no]           INT            NOT NULL,
    [package_desc2]        DECIMAL (9, 4) NULL,
    [package_unit_id]      INT            NULL,
    [category_id]          INT            NULL,
    [deleted_item]         BIT            NOT NULL,
    [discontinue_item]     BIT            NOT NULL,
    [pos_description]      VARCHAR (26)   NOT NULL,
    [price_required]       BIT            NOT NULL,
    [item_type_id]         INT            NOT NULL,
    [not_availablenote]    BIT            NULL,
    [insert_date]          DATETIME       NOT NULL,
    [category_name]        VARCHAR (35)   NULL,
    [identifier]           VARCHAR (13)   NOT NULL,
    [price]                SMALLMONEY     NOT NULL,
    [sale_end_date]        SMALLDATETIME  NULL,
    [avgcost]              SMALLMONEY     NOT NULL,
    [vendor_key]           VARCHAR (10)   NULL,
    [unitcost]             SMALLMONEY     NOT NULL,
    [package_desc1]        DECIMAL (9, 4) NULL,
    [team_no]              INT            NULL,
    [team_name]            VARCHAR (100)  NULL,
    [subteam_name]         VARCHAR (100)  NULL,
    [dept_no]              INT            NULL,
    [target_margin]        DECIMAL (9, 4) NULL,
    [default_identifier]   TINYINT        CONSTRAINT [DF_Item_Temp_default_identifier] DEFAULT ((1)) NOT NULL,
    [business_unit]        INT            NULL,
    [isPrimary]            BIT            NULL,
    [onPromotion]          BIT            NULL,
    [CheckDigit]           VARCHAR (1)    NULL,
    [IdentifierType]       VARCHAR (1)    NULL,
    [TaxClassId]           INT            NULL,
    [StopSale]             BIT            NULL,
    [LabelTypeId]          INT            NULL,
    [CostedByWeight]       INT            NULL,
    [Cost_Unit_id]         INT            NULL,
    [freight_unit_id]      INT            NULL,
    [vendor_unit_id]       INT            NULL,
    [distribution_unit_id] INT            NULL,
    [retail_unit_id]       INT            NULL,
    [vendor_item_id]       VARCHAR (20)   NULL,
    [saleprice]            SMALLMONEY     NULL,
    [posprice]             SMALLMONEY     NULL,
    [possaleprice]         SMALLMONEY     NULL,
    [master_upc]           VARCHAR (13)   NULL,
    [sale_start_date]      SMALLDATETIME  NULL,
    [cmp]                  BIT            CONSTRAINT [DF_Item_Temp_cmp] DEFAULT ((0)) NULL,
    [food_stamps]          BIT            NULL,
    [PosTare]              INT            NULL,
    [LinkCode]             VARCHAR (13)   NULL,
    [GrillPrint]           BIT            NULL,
    [AgeCode]              INT            NULL,
    [VisualVerify]         BIT            NULL,
    [SrCitizenDiscount]    BIT            NULL,
    [QtyProhibit]          BIT            NULL,
    [GroupList]            INT            NULL,
    [Item_Key_Temp]        INT            NULL,
    [Item_Key_Temp2]       INT            NULL,
    [PricingMethod_ID]     INT            NULL,
    [ShelfLife_Id]         INT            NULL,
    [ShelfLife_Length]     INT            NULL,
    [Multiple]             TINYINT        NULL,
    [MSRPPrice]            SMALLMONEY     NULL,
    [MSRPMultiple]         TINYINT        NULL,
    [Sale_Multiple]        TINYINT        NULL,
    [Sale_Earned_Disc1]    TINYINT        NULL,
    [Sale_Earned_Disc2]    TINYINT        NULL,
    [Sale_Earned_Disc3]    TINYINT        NULL,
    [scaledesc1]           VARCHAR (64)   NULL,
    [ingredients]          VARCHAR (2000) NULL,
    [brandid]              INT            NULL,
    [discountable]         BIT            NULL,
    [natclassid]           INT            NULL,
    [ScaleDesc2]           VARCHAR (64)   NULL,
    [ScaleForcedTare]      VARCHAR (1)    NULL,
    [ScaleTare]            INT            NULL,
    [ScaleDesc3]           VARCHAR (64)   NULL,
    [ScaleDesc4]           VARCHAR (64)   NULL,
    [restricted_hours]     BIT            NULL,
    [edlp]                 BIT            NULL,
    [ibm_discount]         BIT            NULL,
    [isAuthorized]         BIT            NULL,
    [national_identifier]  BIT            NULL,
    [poslinkcode]          VARCHAR (10)   NULL,
    [scale_identifier]     INT            NULL,
    [item_package_desc1]   INT            NULL,
    [by_count]             INT            NULL,
    [fixed_weight]         INT            NULL,
    [pzone]                INT            NULL,
    [scale_uom]            INT            NULL,
    [PriceChgTypeId]       INT            DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Item_Temp] PRIMARY KEY CLUSTERED ([item_key] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item_Temp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item_Temp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item_Temp] TO [IRMAReportsRole]
    AS [dbo];

