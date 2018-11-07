CREATE TABLE [dbo].[ItemNutrition] (
    [ItemNutritionId]     INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemKey]             INT NOT NULL,
    [NutriFactsID]        INT NULL,
    [Scale_Ingredient_ID] INT NULL,
    [Scale_Allergen_ID]   INT NULL,
    [Item_ExtraText_ID]   INT NULL,
    CONSTRAINT [PK_ItemNutrition] PRIMARY KEY CLUSTERED ([ItemNutritionId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemNutrition_Item_ExtraText_ID] FOREIGN KEY ([Item_ExtraText_ID]) REFERENCES [dbo].[Item_ExtraText] ([Item_ExtraText_ID]),
    CONSTRAINT [FK_ItemNutrition_ItemKey] FOREIGN KEY ([ItemKey]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemNutrition_NutriFactsID] FOREIGN KEY ([NutriFactsID]) REFERENCES [dbo].[NutriFacts] ([NutriFactsID]),
    CONSTRAINT [FK_ItemNutrition_Scale_Allergen_ID] FOREIGN KEY ([Scale_Allergen_ID]) REFERENCES [dbo].[Scale_Allergen] ([Scale_Allergen_ID]),
    CONSTRAINT [FK_ItemNutrition_Scale_Ingredient_ID] FOREIGN KEY ([Scale_Ingredient_ID]) REFERENCES [dbo].[Scale_Ingredient] ([Scale_Ingredient_ID])
);


GO
ALTER TABLE [dbo].[ItemNutrition] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemNutrition] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemNutrition] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemNutrition] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemNutrition] TO [iCONReportingRole]
    AS [dbo];

