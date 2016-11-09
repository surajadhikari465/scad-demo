CREATE TABLE [dbo].[Scale_Ingredient] (
    [Scale_Ingredient_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Scale_LabelType_ID]  INT            NULL,
    [Description]         VARCHAR (50)   NOT NULL,
    [Ingredients]         VARCHAR (4200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Scale_Ingredient_ID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([Scale_LabelType_ID]) REFERENCES [dbo].[Scale_LabelType] ([Scale_LabelType_ID])
);


GO
ALTER TABLE [dbo].[Scale_Ingredient] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRSUser]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Ingredient] TO [iCONReportingRole]
    AS [dbo];

