CREATE TABLE [dbo].[ItemRequest] (
    [ItemRequest_ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [Identifier]                     CHAR (13)      NOT NULL,
    [ItemStatus_ID]                  SMALLINT       NOT NULL,
    [ItemType_ID]                    SMALLINT       NOT NULL,
    [ItemTemplate]                   BIT            NULL,
    [User_ID]                        INT            NULL,
    [User_Store]                     INT            NULL,
    [UserAccessLevel_ID]             SMALLINT       NULL,
    [VendorRequest_ID]               INT            NULL,
    [Item_Description]               VARCHAR (60)   NOT NULL,
    [POS_Description]                VARCHAR (26)   NOT NULL,
    [ItemUnit]                       INT            NULL,
    [ItemSize]                       DECIMAL (9, 4) NULL,
    [PackSize]                       DECIMAL (9, 4) NULL,
    [VendorNumber]                   CHAR (15)      NULL,
    [SubTeam_No]                     INT            NOT NULL,
    [Price]                          SMALLMONEY     NOT NULL,
    [PriceMultiple]                  TINYINT        NULL,
    [CaseCost]                       SMALLMONEY     NOT NULL,
    [CaseSize]                       SMALLINT       NOT NULL,
    [Warehouse]                      CHAR (12)      NOT NULL,
    [Brand_ID]                       INT            NULL,
    [BrandName]                      VARCHAR (100)  NOT NULL,
    [Category_ID]                    INT            NOT NULL,
    [TaxClass_ID]                    INT            NULL,
    [Insert_Date]                    DATETIME       DEFAULT (getdate()) NULL,
    [ClassID]                        INT            NOT NULL,
    [AgeCode]                        INT            NULL,
    [CRV]                            VARCHAR (15)   NULL,
    [IRMA_Add_Date]                  DATETIME       DEFAULT (getdate()) NULL,
    [Ready_To_Apply]                 BIT            NULL,
    [FoodStamp]                      BIT            NULL,
    [HasIngredients]                 BIT            NULL,
    [Promotional]                    BIT            NULL,
    [CostEnd]                        DATETIME       NULL,
    [CostStart]                      DATETIME       NULL,
    [CostUnit]                       INT            NULL,
    [VendorFreightUnit]              INT            NULL,
    [MSRPPrice]                      SMALLMONEY     NULL,
    [MSRPMultiple]                   TINYINT        NULL,
    [POSLinkCode]                    VARCHAR (50)   NULL,
    [LineDiscount]                   BIT            NULL,
    [CommodityCode]                  VARCHAR (20)   NULL,
    [DiscountTerms]                  VARCHAR (1)    NULL,
    [GoLocal]                        VARCHAR (20)   NULL,
    [Misc]                           VARCHAR (100)  NULL,
    [ESRSCKI]                        VARCHAR (100)  NULL,
    [CostedByWeight]                 BIT            NULL,
    [CountryOfProc]                  INT            NULL,
    [DistributionSubteam]            INT            NULL,
    [DistributionUnits]              INT            NULL,
    [IdentifierType]                 CHAR (1)       NULL,
    [KeepFrozen]                     BIT            NULL,
    [ShelfLabelType]                 INT            NULL,
    [ManufacturingUnits]             INT            NULL,
    [Organic]                        BIT            NULL,
    [Origin]                         INT            NULL,
    [POSTare]                        INT            NULL,
    [PriceRequired]                  BIT            NULL,
    [QuantityProhibit]               BIT            NULL,
    [QuantityRequired]               BIT            NULL,
    [Refrigerated]                   BIT            NULL,
    [Restricted]                     BIT            NULL,
    [RetailUnits]                    INT            NULL,
    [NotAvailable]                   BIT            NULL,
    [NotAvailableNote]               VARCHAR (255)  NULL,
    [UnitFreight]                    SMALLMONEY     NULL,
    [Allowances]                     SMALLMONEY     NULL,
    [Discounts]                      SMALLMONEY     NULL,
    [AllowanceStartDate]             DATETIME       NULL,
    [AllowanceEndDate]               DATETIME       NULL,
    [DiscountStartDate]              DATETIME       NULL,
    [DiscountEndDate]                DATETIME       NULL,
    [MixMatch]                       INT            NULL,
    [Venue]                          VARCHAR (100)  NULL,
    [VisualVerify]                   BIT            NULL,
    [VendorUnits]                    INT            NULL,
    [RequestedBy]                    VARCHAR (255)  NULL,
    [ProcessedBy]                    VARCHAR (255)  NULL,
    [Comments]                       VARCHAR (255)  NULL,
    [EmpDiscount]                    BIT            NULL,
    [Age_Restrict]                   BIT            DEFAULT ((0)) NULL,
    [CaseDistHandlingChargeOverride] SMALLMONEY     DEFAULT ((0)) NULL,
    [CatchWeightRequired]            BIT            DEFAULT ((0)) NULL,
    [Cool]                           BIT            DEFAULT ((0)) NOT NULL,
    [Bio]                            BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemRequest_ItemRequestID] PRIMARY KEY CLUSTERED ([ItemRequest_ID] ASC),
    CONSTRAINT [FK_ItemRequest_Type] FOREIGN KEY ([ItemType_ID]) REFERENCES [dbo].[ItemRequestIdentifier_Type] ([ItemType_ID]),
    CONSTRAINT [FK_ItemRequest_UserAccess] FOREIGN KEY ([UserAccessLevel_ID]) REFERENCES [dbo].[UserAccess] ([UserAccessLevel_ID]),
    CONSTRAINT [FK_ItemRequest_VendorRequest] FOREIGN KEY ([VendorRequest_ID]) REFERENCES [dbo].[VendorRequest] ([VendorRequest_ID])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemRequest] TO [IRMASLIMRole]
    AS [dbo];

