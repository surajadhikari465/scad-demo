CREATE TABLE [dbo].[Scale_Allergen] (
    [Scale_Allergen_ID]  INT            IDENTITY (1, 1) NOT NULL,
    [Scale_LabelType_ID] INT            NULL,
    [Description]        VARCHAR (50)   NOT NULL,
    [Allergens]          VARCHAR (4200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Scale_Allergen_ID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([Scale_LabelType_ID]) REFERENCES [dbo].[Scale_LabelType] ([Scale_LabelType_ID])
);


GO
ALTER TABLE [dbo].[Scale_Allergen] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRSUser]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Scale_Allergen] TO [iCONReportingRole]
    AS [dbo];

