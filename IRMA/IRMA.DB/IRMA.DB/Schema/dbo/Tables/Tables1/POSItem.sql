CREATE TABLE [dbo].[POSItem] (
    [Item_Key]          INT           CONSTRAINT [DF__POSItem__Item_Ke__0A83F502] DEFAULT ((-1)) NOT NULL,
    [Identifier]        VARCHAR (12)  NOT NULL,
    [Store_No]          SMALLINT      NOT NULL,
    [POS_Description]   VARCHAR (26)  NOT NULL,
    [Retail_Sale]       BIT           CONSTRAINT [DF__POSItem__Retail___0C6C3D74] DEFAULT ((0)) NOT NULL,
    [Food_Stamps]       BIT           CONSTRAINT [DF__POSItem__Food_St__0D6061AD] DEFAULT ((0)) NOT NULL,
    [Discountable]      BIT           CONSTRAINT [DF__POSItem__Discoun__0E5485E6] DEFAULT ((0)) NOT NULL,
    [IBM_Discount]      BIT           CONSTRAINT [DF__POSItem__IBM_Dis__0F48AA1F] DEFAULT ((0)) NOT NULL,
    [Tax_Table_A]       BIT           CONSTRAINT [DF__POSItem__Tax_Tab__103CCE58] DEFAULT ((0)) NOT NULL,
    [Tax_Table_B]       BIT           CONSTRAINT [DF__POSItem__Tax_Tab__1130F291] DEFAULT ((0)) NOT NULL,
    [Tax_Table_C]       BIT           CONSTRAINT [DF__POSItem__Tax_Tab__122516CA] DEFAULT ((0)) NOT NULL,
    [Tax_Table_D]       BIT           CONSTRAINT [DF__POSItem__Tax_Tab__13193B03] DEFAULT ((0)) NOT NULL,
    [Price_Required]    BIT           CONSTRAINT [DF__POSItem__Price_R__140D5F3C] DEFAULT ((0)) NOT NULL,
    [Quantity_Required] BIT           CONSTRAINT [DF__POSItem__Quantit__15018375] DEFAULT ((0)) NOT NULL,
    [Restricted_Hours]  BIT           CONSTRAINT [DF__POSItem__Restric__15F5A7AE] DEFAULT ((0)) NOT NULL,
    [ItemType_ID]       INT           CONSTRAINT [DF__POSItem__ItemTyp__16E9CBE7] DEFAULT ((0)) NOT NULL,
    [SubTeam_No]        INT           CONSTRAINT [DF__POSItem__SubTeam__17DDF020] DEFAULT ((0)) NOT NULL,
    [Case_Price]        SMALLMONEY    CONSTRAINT [DF__POSItem__Case_Pr__18D21459] DEFAULT ((0)) NOT NULL,
    [PricingMethod_ID]  INT           CONSTRAINT [DF__POSItem__Pricing__19C63892] DEFAULT ((0)) NOT NULL,
    [Unit_Price]        SMALLMONEY    CONSTRAINT [DF__POSItem__Unit_Pr__1ABA5CCB] DEFAULT ((0)) NOT NULL,
    [Deal_Quantity]     TINYINT       CONSTRAINT [DF__POSItem__Deal_Qu__1BAE8104] DEFAULT ((1)) NOT NULL,
    [Deal_Price]        SMALLMONEY    CONSTRAINT [DF__POSItem__Deal_Pr__1CA2A53D] DEFAULT ((0)) NOT NULL,
    [LOAD_Date]         SMALLDATETIME NOT NULL,
    [SoldByWeight]      BIT           DEFAULT ((0)) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [idxPOSItemDesc]
    ON [dbo].[POSItem]([POS_Description] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxPOSItemIdentifier]
    ON [dbo].[POSItem]([Identifier] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxPOSItemSubTeamNo]
    ON [dbo].[POSItem]([SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxPOSItemKey]
    ON [dbo].[POSItem]([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[POSItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[POSItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[POSItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSItem] TO [IRMAReportsRole]
    AS [dbo];

