IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemNutritionOverride]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].[ItemNutritionOverride]

CREATE TABLE [dbo].[ItemNutritionOverride] (
    [ItemNutritionOverride_ID]  INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemKey]             INT NOT NULL,
    [StoreJurisdictionID] INT NOT NULL,
    [NutriFactsID]        INT NULL,
    [Scale_Ingredient_ID] INT NULL,
    [Scale_Allergen_ID]   INT NULL,
    [Item_ExtraText_ID]   INT NULL,
    CONSTRAINT [PK_ItemNutritionOverride] PRIMARY KEY CLUSTERED ([ItemNutritionOverride_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemNutritionOverride_StoreJurisdictionID] FOREIGN KEY ([StoreJurisdictionID]) REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID]),
    CONSTRAINT [FK_ItemNutritionOverride_Item_ExtraText_ID] FOREIGN KEY ([Item_ExtraText_ID]) REFERENCES [dbo].[Item_ExtraText] ([Item_ExtraText_ID]),
    CONSTRAINT [FK_ItemNutritionOverride_ItemKey] FOREIGN KEY ([ItemKey]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemNutritionOverride_NutriFactsID] FOREIGN KEY ([NutriFactsID]) REFERENCES [dbo].[NutriFacts] ([NutriFactsID]),
    CONSTRAINT [FK_ItemNutritionOverride_Scale_Allergen_ID] FOREIGN KEY ([Scale_Allergen_ID]) REFERENCES [dbo].[Scale_Allergen] ([Scale_Allergen_ID]),
    CONSTRAINT [FK_ItemNutritionOverride_Scale_Ingredient_ID] FOREIGN KEY ([Scale_Ingredient_ID]) REFERENCES [dbo].[Scale_Ingredient] ([Scale_Ingredient_ID])
);

GO
ALTER TABLE [dbo].[ItemNutritionOverride] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IRMAAdminRole]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IRMASupportRole]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IRMASchedJobsRole]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IRMAReportsRole]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IRMA_Teradata]
    AS [dbo];
GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IConInterface]
    AS [dbo];
GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IConInterface]
    AS [dbo];
GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IConInterface]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [IConInterface]
    AS [dbo];
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutritionOverride] TO [iCONReportingRole]
    AS [dbo];
