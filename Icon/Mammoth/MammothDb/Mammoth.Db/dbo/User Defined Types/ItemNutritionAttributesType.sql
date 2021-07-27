﻿CREATE TYPE [dbo].ItemNutritionAttributesType AS TABLE
(
	[ItemID]                   VARCHAR (50)    NULL,
    [RecipeName]               NVARCHAR (100)  NULL,
    [Allergens]                NVARCHAR (510)  NULL,
    [Ingredients]              NVARCHAR (4000) NULL,
    [ServingsPerPortion]       FLOAT (53)      NULL,
    [ServingSizeDesc]          NVARCHAR (50)   NULL,
    [ServingPerContainer]      NVARCHAR (50)   NULL,
    [HshRating]                INT             NULL,
    [ServingUnits]             TINYINT         NULL,
    [SizeWeight]               INT             NULL,
    [Calories]                 INT             NULL,
    [CaloriesFat]              INT             NULL,
    [CaloriesSaturatedFat]     INT             NULL,
    [TotalFatWeight]           DECIMAL (10, 1) NULL,
    [TotalFatPercentage]       SMALLINT        NULL,
    [SaturatedFatWeight]       DECIMAL (10, 1) NULL,
    [SaturatedFatPercent]      SMALLINT        NULL,
    [PolyunsaturatedFat]       DECIMAL (10, 1) NULL,
    [MonounsaturatedFat]       DECIMAL (10, 1) NULL,
    [CholesterolWeight]        DECIMAL (10, 1) NULL,
    [CholesterolPercent]       SMALLINT        NULL,
    [SodiumWeight]             DECIMAL (10, 1) NULL,
    [SodiumPercent]            SMALLINT        NULL,
    [PotassiumWeight]          DECIMAL (10, 1) NULL,
    [PotassiumPercent]         SMALLINT        NULL,
    [TotalCarbohydrateWeight]  DECIMAL (10, 1) NULL,
    [TotalCarbohydratePercent] SMALLINT        NULL,
    [DietaryFiberWeight]       DECIMAL (10, 1) NULL,
    [DietaryFiberPercent]      SMALLINT        NULL,
    [SolubleFiber]             DECIMAL (10, 1) NULL,
    [InsolubleFiber]           DECIMAL (10, 1) NULL,
    [Sugar]                    DECIMAL (10, 1) NULL,
    [SugarAlcohol]             DECIMAL (10, 1) NULL,
    [OtherCarbohydrates]       DECIMAL (10, 1) NULL,
    [ProteinWeight]            DECIMAL (10, 1) NULL,
    [ProteinPercent]           SMALLINT        NULL,
    [VitaminA]                 SMALLINT        NULL,
    [Betacarotene]             SMALLINT        NULL,
    [VitaminC]                 SMALLINT        NULL,
    [Calcium]                  SMALLINT        NULL,
    [Iron]                     SMALLINT        NULL,
    [VitaminD]                 SMALLINT        NULL,
    [VitaminE]                 SMALLINT        NULL,
    [Thiamin]                  SMALLINT        NULL,
    [Riboflavin]               SMALLINT        NULL,
    [Niacin]                   SMALLINT        NULL,
    [VitaminB6]                SMALLINT        NULL,
    [Folate]                   SMALLINT        NULL,
    [VitaminB12]               SMALLINT        NULL,
    [Biotin]                   SMALLINT        NULL,
    [PantothenicAcid]          SMALLINT        NULL,
    [Phosphorous]              SMALLINT        NULL,
    [Iodine]                   SMALLINT        NULL,
    [Magnesium]                SMALLINT        NULL,
    [Zinc]                     SMALLINT        NULL,
    [Copper]                   SMALLINT        NULL,
    [TransFat]                 DECIMAL (10, 1) NULL,
    [TransFatWeight]           DECIMAL (10, 1) NULL,
    [CaloriesFromTransFat]     INT             NULL,
    [Om6Fatty]                 DECIMAL (10, 1) NULL,
    [Om3Fatty]                 DECIMAL (10, 1) NULL,
    [Starch]                   DECIMAL (10, 1) NULL,
    [Chloride]                 SMALLINT        NULL,
    [Chromium]                 SMALLINT        NULL,
    [VitaminK]                 SMALLINT        NULL,
    [Manganese]                SMALLINT        NULL,
    [Molybdenum]               SMALLINT        NULL,
    [Selenium]                 SMALLINT        NULL,
    [AddedSugarsWeight]        DECIMAL (10, 1) NULL,
    [AddedSugarsPercent]       SMALLINT        NULL,
    [CalciumWeight]            DECIMAL (10, 1) NULL,
    [IronWeight]               DECIMAL (10, 1) NULL,
    [VitaminDWeight]           DECIMAL (10, 1) NULL,
    [ProfitCenter]             INT             NULL,
    [CanadaAllergens]          NVARCHAR (510)  NULL,
    [CanadaIngredients]        NVARCHAR (4000) NULL,
    [CanadaSugarPercent]       SMALLINT        NULL
)
GO

GRANT EXEC ON type::dbo.ItemNutritionAttributesType TO MammothRole
GO