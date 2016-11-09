﻿CREATE TYPE [dbo].[IconUpdateItemType] AS TABLE (
    [ItemId]                INT            NOT NULL,
    [ValidationDate]        NVARCHAR (255) NOT NULL,
    [ScanCode]              NVARCHAR (13)  NOT NULL,
    [ScanCodeType]          NVARCHAR (255) NOT NULL,
    [ProductDescription]    NVARCHAR (255) NOT NULL,
    [PosDescription]        NVARCHAR (255) NOT NULL,
    [PackageUnit]           NVARCHAR (255) NOT NULL,
    [FoodStampEligible]     NVARCHAR (255) NOT NULL,
    [Tare]                  NVARCHAR (255) NULL,
    [BrandId]               INT            NOT NULL,
    [BrandName]             NVARCHAR (35)  NOT NULL,
    [TaxClassName]          NVARCHAR (50)  NOT NULL,
    [NationalClassCode]     NVARCHAR (50)  NOT NULL,
    [SubTeamName]           NVARCHAR (255) NULL,
    [SubTeamNo]             INT            NOT NULL,
    [DeptNo]                INT            NOT NULL,
    [SubTeamNotAligned]     BIT            NOT NULL,
    [EventTypeId]           INT            NULL,
    [AnimalWelfareRating]   NVARCHAR (10)  NULL,
    [Biodynamic]            BIT            NULL,
    [CheeseMilkType]        NVARCHAR (40)  NULL,
    [CheeseRaw]             BIT            NULL,
    [EcoScaleRating]        NVARCHAR (30)  NULL,
    [GlutenFree]            BIT            NULL,
    [Kosher]                BIT            NULL,
    [NonGmo]                BIT            NULL,
    [Organic]               BIT            NULL,
    [PremiumBodyCare]       BIT            NULL,
    [FreshOrFrozen]         NVARCHAR (30)  NULL,
    [SeafoodCatchType]      NVARCHAR (15)  NULL,
    [Vegan]                 BIT            NULL,
    [Vegetarian]            BIT            NULL,
    [WholeTrade]            BIT            NULL,
    [Msc]                   BIT            NULL,
    [GrassFed]              BIT            NULL,
    [PastureRaised]         BIT            NULL,
    [FreeRange]             BIT            NULL,
    [DryAged]               BIT            NULL,
    [AirChilled]            BIT            NULL,
    [MadeInHouse]           BIT            NULL,
    [HasItemSignAttributes] BIT            NULL,
    [RetailSize]            DECIMAL (9, 4) NULL,
    [RetailUom]             VARCHAR (5)    NULL,
    PRIMARY KEY CLUSTERED ([ItemId] ASC));




GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconUpdateItemType] TO [IConInterface];

