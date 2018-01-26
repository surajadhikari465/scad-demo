CREATE TYPE [dbo].ItemSignAttributesType AS TABLE
(
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
    [Seafood_CatchType]     NVARCHAR (255) NULL
)
GO

GRANT EXEC ON type::dbo.ItemSignAttributesType TO MammothRole
GO