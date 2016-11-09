--WFM\nutriconservicedev' account should belogn to NutritionRole
--IF (SELECT name FROM sys.server_principals WHERE name = 'WFM\NutriconServiceDev') IS NULL
--CREATE LOGIN [WFM\NutriconServiceDev] FROM WINDOWS;
--IF (SELECT name FROM sys.database_principals WHERE name = 'WFM\nutriconservicedev') IS NULL
--CREATE USER [WFM\nutriconservicedev] FROM LOGIN [WFM\nutriconservicedev];
--IF (SELECT name FROM sys.database_principals WHERE name = 'nutritionrole') IS NOT NULL
--ALTER ROLE nutritionrole ADD MEMBER [WFM\nutriconservicedev];

GRANT DELETE ON SCHEMA::nutrition TO [NutritionRole]
GRANT INSERT ON SCHEMA::nutrition TO [NutritionRole]
GRANT SELECT ON SCHEMA::nutrition TO [NutritionRole]
GRANT UPDATE ON SCHEMA::nutrition TO [NutritionRole]
GRANT EXECUTE ON SCHEMA::nutrition TO [NutritionRole]

GRANT SELECT ON app.App TO [NutritionRole]
GRANT INSERT ON app.AppLog TO [NutritionRole]
GRANT SELECT ON App.AppLog TO [NutritionRole]
GRANT SELECT ON app.EventType TO [NutritionRole]
GRANT INSERT ON app.EventQueue TO [NutritionRole]
GRANT SELECT ON app.Settings TO [NutritionRole]
GRANT SELECT ON app.RegionalSettings TO [NutritionRole]
GRANT SELECT ON app.Regions TO [NutritionRole]
GRANT SELECT ON app.IRMAItemSubscription TO [NutritionRole]
GRANT SELECT ON dbo.ItemSignAttribute TO [NutritionRole]
GRANT INSERT ON dbo.ItemSignAttribute TO [NutritionRole]
GRANT SELECT ON dbo.ScanCode TO [NutritionRole]
GRANT SELECT on dbo.ItemTrait TO [NutritionRole]
GRANT SELECT on dbo.Trait TO [NutritionRole]
GRANT SELECT on dbo.HealthyEatingRating TO [NutritionRole]
GRANT EXECUTE ON dbo.GenerateItemUpdateMessages TO [NutritionRole]
GRANT SELECT ON app.MessageQueueProduct TO [NutritionRole]
GRANT INSERT ON app.MessageQueueProduct TO [NutritionRole]
GRANT UPDATE ON app.MessageQueueProduct TO [NutritionRole]
GRANT SELECT ON app.MessageQueueNutrition TO [NutritionRole]
GRANT INSERT ON app.MessageQueueNutrition TO [NutritionRole]
GRANT UPDATE ON app.MessageQueueNutrition TO [NutritionRole]



