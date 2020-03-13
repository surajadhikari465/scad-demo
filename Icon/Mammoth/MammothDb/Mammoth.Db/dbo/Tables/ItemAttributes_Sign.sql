﻿CREATE TABLE [dbo].[ItemAttributes_Sign] (
    [ItemAttributeID]       INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]                INT            NOT NULL,
    [CheeseMilkType]        NVARCHAR (255) NULL,
    [Agency_GlutenFree]     NVARCHAR (255) NULL,
    [Agency_Kosher]         NVARCHAR (255) NULL,
    [Agency_NonGMO]         NVARCHAR (255) NULL,
    [Agency_Organic]        NVARCHAR (255) NULL,
    [Agency_Vegan]          NVARCHAR (255) NULL,
    [IsAirChilled]          BIT            NULL,
    [IsBiodynamic]          BIT            NULL,
    [IsCheeseRaw]           BIT            NULL,
    [IsDryAged]             BIT            NULL,
    [IsFreeRange]           BIT            NULL,
    [IsGrassFed]            BIT            NULL,
    [IsMadeInHouse]         BIT            NULL,
    [IsMsc]                 BIT            NULL,
    [IsPastureRaised]       BIT            NULL,
    [IsPremiumBodyCare]     BIT            NULL,
    [IsVegetarian]          BIT            NULL,
    [IsWholeTrade]          BIT            NULL,
    [Rating_AnimalWelfare]  NVARCHAR (255) NULL,
    [Rating_EcoScale]       NVARCHAR (255) NULL,
    [Rating_HealthyEating]  NVARCHAR (255) NULL,
    [Seafood_FreshOrFrozen] NVARCHAR (255) NULL,
    [Seafood_CatchType]     NVARCHAR (255) NULL,
    [AddedDate]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]          DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Sign] PRIMARY KEY CLUSTERED ([ItemAttributeID] ASC) WITH (FILLFACTOR = 100)
);
GO

CREATE NONCLUSTERED INDEX [IX_ItemAttributesSign_ItemId] ON ItemAttributes_Sign (ItemID)
GO

GRANT SELECT, UPDATE, INSERT, DELETE ON dbo.ItemAttributes_Sign TO MammothRole
GO
