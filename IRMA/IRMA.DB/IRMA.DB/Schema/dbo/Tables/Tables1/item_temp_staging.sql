CREATE TABLE [dbo].[item_temp_staging] (
    [item_status]           CHAR (1)       NULL,
    [master_upc]            CHAR (13)      NULL,
    [Chain_Code]            CHAR (13)      NULL,
    [Item_Description]      CHAR (30)      NULL,
    [CIX_upcno]             CHAR (13)      NULL,
    [SubTeam_No]            CHAR (4)       NULL,
    [Sales_Account]         INT            NULL,
    [Package_Desc2]         NUMERIC (7, 3) NULL,
    [Package_Unit_ID]       INT            NOT NULL,
    [Min_Temperature]       INT            NOT NULL,
    [Max_Temperature]       INT            NOT NULL,
    [Units_Per_Pallet]      INT            NOT NULL,
    [Average_Unit_Weight]   INT            NOT NULL,
    [Tie]                   INT            NOT NULL,
    [High]                  INT            NOT NULL,
    [Yield]                 INT            NOT NULL,
    [Brand_ID]              INT            NOT NULL,
    [Category_ID]           INT            NULL,
    [Origin_ID]             INT            NULL,
    [Retail_Unit_ID]        INT            NULL,
    [Vendor_Unit_ID]        INT            NOT NULL,
    [Distribution_Unit_ID]  INT            NOT NULL,
    [Cost_Unit_ID]          INT            NOT NULL,
    [Freight_Unit_ID]       INT            NOT NULL,
    [Deleted_Item]          INT            NOT NULL,
    [Discontinue_Item]      INT            NOT NULL,
    [WFM_Item]              INT            NOT NULL,
    [Not_Available]         INT            NOT NULL,
    [Pre_Order]             INT            NOT NULL,
    [Remove_Item]           INT            NOT NULL,
    [NoDistMarkup]          INT            NOT NULL,
    [Organic]               INT            NOT NULL,
    [Refrigerated]          INT            NOT NULL,
    [Keep_Frozen]           INT            NOT NULL,
    [Shipper_Item]          INT            NOT NULL,
    [Full_Pallet_Only]      INT            NOT NULL,
    [User_ID]               INT            NOT NULL,
    [POS_Description]       CHAR (25)      NULL,
    [Retail_Sale]           INT            NOT NULL,
    [Food_Stamps]           INT            NOT NULL,
    [Discountable]          INT            NOT NULL,
    [Price_Required]        INT            NOT NULL,
    [Quantity_Required]     INT            NOT NULL,
    [ItemType_ID]           INT            NOT NULL,
    [HFM_Item]              INT            NOT NULL,
    [ScaleDesc1]            CHAR (32)      NULL,
    [ScaleDesc2]            CHAR (32)      NULL,
    [Not_AvailableNote]     INT            NOT NULL,
    [CountryProc_ID]        INT            NULL,
    [Insert_Date]           DATETIME       NULL,
    [Manufacturing_Unit_ID] INT            NOT NULL,
    [EXEDistributed]        INT            NOT NULL,
    [ClassID]               INT            NULL,
    [User_ID_Date]          INT            NULL,
    [DistSubTeam_No]        INT            NULL,
    [CostedByWeight]        INT            NOT NULL,
    [TaxClassID]            INT            NULL,
    [LabelType_ID]          INT            NULL,
    [QtyProhibit]           INT            NOT NULL,
    [GroupList]             SMALLINT       NULL,
    [ShelfLife_Id]          INT            NULL,
    [ShelfLife_Length]      SMALLINT       NOT NULL,
    [ingredients]           CHAR (972)     NULL,
    [scaleForcedtare]       CHAR (1)       NULL,
    [scaletare]             SMALLINT       NULL,
    [ScaleDesc3]            CHAR (32)      NULL,
    [ScaleDesc4]            CHAR (32)      NULL,
    [scale_uom]             CHAR (2)       NULL
);


GO
CREATE NONCLUSTERED INDEX [its_st]
    ON [dbo].[item_temp_staging]([SubTeam_No] ASC);


GO
CREATE NONCLUSTERED INDEX [its_ci]
    ON [dbo].[item_temp_staging]([Category_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [its_sd]
    ON [dbo].[item_temp_staging]([CIX_upcno] ASC);

