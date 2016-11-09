﻿CREATE TABLE [irma].[ItemOverride] (
    [Region]                        NCHAR (2)      NOT NULL,
    [Item_Key]                      INT            NOT NULL,
    [StoreJurisdictionID]           INT            NOT NULL,
    [Item_Description]              VARCHAR (60)   NOT NULL,
    [Sign_Description]              VARCHAR (60)   NOT NULL,
    [Package_Desc1]                 DECIMAL (9, 4) NOT NULL,
    [Package_Desc2]                 DECIMAL (9, 4) NOT NULL,
    [Package_Unit_ID]               INT            NOT NULL,
    [Retail_Unit_ID]                INT            NOT NULL,
    [Vendor_Unit_ID]                INT            NOT NULL,
    [Distribution_Unit_ID]          INT            NOT NULL,
    [POS_Description]               VARCHAR (26)   NOT NULL,
    [Food_Stamps]                   BIT            NOT NULL,
    [Price_Required]                BIT            NOT NULL,
    [Quantity_Required]             BIT            NOT NULL,
    [Manufacturing_Unit_ID]         INT            NULL,
    [QtyProhibit]                   BIT            NULL,
    [GroupList]                     INT            NULL,
    [Case_Discount]                 BIT            NULL,
    [Coupon_Multiplier]             BIT            NULL,
    [Misc_Transaction_Sale]         SMALLINT       NULL,
    [Misc_Transaction_Refund]       SMALLINT       NULL,
    [Ice_Tare]                      INT            NULL,
    [Brand_ID]                      INT            NULL,
    [Origin_ID]                     INT            NULL,
    [CountryProc_ID]                INT            NULL,
    [SustainabilityRankingRequired] BIT            NULL,
    [SustainabilityRankingID]       INT            NULL,
    [LabelType_ID]                  INT            NULL,
    [CostedByWeight]                BIT            NOT NULL,
    [Average_Unit_Weight]           DECIMAL (9, 4) NULL,
    [Ingredient]                    BIT            NOT NULL,
    [Recall_Flag]                   BIT            NULL,
    [LockAuth]                      BIT            NULL,
    [Not_Available]                 BIT            NOT NULL,
    [Not_AvailableNote]             VARCHAR (255)  NULL,
    [FSA_Eligible]                  BIT            NOT NULL,
    [Product_Code]                  VARCHAR (15)   NULL,
    [Unit_Price_Category]           INT            NULL,
    [LastModifiedUser_ID]           INT            NULL
);

